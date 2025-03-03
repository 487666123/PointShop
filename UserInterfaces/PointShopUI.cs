using PointShop.Items;
using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.BasicElements;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

[AutoloadUI("Vanilla: Radial Hotbars", "PointShop: PointShopUI")]
public partial class PointShopUI : BasicBody
{
    public static bool OpenUI { get; set; } = false;
    public override bool Enabled
    {
        get
        {
            if (OpenUI) return OpenUI;
            return !SwitchTimer.ReverseUpdateCompleted;
        }
        set => OpenUI = value;
    }

    public string CurrentEnvironmentName { get; set; } = "Forest";

    public SUIDraggableView MainPanel { get; private set; }
    public SUIScrollView MenuList { get; private set; }
    public View Header { get; private set; }
    public View Footer { get; private set; }
    public View ContentContainer { get; private set; }
    public SUIScrollView ShopItemTable { get; private set; }

    public bool IsLayoutDirty { get; set; } = true;
    public void MakeLayoutDirty() => IsLayoutDirty = true;

    public override void OnInitialize()
    {
        MainPanel = new SUIDraggableView
        {
            // DragIncrement = new Vector2(5f),
            Border = 2,
            Draggable = true,
            FlexWrap = false,
            Display = Display.Flexbox,
            LayoutDirection = LayoutDirection.Column,
            Gap = new Vector2(0f),
            CornerRadius = new Vector4(8f),
            DragOffset = new Vector2(630f, 20f),
        }.Join(this);
        MainPanel.SetWidth(700);
        MainPanel.SetPadding(0f);

        CreateHeader();

        // 菜单列表 and 商品列表

        ContentContainer = new View
        {
            Display = Display.Flexbox,
            LayoutDirection = LayoutDirection.Row,
            FlexWrap = false,
            Gap = new Vector2(0f),
        }.Join(MainPanel);
        ContentContainer.SetSize(0f, 400f, 1f);

        #region 菜单

        MenuList = new SUIScrollView
        {
            Gap = new Vector2(4f),
        }.Join(ContentContainer);
        MenuList.SetPadding(4f);
        MenuList.SetSize(0f, 0f, 0.25f, 1f);
        MenuList.Container.HiddenBox = HiddenBox.Inner;
        MenuList.Container.Gap = Vector2.Zero;
        MenuList.Container.CornerRadius = new Vector4(4);
        MenuList.Container.Border = 2;
        MenuList.Container.BorderColor = Color.Black * 0.75f;

        UpdateMenuList();

        SUIDividingLine.Vertical(Color.Black * 0.5f).Join(ContentContainer);

        #endregion

        // 商品列表
        var rightContainer = new View
        {
            Display = Display.Flexbox,
            FlexWrap = false,
            LayoutDirection = LayoutDirection.Column,
            FlexWeight = { Enable = true, Value = 1f },
        }.Join(ContentContainer);
        rightContainer.SetSize(0f, 0f, 0f, 1f);

        #region 过滤器

        // var filtersContainer = new View()
        // {
        //     Display = SilkyUIFramework.BasicElements.Display.Flexbox,
        //     Gap = new Vector2(4f),
        //     LayoutDirection = LayoutDirection.Row,
        //     FlexWrap = false,
        // }.Join(productContainer);
        // filtersContainer.SetPadding(4f);
        // filtersContainer.SetSize(0f, 35f, 1f);

        // string[] strs = ["武器", "药水", "宠物", "道具"];
        // for (int i = 0; i < strs.Length; i++)
        // {
        //     var tag = new SUIText
        //     {
        //         CornerRadius = new Vector4(4f),
        //         BgColor = Color.Black * 0.2f,
        //         Text = strs[i],
        //         TextScale = 0.7f,
        //         TextAlign = new Vector2(0.5f),
        //         FlexWeight = { Enable = true, Value = 1f },
        //     }.Join(filtersContainer);
        //     tag.SetSize(0f, 0f, 1f, 1f);
        //     tag.SetPadding(12f);
        // }

        // SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(productContainer);

        #endregion

        // 商品表格
        ShopItemTable = new SUIScrollView
        {
            Gap = new Vector2(4),
            FlexWeight = { Enable = true, Value = 1f },
        }.Join(rightContainer);
        ShopItemTable.SetPadding(4f);
        ShopItemTable.SetSize(0f, 0f, 1f, 1f);

        ShopItemTable.Container.Gap = new Vector2(4);
        ShopItemTable.Container.Display = Display.Grid;
        ShopItemTable.Container.TemplateColumns = [.. TemplateUnit.Repeat(4, 0f, 1f)];
        ShopItemTable.Container.TemplateRows = [.. TemplateUnit.Repeat(1, 160f)];

        UpdateShopItemTable("Forest", item => true);

        CreateFooter();
    }

