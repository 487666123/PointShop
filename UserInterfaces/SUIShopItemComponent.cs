using Microsoft.Extensions.DependencyInjection;
using PointShop.Items;
using SilkyUIFramework.Extensions;
using SilkyUIFramework.UserInterfaces;
using Terraria.GameContent.UI;
using Terraria.ModLoader.UI;

namespace PointShop.UserInterfaces;

/// <summary>
/// 商品卡片组件，包括商品名称、商品图标、购买按钮、价格等
/// </summary>
public partial class SUIShopItemComponent : UIElementGroup, IEventHandlerHolder
{
    List<object> IEventHandlerHolder.ActiveHandlers { get; } = [];

    public ShopItem ShopItem { get; }

    public SUIImage Image { get; protected set; }
    public UIElementGroup BuyButton { get; protected set; }
    public SUIDividingLine SUIDividingLine { get; protected set; }
    public SUICoverView CoverView { get; protected set; }

    public SUIShopItemComponent(ShopItem shopItem)
    {
        ShopItem = shopItem;

        LayoutType = LayoutType.Flexbox;
        FlexDirection = FlexDirection.Column;

        BorderRadius = new Vector4(4f);
        Border = 2;
        BorderColor = SUIColor.Border * 0.75f;
        BackgroundColor = Color.Black * 0.25f;

        FlexGrow = 1f;
        SetSize(100f, 160f);

        Create(shopItem);

        // 购买按钮
        BuyButton = new UIElementGroup()
        {
            BorderRadius = new Vector4(0f, 0f, 2f, 2f),
            LayoutType = LayoutType.Flexbox,
            FlexDirection = FlexDirection.Row,
            MainAlignment = MainAlignment.Center,
            CrossAlignment = CrossAlignment.Center,
        }.Join(this);
        BuyButton.SetSize(0f, 28f, 1f);
        BuyButton.OnUpdateStatus += (gameTime) =>
        {
            BuyButton.BackgroundColor = BuyButton.HoverTimer.Lerp(SUIColor.Highlight * 0.05f, Color.White * 0.1f);
        };
        BuyButton.LeftMouseDown += delegate
        {
            ShopItem.Buy();
        };
        BuyButton.RightMouseDown += delegate
        {
            IMouseMenu mouseMenu = SilkyUISystem.ServiceProvider.GetRequiredService<IMouseMenu>();
            mouseMenu.OpenMenu(MouseAnchor.TopLeft, BuyButton.Bounds.Center,
                ["购买 3 份", "购买 5 份", "购买 10 份", "购买 100 份"], (content, index) =>
                {
                    return index switch
                    {
                        0 => ShopItem.Buy(3),
                        1 => ShopItem.Buy(5),
                        2 => ShopItem.Buy(10),
                        3 => ShopItem.Buy(100),
                        _ => ShopItem.Buy(),
                    };
                });
        };

        // 价格左边的积分币 ItemSlot
        var coinSlot = new SUIItemSlot
        {
            Item = new Item(ModContent.ItemType<PointCoin>()),
            ItemAlign = new Vector2(0f, 0.5f),
            ItemScale = 0.8f,
            BackgroundColor = Color.Transparent,
            BorderColor = Color.Transparent,
            ItemInteractive = false,
        }.Join(BuyButton);
        coinSlot.SetSize(20, 0f, 0f, 1f);

        // 价格
        var prices = new UITextView()
        {
            Text = $"{ShopItem.Prices:#,##0}",
            TextAlign = new Vector2(0.5f, 0.5f),
            TextScale = 0.8f,
            TextColor = Color.White,
        }.Join(BuyButton);
        prices.SetHeight(0f, 1f);
        //prices.UseMenuTickSoundForMouseOver();

        shopItem.PricesChanged.AddHandler(this, (_, args) =>
        {
            prices.Text = $"{args.Value:#,##0}";
        });

        if (shopItem.TryGetUnlockCondition(out var unlockCondition))
        {

            CoverView = new SUICoverView(unlockCondition.Icon, unlockCondition.DisplayName, unlockCondition.Description);
            if (!unlockCondition.IsUnlock)
            {
                CoverView.Join(this);
            }

            ShopItem.UnlockStateChanged.AddHandler(this, (_, args) =>
            {
                if (args.Value)
                {
                    if (HasChild(CoverView)) RemoveChild(CoverView);
                }
                else
                {
                    if (!HasChild(CoverView)) AddChild(CoverView);
                }
            });
        }
    }

