using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Tweening;

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
        Header.Title.Text = $"{LanguageHelper.GetTextByPointShop("DisplayName")}";

        Header.CloseButton.LeftMouseDown += delegate { Close(); };

        MenuListScrollView.Mask.Border = 2;
        MenuListScrollView.Mask.BorderRadius = new Vector4(4);
        MenuListScrollView.Mask.BorderColor = Color.Black * 0.75f;
        MenuListScrollView.Container.HiddenBox = HiddenBox.Inner;
        MenuListScrollView.Container.Gap = Size.Zero;

        UpdateMenuList();

        SearchBar.Border = 2;
        SearchBar.BorderColor = SUIColor.Border * 0.75f;
        SearchBar.BackgroundColor = SUIColor.Background * 0.25f;

        SearchLeftText.Text = $"{LanguageHelper.GetTextByPointShop("NameFilter")}";
        SearchLeftText.BackgroundColor = SUIColor.Background * 0.5f;

        SearchBox.Placeholder = LanguageHelper.GetTextByPointShop("ItemSearchTips").Value;
        SearchBox.BackgroundColor = SUIColor.Border * 0.25f;
        SearchBox.CursorFlashColor = Color.White;
        SearchBox.ContentChanged += (sender, e) =>
        {
            _keywords = SearchBox.Text;
            ShopItemTableIsDirty = true;
        };

        ClearSearchButton.Text = $"{LanguageHelper.GetTextByPointShop("Clear")}";
        ClearSearchButton.LeftMouseDown += (_, _) => SearchBox.Text = string.Empty;

        ShopItemTableScrollView.Container.SetGap(4);

        ShopItemTableIsDirty = true;
    }

    private Tween _animTween;

    public static void Toggle()
    {
        if (!SilkyUIManager.Instance.TryGetInstance<PointShopUI>(out var ui)) return;

        if (ui.IsOpen) ui.Close();
        else ui.Open();
    }

    public bool IsOpen { get; set; }

    public void Open()
    {
        IsOpen = true;
        Enabled = true;
        UseRenderTarget = true;

        _animTween?.Kill();
        _animTween = CreateTween().Parallel();
        _animTween.TweenProperty(opacity => Opacity = opacity, () => Opacity, 1f, 0.2f, MathHelper.Lerp)
            .SetTrans(TransitionType.Expo).SetEase(EaseType.Out);
        _animTween.TweenProperty(renderScale => _renderScale = renderScale, () => _renderScale, 1f, 0.2f, MathHelper.Lerp)
            .SetTrans(TransitionType.Expo).SetEase(EaseType.Out);
        _animTween.OnFinished += () => UseRenderTarget = false;
    }

    public void Close()
    {
        IsOpen = false;
        UseRenderTarget = true;

        _animTween?.Kill();
        _animTween = CreateTween().Parallel();
        _animTween.TweenProperty(opacity => Opacity = opacity, () => Opacity, 0f, 0.2f, MathHelper.Lerp)
            .SetTrans(TransitionType.Quint).SetEase(EaseType.Out);
        _animTween.TweenProperty(renderScale => _renderScale = renderScale, () => _renderScale, 0.9f, 0.2f, MathHelper.Lerp)
            .SetTrans(TransitionType.Quint).SetEase(EaseType.Out);
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