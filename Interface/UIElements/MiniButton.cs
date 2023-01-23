using PointShop.Common.Animations;
using PointShop.Common.Configs;
using PointShop.Interface;

namespace PointShop.ModUI.UIElements
{
    public class MiniButton : UIElement
    {
        public AnimationTimer HoverTimer = new(3);

        private string text;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                this.SetSize(MyUtils.TextSize(value) * Scale);
            }
        }

        public float Scale { get; set; }

        public MiniButton(string text, float scale = 0.8f)
        {
            Scale = scale;
            Text = text;

            Height.Pixels = 36f * scale;
            Width.Pixels = (MyUtils.TextSize(text).X + 30f) * scale;
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

            Color border = Color.Lerp(UIColor.ButtonBorder, UIColor.ButtonBorderHover, HoverTimer.Schedule);
            PixelShader.RoundedRectangle(position, size, new Vector4(10f), UIColor.ButtonBackground, 2, border);

            position = GetInnerDimensions().Position();
            size = GetInnerDimensions().Size();
            Vector2 textSize = MyUtils.TextSize(Text) * Scale;
            Vector2 textPosition = position + size / 2f - textSize / 2f;
            textPosition.Y += PointConfig.Instance.UIYAxisOffset * Scale;
            MyUtils.DrawText(textPosition, Text, Color.White, Color.Black, Vector2.Zero, Scale);
        }
    }
}