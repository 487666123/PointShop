using PointShop.Common.Animations;
using PointShop.Interface;

namespace PointShop.ModUI.UIElements
{
    public class Button : UIElement
    {
        public int[] data = new int[5]; // 专门为此按钮储存的数据
        public AnimationTimer HoverTimer = new(3);

        public string text;
        public Vector2 textSize;
        public Vector2 TextPosition => new(ImagePosition.X + imageSize.X + 10, 6 + this.Height() / 2 - textSize.Y / 2);

        public Texture2D image;
        public Vector2 imageSize;
        public Vector2 ImagePosition => new Vector2(30, this.Height() / 2) - imageSize / 2;

        public Button(Texture2D texture, string text)
        {
            Width.Pixels = ModHelper.GetTextSize(text).X + this.HPadding() + 75;
            Height.Pixels = 40f;

            image = texture;
            imageSize = texture.Size();

            this.text = text;
            textSize = ModHelper.GetTextSize(text);
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
            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = dimensions.Position();
            Vector2 size = dimensions.Size();

            Color border = Color.Lerp(ModColor.ButtonBorder, ModColor.ButtonBorderHover, HoverTimer.Schedule);

            PixelShader.DrawBox(Main.UIScaleMatrix, position, size, 10, 3, border, ModColor.ButtonBackground);

            sb.Draw(image, position + ImagePosition, Color.White);
            ModHelper.DrawString(position + TextPosition, text, Color.White, Color.Black);
        }

        public void SetText(string text)
        {
            this.text = text;
            textSize = ModHelper.GetTextSize(text);
        }

        public void SetImage(Texture2D texture)
        {
            image = texture;
            imageSize = texture.Size();
        }
    }
}
