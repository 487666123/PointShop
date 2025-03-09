using PointShop.Items;
using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

[RegisterUI("Vanilla: Radial Hotbars", "PointShop: PointShopUI")]
public partial class PointShopUI : BasicBody
{
    public static bool SwitchStatus { get; set; } = false;
    public override bool Enabled
    {
        get
        {
            if (SwitchStatus) return true;
            return !SwitchTimer.IsReverseCompleted;
        }
        set => SwitchStatus = value;
    }

    public string CurrentEnvironmentName { get; set; } = "Forest";

    public override bool IsInteractable => !SwitchTimer.IsReverse;

    public SUIDraggableView MainPanel { get; private set; }
    public SUIScrollView MenuList { get; private set; }
    public View Header { get; private set; }
    //public View Footer { get; private set; }
    public View ContentContainer { get; private set; }
    public SUIScrollView ShopItemTable { get; private set; }

    public SUIEditText SearchBox { get; private set; }

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
        MainPanel.SetPadding(0f);
        MainPanel.SetWidth(700);

        // 标题

        new SUICommonHeader($"{LanguageHelper.GetTextByPointShop("DisplayName")}").Join(MainPanel);
        SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(MainPanel);

        // 菜单列表 and 商品列表

        ContentContainer = new View
        {
            Display = Display.Flexbox,
            LayoutDirection = LayoutDirection.Row,
            FlexWrap = false,
            Gap = new Vector2(0f),
        }.Join(MainPanel);
        ContentContainer.SetSize(0f, 450f, 1f);

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

        var searchBarContainer = new View
        {
            Display = Display.Flexbox,
            //SpecifyWidth = true,
            //FlexWeight = { Enable = true, Value = 1f },
            MainAlignment = MainAlignment.Start,
            CrossAlignment = CrossAlignment.Center,
            Gap = new Vector2(4),
        }.Join(rightContainer);
        searchBarContainer.SetWidth(0f, 1f);
        searchBarContainer.PaddingTop = 4f;
        searchBarContainer.PaddingLeft = 4f;
        searchBarContainer.PaddingRight = 4f;

        var searchBar = new View()
        {
            CornerRadius = new Vector4(4f),
            Border = 2,
            BorderColor = SUIColor.Border * 0.75f,
            BgColor = SUIColor.Background * 0.25f,
            Display = Display.Flexbox,
            //SpecifyWidth = true,
            //FlexWeight = { Enable = true, Value = 1f },
        }.Join(searchBarContainer);
        searchBar.SetWidth(0, 1f);
        searchBar.SetHeight(32f, 0f);

        // 搜索
        var searchText = new SUIText
        {
            Text = $"{LanguageHelper.GetTextByPointShop("NameFilter")}",
            TextScale = 0.8f,
            TextAlign = new Vector2(0.5f),
            CornerRadius = new Vector4(2f, 0f, 2f, 0f),
            BgColor = SUIColor.Background * 0.5f,
        }.Join(searchBar);
        searchText.SetPadding(12f);
        searchText.SetHeight(0f, 1f);

        SUIDividingLine.Vertical(Color.Black * 0.5f).Join(searchBar);

        SearchBox = new SUIEditText
        {
            BgColor = SUIColor.Border * 0.25f,
            TextAlign = new Vector2(0f, 0.5f),
            TextScale = 0.8f,
            CursorFlashColor = Color.White,
            OverflowHidden = true,
            SpecifyWidth = true,
            FlexWeight = { Enable = true, Value = 1 },
        }.Join(searchBar);
        SearchBox.OnTextChanged += () =>
        {
            _keywords = SearchBox.Text;
            UpdateShopItemTable();
        };
        SearchBox.SetPadding(8f);
        SearchBox.SetHeight(0f, 1f);

        SUIDividingLine.Vertical(Color.Black * 0.5f).Join(searchBar);