    protected virtual void Create(ShopItem shopItem)
    {
        var ShopItemName = new UITextView
        {
            BorderRadius = new Vector4(2f, 2f, 0f, 0f),
            Text = $"{shopItem.DisplayName}",
            BackgroundColor = SUIColor.Background * 0.25f,
            TextScale = 0.65f,
            TextAlign = new Vector2(0.5f),
        }.Join(this);
        ShopItemName.SetSize(0f, 28f, 1f);

        SUIDividingLine = SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(this);

        Image = new SUIImage(ShopItem.Icon).Join(this);
        Image.FlexGrow = 1f;
        Image.SetWidth(0f, 1f);

        SUIDividingLine = SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(this);
    }

    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

        if (BuyButton.IsMouseHovering)
            UICommon.TooltipMouseText(LanguageHelper.GetTextByPointShop("Buy").Value);
    }
}

public class SUISimpleShopItem(SimpleShopItem shopItem) : SUIShopItemComponent(shopItem)
{
    public SUIItemSlot SUIItemSlot;

    protected override void Create(ShopItem shopItem)
    {
        if (ShopItem is not SimpleShopItem simpleShopItem) throw new ArgumentException("ShopItem is not SimpleShopItem");

        Color rarityColor = Color.White;
        if (ItemRarity._rarities.TryGetValue(simpleShopItem.Item.rare, out var color))
        {
            rarityColor = color;
        }

        var ShopItemName = new UITextView
        {
            BorderRadius = new Vector4(2f, 2f, 0f, 0f),
            Text = $"{shopItem.DisplayName}",
            BackgroundColor = rarityColor * 0.05f,
            TextColor = rarityColor,
            TextScale = 0.65f,
            TextAlign = new Vector2(0.5f),
            FitWidth = false,
            FitHeight = false,
        }.Join(this);
        ShopItemName.SetSize(0f, 28f, 1f);

        SUIDividingLine = SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(this);

        SUIItemSlot = new SUIItemSlot
        {
            Item = new Item(simpleShopItem.Item.type, simpleShopItem.Item.stack),
            Border = 0,
            BorderColor = Color.Transparent,
            BackgroundColor = Color.Transparent,
            ItemInteractive = false,
            ItemIconSizeLimit = 50,
            StackAlign = new Vector2(0.7f, 0.7f),
            StackFormat = "{0}",
            FlexGrow = 1f,
            FlexShrink = 1f,
        }.Join(this);
        SUIItemSlot.SetHeight(0f, 0.5f);
        SUIItemSlot.SetWidth(0f, 1f);

        SUIDividingLine = SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(this);
    }
}

/// <summary>
/// Cover 覆盖
/// </summary>
public class SUICoverView : UIElementGroup
{
    public readonly SUIImage Icon;
    public readonly UITextView DisplayName;
    public string Description = string.Empty;

    public SUICoverView(Asset<Texture2D> icon, string displayName, string description)
    {
        Positioning = Positioning.Absolute;

        ZIndex = 114514;
        Description = description;

        SetSize(0f, 0f, 1f, 1f);

        LayoutType = LayoutType.Flexbox;
        FlexDirection = FlexDirection.Column;
        MainAlignment = MainAlignment.Center;
        CrossAlignment = CrossAlignment.Center;
        CrossContentAlignment = CrossContentAlignment.Center;

        BorderRadius = new Vector4(2);
        BackgroundColor = new Color(0.5f, 0.5f, 0.5f) * 0.25f;

        Icon = new SUIImage(icon)
        {
            ImageAlign = new Vector2(0.5f),
            ImageScale = new Vector2(0.85f),
            FitWidth = false,
            FitHeight = false,
        }.Join(this);
        Icon.SetSize(32f, 32f);

        DisplayName = new UITextView
        {
            Text = displayName,
            TextAlign = new Vector2(0.5f),
            TextScale = 0.8f,
        }.Join(this);
    }

    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null,
            SilkyUI.RasterizerStateForOverflowHidden, null, SilkyUI.TransformMatrix);

        base.Draw(gameTime, spriteBatch);

        if (IsMouseHovering && !string.IsNullOrWhiteSpace(Description))
        {
            UICommon.TooltipMouseText(Description);
        }
    }
}