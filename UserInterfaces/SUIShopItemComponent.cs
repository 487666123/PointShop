using System.Security.Cryptography.X509Certificates;
using PointShop.Items;
using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.BasicElements;
using SilkyUIFramework.Extensions;
using Terraria.GameContent.UI;

namespace PointShop.UserInterfaces;

public partial class SUIShopItemComponent : View, IEventHandlerHolder
{
    List<object> IEventHandlerHolder.ActiveHandlers { get; } = [];

    protected bool IsDirty;
    public void MakeDirty() => IsDirty = true;

    public ShopItem ShopItem { get; }

    public SUIImage Image { get; protected set; }
    public View BuyButton { get; protected set; }
    public SUIDividingLine SUIDividingLine { get; protected set; }
    public SUICoverView CoverView { get; protected set; }

    public SUIShopItemComponent(ShopItem shopItem)
    {
        ShopItem = shopItem;

        Display = Display.Flexbox;
        LayoutDirection = LayoutDirection.Column;
        FlexWrap = false;

        CornerRadius = new Vector4(4f);
        Border = 2;
        BorderColor = SUIColor.Border * 0.75f;
        BgColor = Color.Black * 0.25f;

        SetSize(0, 0f, 1f, 1f);

        Create(shopItem);

        // 购买按钮
        BuyButton = new View()
        {
            CornerRadius = new Vector4(0f, 0f, 2f, 2f),
            Display = Display.Flexbox,
            LayoutDirection = LayoutDirection.Row,
            MainAlignment = MainAlignment.Center,
            CrossAlignment = CrossAlignment.Center,
        }.Join(this);
        BuyButton.SetSize(0f, 28f, 1f);
        BuyButton.OnUpdateAnimationTimer += (_) =>
        {
            BuyButton.BgColor = BuyButton.HoverTimer.Lerp(SUIColor.Highlight * 0.05f, Color.White * 0.1f);
        };
        BuyButton.OnLeftMouseDown += (_, _) => ShopItem.Buy();

        // 价格左边的积分币 ItemSlot
        var coinSlot = new SUIItemSlot
        {
            Item = new Item(ModContent.ItemType<PointCoin>()),
            ItemAlign = new Vector2(0f, 0.5f),
            ItemScale = 0.8f,
            BgColor = Color.Transparent,
            BorderColor = Color.Transparent,
            ItemInteractive = false,
        }.Join(BuyButton);
        coinSlot.SetSize(20, 0f, 0f, 1f);

        // 价格
        var prices = new SUIText()
        {
            Text = $"{ShopItem.Prices:#,##0}",
            TextAlign = new Vector2(0.5f, 0.5f),
            TextScale = 0.8f,
            TextColor = Color.White,
        }.Join(BuyButton);
        prices.SetHeight(0f, 1f);
        prices.UseMenuTickSoundForMouseOver();

        shopItem.PricesChanged.AddHandler(this, (_, args) =>
        {
            prices.Text = $"{args.Value:#,##0}";
            MakeDirty();
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
                    if (HasChild(CoverView))
                    {
                        RemoveChild(CoverView);
                        MakeDirty();
                    }
                }
                else
                {
                    if (!HasChild(CoverView))
                    {
                        AppendFromView(CoverView);
                        MakeDirty();
                    }
                }
            });
        }
    }

    protected virtual void Create(ShopItem shopItem)
    {
        var ShopItemName = new SUIText
        {
            CornerRadius = new Vector4(2f, 2f, 0f, 0f),
            Text = $"{shopItem.DisplayName}",
            BgColor = SUIColor.Background * 0.25f,
            TextScale = 0.65f,
            TextAlign = new Vector2(0.5f),
        }.Join(this);
        ShopItemName.SetSize(0f, 28f, 1f);

        SUIDividingLine = SUIDividingLine.Horizontal(SUIColor.Border * 0.25f).Join(this);

        Image = new SUIImage(ShopItem.Icon)
        { }.Join(this);
        Image.SetSize(0f, -2f, 1f, 0.8f);

        SUIDividingLine = SUIDividingLine.Horizontal(Color.Black * 0.4f).Join(this);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (IsDirty)
        {
            Recalculate();
            IsDirty = false;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (BuyButton.IsMouseHovering)
            Main.hoverItemName = LanguageHelper.GetTextByPointShop("Buy").Value;
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

        var ShopItemName = new SUIText
        {
            CornerRadius = new Vector4(2f, 2f, 0f, 0f),
            Text = $"{shopItem.DisplayName}",
            BgColor = rarityColor * 0.05f,
            TextColor = rarityColor,
            TextScale = 0.65f,
            TextAlign = new Vector2(0.5f),
        }.Join(this);
        ShopItemName.SetSize(0f, 28f, 1f);

        SUIDividingLine = SUIDividingLine.Horizontal(SUIColor.Border * 0.25f).Join(this);

        SUIItemSlot = new SUIItemSlot
        {
            Item = new Item(simpleShopItem.Item.type, simpleShopItem.Item.stack),
            BgColor = Color.Transparent,
            BorderColor = Color.Transparent,
            Border = 0,
            ItemInteractive = false,
            FlexWeight = { Enable = true, Value = 1f },
            SpecifyHeight = true,
            ItemIconSizeLimit = 50,
            StackAlign = new Vector2(0.7f, 0.65f),
            StackFormat = "x{0}",
        }.Join(this);
        SUIItemSlot.SetWidth(0f, 1f);

        SUIDividingLine = SUIDividingLine.Horizontal(SUIColor.Border * 0.25f).Join(this);
    }
}

/// <summary>
/// Cover 覆盖
/// </summary>
public class SUICoverView : View
{
    public readonly SUIImage Icon;
    public readonly SUIText DisplayName;
    public string Description = "解锁条件详情";

    public SUICoverView(Asset<Texture2D> icon, string displayName, string description)
    {
        Description = description;
        UseImmediateMode = true;
        ZIndex = 9999;

        SetSize(0f, 0f, 1f, 1f);
        CornerRadius = new Vector4(2);
        BgColor = new Color(0.5f, 0.5f, 0.5f) * 0.25f;
        Positioning = SilkyUIFramework.Core.Positioning.Absolute;

        Display = Display.Flexbox;
        Gap = new Vector2(8f);
        LayoutDirection = LayoutDirection.Column;
        MainAlignment = MainAlignment.Center;
        CrossAlignment = CrossAlignment.Center;

        Icon = new SUIImage(icon)
        {
            ImageAlign = new Vector2(0.5f),
            ImageScale = new Vector2(0.85f),
        }.Join(this);
        Icon.SetSize(32f, 32f);

        DisplayName = new SUIText()
        {
            Text = displayName,
            TextAlign = new Vector2(0.5f),
            TextScale = 0.8f,
        }.Join(this);
        DisplayName.SetWidth(0f, 1f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (IsMouseHovering && !string.IsNullOrWhiteSpace(Description))
        {
            Main.hoverItemName = Description;
        }
    }
}