    public void CreateHeader()
    {
        Header = new View
        {
            Display = Display.Flexbox,
            LayoutDirection = LayoutDirection.Row,
            FlexWrap = false,
            Gap = new Vector2(4f),
            BgColor = Color.Black * 0.25f,
            CornerRadius = new Vector4(6f, 6f, 0f, 0f),
        }.Join(MainPanel);
        Header.SetSize(0f, 45f, 1f);
        Header.PaddingLeft = 12f;
        Header.PaddingRight = 12f;

        var titleText = new SUIText
        {
            Text = $"{LanguageHelper.GetTextByPointShop("DisplayName")}",
            TextScale = 0.45f,
            TextAlign = new Vector2(0.5f),
        }.Join(Header);
        titleText.SetHeight(0, 1f);
        titleText.UseDeathText();

        SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(MainPanel);
    }

    public void CreateFooter()
    {
        SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(MainPanel);

        Footer = new View
        {
            Display = Display.Flexbox,
            LayoutDirection = LayoutDirection.Row,
            MainAlignment = MainAlignment.SpaceBetween,
            Gap = new Vector2(4f),
            BgColor = Color.Black * 0.25f,
            CornerRadius = new Vector4(0f, 0f, 6f, 6f),
        }.Join(MainPanel);
        Footer.SetSize(0f, 30f, 1f);
        Footer.PaddingLeft = 12f;
        Footer.PaddingRight = 12f;

        BalanceContainer = new View
        {
            Display = Display.Flexbox,
            LayoutDirection = LayoutDirection.Row,
        }.Join(Footer);
        BalanceContainer.SetHeight(0, 1f);

        EnvironmentName = new SUIText()
        {
            TextAlign = new Vector2(0.5f, 0.5f),
            TextScale = 0.8f,
            TextColor = Color.White,
        }.Join(BalanceContainer);
        EnvironmentName.SetHeight(0f, 1f);

        // 价格左边的积分币 ItemSlot
        var coinSlot = new SUIItemSlot
        {
            Item = new Item(ModContent.ItemType<PointCoin>()),
            ItemAlign = new Vector2(0.5f),
            ItemScale = 0.8f,
            BgColor = Color.Transparent,
            BorderColor = Color.Transparent,
            ItemInteractive = false,
        }.Join(BalanceContainer);
        coinSlot.SetSize(24f, 0f, 0f, 1f);

        // 价格
        Balance = new SUIText()
        {
            TextAlign = new Vector2(0.5f, 0.5f),
            TextScale = 0.8f,
            TextColor = Color.White,
        }.Join(BalanceContainer);
        Balance.SetHeight(0f, 1f);

        // 版本号
        var titleText = new SUIText
        {
            Text = $"{LanguageHelper.GetTextByPointShop("DisplayName")} {ModContent.GetInstance<PointShop>().Version}",
            TextScale = 0.75f,
            TextAlign = new Vector2(0.5f),
        }.Join(Footer);
        titleText.SetHeight(0, 1f);
    }

    public View BalanceContainer { get; private set; }
    public SUIText EnvironmentName { get; private set; }
    public SUIText Balance { get; private set; }

    public readonly AnimationTimer SwitchTimer = new(3);

    protected override void UpdateAnimationTimer(GameTime gameTime)
    {
        base.UpdateAnimationTimer(gameTime);
        StartByStatus(SwitchTimer, OpenUI);
        SwitchTimer.Update((float)gameTime.ElapsedGameTime.TotalSeconds * 60f);
    }

    public static void StartByStatus(AnimationTimer timer, bool status)
    {
        if (status) { if (!timer.IsForward) timer.StartForwardUpdate(); }
        else if (!timer.IsReverse) timer.StartReverseUpdate();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        UseRenderTarget = SwitchTimer.Status is AnimationTimerStaus.ForwardUpdating or AnimationTimerStaus.ReverseUpdating;
        Opacity = SwitchTimer.Lerp(0f, 1f);

        var center = MainPanel.GetDimensions().Center();
        TransformMatrix =
            Matrix.CreateTranslation(-center.X, -center.Y, 0) *
            Matrix.CreateScale(SwitchTimer.Lerp(0.9f, 1f), SwitchTimer.Lerp(0.9f, 1f), 1) *
            Matrix.CreateTranslation(center.X, center.Y, 0);

        base.Draw(spriteBatch);
    }
}