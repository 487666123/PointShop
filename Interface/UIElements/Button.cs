using PointShop.Common.Animations;
using PointShop.Common.Configs;
using PointShop.Interface;

namespace PointShop.ModUI.UIElements
{
    public class Button : UIElement
    {
        public int[] data = new int[5]; // 专门为此按钮储存的数据
        public AnimationTimer HoverTimer = new(3);

        public Texture2D texture;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                textSize = MyUtils.TextSize(value);
            }
        }

        public Vector2 TextSize => textSize;
        private string text;
        private Vector2 textSize;

        public Button(Texture2D texture, string text)
        {
            this.SetSize(100f, 40f);
            this.texture = texture;
            Text = text;
            PaddingLeft = 15f;
        }

        public override void Update(GameTime gameTime)
        {
            HoverTimer.Update();
            base.Update(gameTime);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            HoverTimer.Open();
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            HoverTimer.Close();
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            Vector2 position = GetDimensions().Position();
            Vector2 size = GetDimensions().Size();
            Color borderColor = Color.Lerp(UIColor.ButtonBorder, UIColor.ButtonBorderHover, HoverTimer.Schedule);
            PixelShader.RoundedRectangle(position, size, new Vector4(10), UIColor.ButtonBackground, 2, borderColor);

            position = GetInnerDimensions().Position();
            size = GetInnerDimensions().Size();
            sb.Draw(texture, position + new Vector2(15, size.Y / 2f) - texture.Size() / 2f, Color.White);
            MyUtils.DrawText(
                position + new Vector2(40, size.Y / 2 - textSize.Y / 2f + PointConfig.Instance.UIYAxisOffset), text,
                Color.White, Color.Black);
        }
    }
}