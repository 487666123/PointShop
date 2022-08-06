using PointShop.Common.Animations;
using PointShop.Interface;

namespace PointShop.ModUI.UIElements
{
    public class MiniButton : UIElement
    {
        private readonly Asset<Texture2D> Background;
        private readonly Asset<Texture2D> BackgroundBorder;
        private readonly Asset<Texture2D> Point;

        public int[] data;
        public AnimationTimer HoverTimer = new(3);

        private string text;
        private Vector2 textOffset;
        public string Text { get => text; set => text = value; }
        public float Scale { get; set; }

        public MiniButton(string text, float scale = 0.8f)
        {
            Background = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale");
            BackgroundBorder = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder");
            Point = ModHelper.GetTexture("Point");
            textOffset.X = Point.Value.Width;

            data = new int[5];
            Scale = scale;
            Text = text;

            Height.Pixels = 36f * scale;
            Width.Pixels = (ModHelper.GetTextSize(text).X + 30f) * scale;
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

            PixelShader.DrawBox(Main.UIScaleMatrix, position, size, 8, 3, border, ModColor.ButtonBackground);

            Vector2 textSize = ModHelper.GetTextSize(Text) * Scale;
            Vector2 textPosition = position + size / 2f - textSize / 2f;
            textPosition.Y += 4 * Scale;
            ModHelper.DrawString(textPosition, Text, Color.White, Color.Black, Vector2.Zero, Scale);
        }
    }
}
