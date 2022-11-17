using PointShop.Common.Animations;
using PointShop.Common.Configs;
using PointShop.Interface.Common;

namespace ImproveGame.Interface.UIElements_Shader
{
    public class UITitle : UIElement
    {
        private string text;
        private Vector2 textSize;
        public float Scale { get; set; }
        public Color textColor;
        public Color textBorderColor;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                textSize = ModHelper.GetTextSize_Big(text) * Scale;
            }
        }

        public UITitle(string text, float scale)
        {
            this.Scale = scale;

            PaddingTop = 5f;
            PaddingBottom = 5f;
            PaddingLeft = 30;
            PaddingRight = 30;

            Text = text;
            Width.Pixels = textSize.X + this.HPadding();
            Height.Pixels = textSize.Y + this.VPadding();
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            CalculatedStyle rectangle = GetDimensions();
            Vector2 position = rectangle.Position();
            Vector2 size = rectangle.Size();
            PixelShader.DrawBox(position, size, 10, 0, UIColor.Default.TitleBackground, UIColor.Default.TitleBackground);

            position = GetInnerDimensions().Position();
            size = GetInnerDimensions().Size();
            Utils.DrawBorderStringBig(sb, text, position + new Vector2(0, size.Y / 2 - textSize.Y / 2 + PointConfig.Instance.UIYAxisOffset * 3 * Scale), Color.White, Scale);
        }
    }
}
