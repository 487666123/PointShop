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
                this.SetSize(ModHelper.GetTextSize(value) * Scale);
            }
        }
        public float Scale { get; set; }

        public MiniButton(string text, float scale = 0.8f)
        {
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
            Vector2 position = GetDimensions().Position();
            Vector2 size = GetDimensions().Size();
            Color border = Color.Lerp(UIColor.ButtonBorder, UIColor.ButtonBorderHover, HoverTimer.Schedule);
            PixelShader.DrawBox(position, size, 8, 3, border, UIColor.ButtonBackground);

            position = GetInnerDimensions().Position();
            size = GetInnerDimensions().Size();
            Vector2 textSize = ModHelper.GetTextSize(Text) * Scale;
            Vector2 textPosition = position + size / 2f - textSize / 2f;
            textPosition.Y += PointConfig.Instance.UIYAxisOffset * Scale;
            ModHelper.DrawString(textPosition, Text, Color.White, Color.Black, Vector2.Zero, Scale);
        }
    }
}
