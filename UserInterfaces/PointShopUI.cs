using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.Extensions;
using SilkyUIFramework.Graphics2D;

namespace PointShop.UserInterfaces;

[RegisterUI("Vanilla: Radial Hotbars", "PointShop: PointShopUI")]
public partial class PointShopUI : BasicBody
{
    /// <summary>
    /// 显示 UI，状态控制
    /// </summary>
    public static bool ShowUI { get; set; }

    /// <summary>
    /// 是否启用，包括事件与绘制
    /// </summary>
    public override bool Enabled
    {
        get
        {
            if (ShowUI) return true;
            return !SwitchTimer.IsReverseCompleted;
        }
        set => ShowUI = value;
    }

    /// <summary>
    /// 商店 UI 中当前显示的环境商店内部名称
    /// </summary>
    public static string CurrentEnvironmentName
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            ShopItemTableIsDirty = true;
        }
    } = "Forest";

    /// <summary>
    /// 是否可交互（不影响绘制）
    /// </summary>
    public override bool IsInteractable => SwitchTimer.IsCompleted;

    /// <summary>
    /// 菜单列表
    /// </summary>
    public SUIScrollView MenuListScrollView { get; private set; }
    /// <summary>
    /// 商店底部栏
    /// </summary>
    public PointShopFooter ShopFooter { get; private set; }
    /// <summary>
    /// 内容容器（所有内容的容器）
    /// </summary>
    public UIElementGroup ContentContainer { get; private set; }
    /// <summary>
    /// 商品表
    /// </summary>
    public SUIScrollView ShopItemTableScrollView { get; private set; }

    public SUIEditText SearchBox { get; private set; }

    protected override void OnInitialize()
    {
        BorderColor = SUIColor.Border;
        BackgroundColor = SUIColor.Background * 0.75f;

        new SUICommonHeader(this, $"{LanguageHelper.GetTextByPointShop("DisplayName")}").Join(this);

        InitializeComponent();

        // 菜单列表 and 商品列表

        ContentContainer = new UIElementGroup
        {
            LayoutType = LayoutType.Flexbox,
            FlexDirection = FlexDirection.Row,
            CrossAlignment = CrossAlignment.Stretch,
            CrossContentAlignment = CrossContentAlignment.Stretch,
            FlexWrap = false,
            Gap = new Vector2(0f),
        }.Join(this);
        ContentContainer.SetSize(0f, 450f, 1f);

        MenuListScrollView = new SUIScrollView
        {
            Gap = new Vector2(4f),
            Mask =
            {
                Border = 2,
                BorderRadius = new Vector4(4),
                BorderColor = Color.Black * 0.75f,
            },
            Container =
            {
                HiddenBox = HiddenBox.Inner,
                Gap = Size.Zero,
            }
        }.Join(ContentContainer);
        MenuListScrollView.SetPadding(4f);
        MenuListScrollView.SetSize(0f, 0f, 0.25f, 1f);

        UpdateMenuList();

        SUIDividingLine.Vertical(Color.Black * 0.75f).Join(ContentContainer);

        // 商品列表
        var rightContainer = new UIElementGroup
        {
            LayoutType = LayoutType.Flexbox,
            FlexWrap = false,
            FlexDirection = FlexDirection.Column,
            FlexGrow = 1f,
        }.Join(ContentContainer);
        rightContainer.SetHeight(0f, 1f);

        #region 过滤器

        var searchBarContainer = new UIElementGroup
        {
            LayoutType = LayoutType.Flexbox,
            MainAlignment = MainAlignment.Start,
            CrossAlignment = CrossAlignment.Center,
            CrossContentAlignment = CrossContentAlignment.Center,
            Gap = new Vector2(4),
            Padding = new Margin(4f, 4f, 4f, 0f),
        }.Join(rightContainer);
        searchBarContainer.SetWidth(0f, 1f);
        searchBarContainer.SetHeight(36f, 0f);

        var searchBar = new UIElementGroup
        {
            BorderRadius = new Vector4(4f),
            Border = 2,
            BorderColor = SUIColor.Border * 0.75f,
            BackgroundColor = SUIColor.Background * 0.25f,
            FlexGrow = 1f,
        }.Join(searchBarContainer);
        searchBar.SetHeight(0f, 1f);

        // 搜索文字
        var searchText = new UITextView
        {
            Text = $"{LanguageHelper.GetTextByPointShop("NameFilter")}",
            TextScale = 0.8f,
            TextAlign = new Vector2(0.5f),
            BorderRadius = new Vector4(2f, 0f, 2f, 0f),
            BackgroundColor = SUIColor.Background * 0.5f,
            FlexShrink = 1f,
            FitWidth = true,
            FitHeight = false,
        }.Join(searchBar);
        searchText.SetPadding(12f, 0f);
        searchText.SetHeight(0f, 1f);

        SUIDividingLine.Vertical(Color.Black * 0.75f).Join(searchBar);

        SearchBox = new SUIEditText
        {
            BackgroundColor = SUIColor.Border * 0.25f,
            TextAlign = new Vector2(0f, 0.5f),
            TextScale = 0.8f,
            CursorFlashColor = Color.White,
            FlexGrow = 1f,
            FitWidth = false,
            FitHeight = false,
        }.Join(searchBar);
        SearchBox.ContentChanged += (sender, e) =>
        {
            _keywords = SearchBox.Text;
            ShopItemTableIsDirty = true;
        };
        SearchBox.SetPadding(8f);
        SearchBox.SetHeight(0f, 1f);

        SUIDividingLine.Vertical(Color.Black * 0.75f).Join(searchBar);

        // 清空
        var clearText = new UITextView
        {
            Text = $"{LanguageHelper.GetTextByPointShop("Clear")}",
            TextScale = 0.8f,
            TextAlign = new Vector2(0.5f),
            BorderRadius = new Vector4(0f, 2f, 0f, 2f),
            BackgroundColor = SUIColor.Background * 0.5f,
            FitWidth = true,
            FitHeight = false,
        }.Join(searchBar);
        clearText.LeftMouseDown += (_, _) => SearchBox.Text = string.Empty;
        clearText.SetPadding(12f, 0f);
        clearText.SetHeight(0f, 1f);

        #endregion

        // 商品表格
        ShopItemTableScrollView = new SUIScrollView
        {
            Gap = new Vector2(4),
            FlexGrow = 1f,
        }.Join(rightContainer);
        ShopItemTableScrollView.SetPadding(4f);
        ShopItemTableScrollView.SetWidth(0f, 1f);

        ShopItemTableScrollView.Container.Gap = new Vector2(4);
        //ShopItemTable.Container.TemplateColumns = [.. TemplateUnit.Repeat(4, 0f, 1f)];
        //ShopItemTable.Container.TemplateRows = [.. TemplateUnit.Repeat(1, 160f)];

        SUIDividingLine.Horizontal(Color.Black * 0.75f).Join(this);
        ShopFooter = new PointShopFooter().Join(this);
    }

    public readonly AnimationTimer SwitchTimer = new(3);

    protected override void UpdateStatus(GameTime gameTime)
    {
        if (ShowUI) SwitchTimer.StartUpdate();
        else SwitchTimer.StartReverseUpdate();

        SwitchTimer.Update(gameTime);

        if (ShopItemTableIsDirty)
        {
            UpdateShopItemTable();
            ShopItemTableIsDirty = false;
        }

        UseRenderTarget = SwitchTimer.IsUpdating;
        Opacity = SwitchTimer.Lerp(0f, 1f);

        var center = Bounds.Center * Main.UIScale;
        RenderTargetMatrix =
            Matrix.CreateTranslation(-center.X, -center.Y, 0) *
            Matrix.CreateScale(SwitchTimer.Lerp(0.95f, 1f), SwitchTimer.Lerp(0.95f, 1f), 1) *
            Matrix.CreateTranslation(center.X, center.Y, 0);

        //UseRenderTarget = true;
        //Opacity = 0f;

        base.UpdateStatus(gameTime);

        //var batch = Main.spriteBatch;

        //var device = Main.graphics.GraphicsDevice;
        //device.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

        //batch.End();

        //// 模糊
        //KawaseBlur(Main.screenTarget, 10f, 2f, BlurType.Two);

        //batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.Identity);

        //device.SetRenderTarget(null);
        //Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.DiscardContents;

        //var bounds = Bounds;
        //bounds.X *= 2f;
        //bounds.Y *= 2f;
        //bounds.Width *= 2f;
        //bounds.Height *= 2f;
        //var rect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);

        //batch.Draw(Main.screenTarget, bounds.Position, rect, Color.White, 0f, Vector2.Zero, 1f, 0, 1f);

        //batch.Draw(TextureAssets.MagicPixel.Value,
        //    bounds.Position + new Vector2(-2f, -2f),
        //    new Rectangle(0, 0, 4, (int)bounds.Height + 6), Color.White, 0f, Vector2.Zero, 1f, 0, 1f);

        //batch.Draw(TextureAssets.MagicPixel.Value,
        //    bounds.Position + new Vector2(bounds.Width, -2f),
        //    new Rectangle(0, 0, 4, (int)bounds.Height + 6), Color.White, 0f, Vector2.Zero, 1f, 0, 1f);

        //batch.Draw(TextureAssets.MagicPixel.Value,
        //    bounds.Position + new Vector2(0f, -2f),
        //    new Rectangle(0, 0, (int)bounds.Width, 4), Color.White, 0f, Vector2.Zero, 1f, 0, 1f);

        //batch.Draw(TextureAssets.MagicPixel.Value,
        //    bounds.Position + new Vector2(0f, bounds.Height),
        //    new Rectangle(0, 0, (int)bounds.Width, 4), Color.White, 0f, Vector2.Zero, 1f, 0, 1f);

        //batch.End();
        //batch.Begin(SpriteSortMode.Deferred, null, null, null, SilkyUI.RasterizerStateForOverflowHidden, null, Main.UIScaleMatrix);
    }

    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (BlurMakeSystem.BlurAvailable)
        {
            if (BlurMakeSystem.SingleBlur)
            {
                var batch = Main.spriteBatch;
                batch.End();
                BlurMakeSystem.KawaseBlur();
                batch.Begin(SpriteSortMode.Deferred, null, null, null, SilkyUI.RasterizerStateForOverflowHidden, null,
                    SilkyUI.TransformMatrix);
            }

            SDFRectangle.SampleVersion(BlurMakeSystem.BlurRenderTarget,
                Bounds.Position * Main.UIScale, Bounds.Size * Main.UIScale, BorderRadius * Main.UIScale,
                Matrix.Identity);
        }

        base.Draw(gameTime, spriteBatch);
    }
}