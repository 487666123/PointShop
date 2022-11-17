using PointShop.Common.Animations;
using PointShop.Common.Configs;
using PointShop.Interface.Common;
using PointShop.ModUI.UIElements;
using Terraria.GameContent.UI;

namespace PointShop.Interface.UIElements
{
    public class DisplaySlot : UIElement
    {
        public Item item;
        public int value;
        public int timing;
        public Terrain terrain;

        public ItemSlotSingle itemSlotSingle;
        public ModUIText title;
        public ModUIText tips;
        public MiniButton button;

        public DisplaySlot(Item item, int value, int timing, Terrain terrain)
        {
            this.item = item;
            this.value = value;
            this.timing = timing;
            this.terrain = terrain;
            OnCreate();
        }

        public void OnCreate()
        {
            SetPadding(12);
            int value = (int)(this.value * (PointConfig.Instance.PointMultiplier / 100f));
            Append(itemSlotSingle = new(item, value, terrain, timing));
            itemSlotSingle.VAlign = 0.5f;

            Append(title = new(item.Name, Color.White, 0.8f, () => ItemRarity.GetColor(item.rare)));
            title.Left.Pixels = itemSlotSingle.Right() + 8f;
            title.RefreshSize();
            title.Width.Pixels = 140;

            Append(tips = new($"{ModHelper.GetText("ShopUI.Price")}: {value}", Color.White, 0.7f));
            tips.Left.Pixels = title.Left.Pixels;
            tips.Top.Pixels = title.Bottom() + 5f;
            tips.RefreshSize();

            Append(button = new(ModHelper.GetText("ShopUI.Pay")) { HAlign = 1f, VAlign = 1f });
            button.OnClick += Button_OnClick;

            Width.Pixels = title.Right() + this.VPadding();
            Height.Pixels = MathF.Max(tips.Bottom(), 52) + this.HPadding();
        }

        private void Button_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            itemSlotSingle.Pay();
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            // 背景和边框
            Vector2 position = GetDimensions().Position();
            Vector2 size = GetDimensions().Size();
            PixelShader.DrawRoundRectangle(position, size, 12, Common.UIColor.Default.PanelBackground, 3, Common.UIColor.Default.PanelBorder);
        }
    }
}
