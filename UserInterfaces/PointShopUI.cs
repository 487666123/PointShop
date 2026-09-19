using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Common.Tweening;
using SilkyUIFramework.Extensions;
using SilkyUIFramework.StyleSystem;

namespace PointShop.UserInterfaces;

[RegisterUI]
public partial class PointShopUI : BaseBody
{
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

    private string _keywords = "";
    private bool ShopItemFilters(ShopItem shopItem) => shopItem.DisplayName.Contains(_keywords.Trim());
    public static bool ShopItemTableIsDirty { get; set; } = true;

    protected override void OnInitialize()
    {
        InitializeComponent();

        BorderColor = SUIColor.Border;
        BackgroundColor = SUIColor.Background * 0.75f;

        Header.ControlTarget = this;
        Header.Title.Text = $"{PSHelper.GetTextByPointShop("DisplayName")}";

        Header.CloseButton.LeftMouseDown += (s, e) => Close();

        UpdateMenuList();

        SearchBar.BorderColor = SUIColor.Border * 0.75f;
        SearchBar.BackgroundColor = SUIColor.Background * 0.25f;

        FilterButton.Text = $"{PSHelper.GetTextByPointShop("NameFilter")}";
        FilterButton.BackgroundColor = SUIColor.Background * 0.5f;

        FilterButton.LeftMouseDown += (s, e) => FilterTable.Invalid = !FilterTable.Invalid;

        SearchBox.Placeholder = PSHelper.GetTextByPointShop("ItemSearchTips").Value;
        SearchBox.BackgroundColor = SUIColor.Border * 0.25f;
        SearchBox.ContentChanged += (sender, e) =>
        {
            _keywords = SearchBox.Text;
            ShopItemTableIsDirty = true;
        };

        ClearButton.Text = $"{PSHelper.GetTextByPointShop("Clear")}";
        ClearButton.LeftMouseDown += (_, _) => SearchBox.Text = string.Empty;

        foreach (var item in TagContainer.Children.Concat([FilterButton, ClearButton]))
        {
            item.StyleSheet.AllTransition.Duration = 0.2f;

            item.StyleSheet.SetStyle(UIElementState.Normal, new StyleDefinition()
            {
                [$"{nameof(BackgroundColor)}"] = Color.Black * 0.2f,
            });

            item.StyleSheet.SetStyle(UIElementState.Hover, new StyleDefinition()
            {
                [$"{nameof(BackgroundColor)}"] = Color.Black * 0.3f,
            });
        }

        ShopItemTableScrollView.Container.SetGap(4);

        ShopItemTableIsDirty = true;
    }

    private Tween _animTween;

    public static void Toggle()
    {
        if (!UISceneManager.Instance.TryGetInstance<PointShopUI>(out var ui)) return;

        if (ui._isOpen) ui.Close(); else ui.Open();
    }

    // 是否展开
    private bool _isOpen;

    public void Open() => PlayExpandAnimation(true, 1f, 1f);
    public void Close() => PlayExpandAnimation(false, 0f, 0.9f);

    private void PlayExpandAnimation(bool open, float fadeTo, float scaleTo)
    {
        _animTween?.Kill();
        _isOpen = open;

        Enabled = true;
        UseRenderTarget = true;

        _animTween = CreateTween().Parallel().SetTrans(TransitionType.Expo).SetEase(EaseType.Out);
        _animTween.FadeTo(this, fadeTo, 0.2f);
        _animTween.MemberTo(this, nameof(_matrixScale), scaleTo, 0.2f);
        _animTween.OnFinished += () =>
        {
            if (!open) Enabled = false;
            UseRenderTarget = false;
        };
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        UpdateShopItemTable();
    }

    private float _matrixScale;
    protected override void DrawWithRenderTarget(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var center = Bounds.Center * Main.UIScale;
        RenderTargetMatrix =
            Matrix.CreateTranslation(-center.X, -center.Y, 0) *
            Matrix.CreateScale(_matrixScale, _matrixScale, 1) *
            Matrix.CreateTranslation(center.X, center.Y, 0);

        base.DrawWithRenderTarget(gameTime, spriteBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (PointShopSystem.TryGetGameEnvironment(CurrentEnvironmentName, out var environment))
        {
            ShopFooter.EnvironmentName.Text = environment.DisplayName;
            ShopFooter.Point = environment.GetPlayerPoints();
        }
    }

    public void UpdateMenuList()
    {
        var environments = PointShopSystem.Environments;

        for (int i = 0; i < environments.Count; i++)
        {
            var environment = environments[i];
            var button = new SUIMenuComponent(environment).Join(MenuListScrollView.Container);

            button.OnEnvironmentChanged(null, CurrentEnvironmentName);

            button.LeftMouseDown += (_, _) => CurrentEnvironmentName = environment.Name;

            if (i + 1 != environments.Count)
            {
                SUIDividingLine.Horizontal(SUIColor.Border * 0.75f).Join(MenuListScrollView.Container);
            }
        }
    }

    /// <summary>
    /// 更新物品表格
    /// </summary>
    public void UpdateShopItemTable()
    {
        if (!ShopItemTableIsDirty) return;
        ShopItemTableIsDirty = false;

        if (!PointShopSystem.TryGetGameEnvironment(CurrentEnvironmentName, out var environment)) return;

        ShopItemTableScrollView.Container.RemoveAllChildren();
        ShopItemTableScrollView.ScrollToStart(false);

        var items = environment.ShopItemList;

        var tween = CreateTween().Parallel().SetTrans(TransitionType.Expo).SetEase(EaseType.Out);
        var delay = 0f;
        foreach (var item in items.Where(ShopItemFilters))
        {
            var view = (item is SimpleShopItem simpleShopItem) ?
                new SUISimpleShopItem(simpleShopItem) :
                new SUIShopItemComponent(item);

            ShopItemTableScrollView.Container.AddChild(view);

            view.SetTop(pixels: 50f);
            tween.MemberTo(view, nameof(view.Top), new Anchor(), 0.2f).SetDelay(delay);
            delay += 0.005f;
        }
    }
}