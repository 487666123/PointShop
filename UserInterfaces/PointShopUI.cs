using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Common.Tweening;
using SilkyUIFramework.Extensions;
using SilkyUIFramework.StyleSystem;

namespace PointShop.UserInterfaces;

[RegisterUI]
public partial class PointShopUI : BaseBody
{
    public override bool Enabled { get; set; }

    public static event EventHandler<string> EnvironmentNameChanged;

    /// <summary>
    /// 商店 UI 中当前显示的环境商店内部名称
    /// </summary>
    public static string CurrentEnvironmentName
    {
        get; set
        {
            if (field == value) return;
            field = value;
            ShopItemTableIsDirty = true;
            EnvironmentNameChanged?.Invoke(null, field);
        }
    } = "Forest";

    /// <summary> 是否可交互 (不影响绘制) </summary>
    public override bool IsInteractable
    {
        get
        {
            if (_animTween is null) return true;
            return _animTween.IsFinished;
        }
    }

    protected override void OnInitialize()
    {
        InitializeComponent();

        EnableBlur = true;
        BorderColor = SUIColor.Border;
        BackgroundColor = SUIColor.Background * 0.75f;
        OverflowHidden = true;
        IndependentRenderTarget = true;

        Header.ControlTarget = this;
        Header.Title.Text = $"{PSHelper.GetTextByPointShop("DisplayName")}";

        Header.CloseButton.LeftMouseDown += delegate { Close(); };

        UpdateMenuList();

        SearchBar.Border = 2;
        SearchBar.BorderColor = SUIColor.Border * 0.75f;
        SearchBar.BackgroundColor = SUIColor.Background * 0.25f;

        SearchLeftText.Text = $"{PSHelper.GetTextByPointShop("NameFilter")}";
        SearchLeftText.BackgroundColor = SUIColor.Background * 0.5f;
        SearchLeftText.LeftMouseDown += delegate
        {
            FilterTable.Invalid = !FilterTable.Invalid;
        };
        SearchLeftText.OnUpdateStatus += delegate
        {
            SearchLeftText.BackgroundColor = SearchLeftText.HoverTimer.Lerp(Color.Black * 0.1f, Color.Black * 0.25f);
        };

        SearchBox.Placeholder = PSHelper.GetTextByPointShop("ItemSearchTips").Value;
        SearchBox.BackgroundColor = SUIColor.Border * 0.25f;
        SearchBox.CursorFlashColor = Color.White;
        SearchBox.ContentChanged += (sender, e) =>
        {
            _keywords = SearchBox.Text;
            ShopItemTableIsDirty = true;
        };

        ClearSearchButton.Text = $"{PSHelper.GetTextByPointShop("Clear")}";
        ClearSearchButton.LeftMouseDown += (_, _) => SearchBox.Text = string.Empty;
        ClearSearchButton.OnUpdateStatus += delegate
        {
            ClearSearchButton.BackgroundColor = ClearSearchButton.HoverTimer.Lerp(Color.Black * 0.1f, Color.Black * 0.25f);
        };

        foreach (var item in FilterTable.Children)
        {
            item.StyleSheet.AllTransition.Duration = 0.2f;

            item.StyleSheet.SetStyle(UIElementState.Normal, new StyleDefinition()
            {
                [$"{nameof(BackgroundColor)}"] = Color.Black * 0.25f,
                [$"{nameof(Border)}"] = 2f
            });

            item.StyleSheet.SetStyle(UIElementState.Hover, new StyleDefinition()
            {
                [$"{nameof(BackgroundColor)}"] = Color.Black * 0.15f,
            });

            item.StyleSheet.SetStyle(UIElementState.Active, new StyleDefinition()
            {
                [$"{nameof(BackgroundColor)}"] = Color.Black * 0.05f,
            });
        }

        ShopItemTableScrollView.Container.SetGap(4);

        ShopItemTableIsDirty = true;
    }

    private Tween _animTween;

    public static void Toggle()
    {
        if (!UISceneManager.Instance.TryGetInstance<PointShopUI>(out var ui)) return;

        if (ui._expanded) ui.Close();
        else ui.Open();
    }

    // 是否展开
    private bool _expanded;

    public void Open()
    {
        _expanded = true;
        _animTween?.Kill();

        Enabled = true;
        UseRenderTarget = true;

        _animTween = CreateTween().Parallel().SetTrans(TransitionType.Expo).SetEase(EaseType.Out);
        _animTween.FadeTo(this, 1f, 0.2f);
        _animTween.MemberTo(this, nameof(_renderScale), 1f, 0.2f);
        _animTween.OnFinished += () => UseRenderTarget = false;
    }

    public void Close()
    {
        _expanded = false;
        _animTween?.Kill();

        UseRenderTarget = true;

        _animTween = CreateTween().Parallel().SetTrans(TransitionType.Expo).SetEase(EaseType.Out);
        _animTween.FadeTo(this, 0f, 0.2f);
        _animTween.MemberTo(this, nameof(_renderScale), 0.9f, 0.2f);
        _animTween.OnFinished += () =>
        {
            Enabled = false;
            UseRenderTarget = false;
        };
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        UpdateShopItemTable();
    }

    private float _renderScale;
    protected override void DrawWithRenderTarget(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var center = Bounds.Center * Main.UIScale;
        RenderTargetMatrix =
            Matrix.CreateTranslation(-center.X, -center.Y, 0) *
            Matrix.CreateScale(_renderScale, _renderScale, 1) *
            Matrix.CreateTranslation(center.X, center.Y, 0);

        base.DrawWithRenderTarget(gameTime, spriteBatch);
    }
}