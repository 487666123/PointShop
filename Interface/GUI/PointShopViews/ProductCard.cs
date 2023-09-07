using PointShop.Interface.SUIElements;
using Terraria.GameContent.UI;

namespace PointShop.Interface.GUI.PointShopViews
{
    public class ProductCard : View
    {
        public ProductCard(Item item, int value, int timing, Terrain terrain)
        {
            value = (int)Math.Round(value * (ShopGUI.PointMultiplier / 100f));

            Rounded = new Vector4(14f);
            BgColor = UIColor.PanelBg;
            Border = 2;
            BorderColor = UIColor.PanelBorder;

            var itemSlotSingle = new SUIItemSlot(item, value, timing, terrain)
            {
                VAlign = 0.5f
            };
            itemSlotSingle.Join(this);

            var itemName = new SUIText(MyUtils.CutText(item.Name, 0.8f, 80f), 0.8f)
            {
                TextColor = () => ItemRarity.GetColor(item.rare)
            };
            itemName.Left.Pixels = itemSlotSingle.RightPixels() + 8f;
            itemName.Join(this);

            var itemPrice = new SUIText($"{MyUtils.GetText("ShopUI.Price")}: {value}", 0.7f)
            {
                VAlign = 1f
            };
            itemPrice.Left.Pixels = itemSlotSingle.RightPixels() + 8f;
            itemPrice.Join(this);

            var button = new MiniButton(MyUtils.GetText("ShopUI.Buy")) { HAlign = 1f, VAlign = 1f };
            button.OnLeftClick += (_, _) => itemSlotSingle.BuyItem();
            button.Join(this);

            SetPadding(10f);
            SetInnerPixels(180f, 52f);

            LayoutMode = LayoutMode.Horizontal;
            Spacing = new Vector2(8f);
            Wrap = true;
        }
    }
}