        // 清空
        var clearText = new SUIText
        {
            Text = $"{LanguageHelper.GetTextByPointShop("Clear")}",
            TextScale = 0.8f,
            TextAlign = new Vector2(0.5f),
            CornerRadius = new Vector4(0f, 2f, 0f, 2f),
            BgColor = SUIColor.Background * 0.5f,
            DragIgnore = false,
        }.Join(searchBar);
        clearText.OnLeftMouseDown += (_, _) => SearchBox.Text = string.Empty;
        clearText.SetPadding(12f);
        clearText.SetHeight(0f, 1f);

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

        UpdateShopItemTable();

        SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(MainPanel);
        ShopFooter = new SUIShopFooter().Join(MainPanel);
    }

    public SUIShopFooter ShopFooter { get; private set; }

    //public View BalanceContainer { get; private set; }
    //public SUIText EnvironmentName { get; private set; }
    //public SUIText Balance { get; private set; }

    public readonly AnimationTimer SwitchTimer = new(3);

    protected override void UpdateAnimationTimer(GameTime gameTime)
    {
        base.UpdateAnimationTimer(gameTime);
        StartByStatus(SwitchTimer, SwitchStatus);
        SwitchTimer.Update(gameTime);
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
            Matrix.CreateScale(SwitchTimer.Lerp(0.95f, 1f), SwitchTimer.Lerp(0.95f, 1f), 1) *
            Matrix.CreateTranslation(center.X, center.Y, 0);

        base.Draw(spriteBatch);
    }
}

public class SUICommonHeader : View
{
    public SUICross SUICross { get; }
    public SUICommonHeader(string name)
    {
        Display = Display.Flexbox;
        LayoutDirection = LayoutDirection.Row;
        MainAlignment = MainAlignment.SpaceBetween;
        CrossAlignment = CrossAlignment.Center;
        FlexWrap = false;
        BgColor = Color.Black * 0.25f;
        CornerRadius = new Vector4(6f, 6f, 0f, 0f);
        SetSize(0f, 40f, 1f);

        var titleText = new SUIText
        {
            Text = name,
            TextScale = 0.45f,
            TextAlign = new Vector2(0f, 0.5f),
        }.Join(this);
        titleText.SetSize(0f, 0f, 0.25f, 1f);
        titleText.UseDeathText();
        titleText.PaddingLeft = 12f;
        titleText.PaddingRight = 12f;

        SUICross = new SUICross(SUIColor.Warn * 0.75f, SUIColor.Border * 0.75f)
        {
            CrossSize = 22f,
            CrossRounded = 3.5f,
            CrossBorderHoverColor = SUIColor.Highlight,
            CrossBackgroundHoverColor = SUIColor.Warn,
            BoxSizing = SilkyUIFramework.Core.BoxSizing.ContentBox,
        }.Join(this);
        SUICross.SetSize(24, 0, 0f, 1f);
        SUICross.PaddingLeft = 12f;
        SUICross.PaddingRight = 12f;
        SUICross.OnLeftMouseDown += delegate { PointShopUI.SwitchStatus = false; };
    }
}

public class SUIShopFooter : View
{
    public View BalanceContainer { get; }
    public SUIText EnvironmentName { get; }
    public SUIText Balance { get; }

    public SUIShopFooter()
    {
        Display = Display.Flexbox;
        LayoutDirection = LayoutDirection.Row;
        MainAlignment = MainAlignment.SpaceBetween;
        Gap = new Vector2(4f);
        BgColor = Color.Black * 0.25f;
        CornerRadius = new Vector4(0f, 0f, 6f, 6f);
        SetSize(0f, 30f, 1f);
        PaddingLeft = 12f;
        PaddingRight = 12f;

        BalanceContainer = new View
        {
            Display = Display.Flexbox,
            LayoutDirection = LayoutDirection.Row,
        }.Join(this);
        BalanceContainer.SetHeight(0, 1f);

        EnvironmentName = new SUIText()
        {
            TextAlign = new Vector2(0.5f, 0.5f),
            TextScale = 0.8f,
            TextColor = Color.White,
        }.Join(BalanceContainer);
        EnvironmentName.SetHeight(0f, 1f);

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
        }.Join(this);
        titleText.SetHeight(0, 1f);
    }
}