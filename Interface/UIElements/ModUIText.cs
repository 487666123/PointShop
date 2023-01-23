using PointShop.Common.Configs;

namespace PointShop.Interface.UIElements
{
    public class ModUIText : UIElement
    {
        public string text;
        public Color textColor;
        public float textScale;
        public Func<Color> GetColor;

        public ModUIText(string text, Color textColor, float textScale, Func<Color> GetColor = null)
        {
            SetPadding(0);
            this.text = text;
            this.textColor = textColor;
            this.textScale = textScale;
            this.GetColor = GetColor;
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            Vector2 pos = GetInnerDimensions().Position();
            Vector2 size = GetInnerDimensions().Size();
            Vector2 textSize = MyUtils.TextSize(text) * textScale;

            if (GetColor != null)
                textColor = GetColor();

            pos.X++;
            MyUtils.DrawText(pos + new Vector2(0, PointConfig.Instance.UIYAxisOffset * textScale + size.Y / 2 - textSize.Y / 2), text, textColor, Color.Black, Vector2.Zero, textScale);
            base.DrawChildren(spriteBatch);
        }

        public void RefreshSize()
        {
            this.SetSize(MyUtils.TextSize(text) * textScale);
        }
    }
}
