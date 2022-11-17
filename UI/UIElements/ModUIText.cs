using PointShop.Common.Configs;

namespace PointShop.Interface.UIElements
{
    public class ModUIText : UIElement
    {
        public string text;
        public Color textColor;
        public float textScale;
        private float timer1;
        private int textScrollingMode;
        public Func<Color> GetColor;

        public ModUIText(string text, Color textColor, float textScale, Func<Color> GetColor = null)
        {
            SetPadding(0);
            this.OverflowHidden = true;
            this.text = text;
            this.textColor = textColor;
            this.textScale = textScale;
            this.GetColor = GetColor;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer1++;
            if (timer1 % 120 == 0)
            {
                textScrollingMode++;
                textScrollingMode %= 4;
            }
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            Vector2 pos = GetInnerDimensions().Position();
            Vector2 size = GetInnerDimensions().Size();
            Vector2 textSize = ModHelper.GetTextSize(text) * textScale;

            if (textSize.X > size.X)
            {
                switch (textScrollingMode)
                {
                    case 0:
                        pos.X -= (timer1 % 120 / 119) * (textSize.X - size.X);
                        break;
                    case 1:
                        pos.X -= (textSize.X - size.X);
                        break;
                    case 2:
                        pos.X -= (1 - timer1 % 120 / 119) * (textSize.X - size.X);
                        break;
                }
            }
            if (GetColor != null) textColor = GetColor();
            ModHelper.DrawString(pos + new Vector2(0, PointConfig.Instance.UIYAxisOffset * textScale + size.Y / 2 - textSize.Y / 2), text, textColor, Color.Black, Vector2.Zero, textScale);
            base.DrawChildren(spriteBatch);
        }

        public void RefreshSize()
        {
            this.SetSize(ModHelper.GetTextSize(text) * textScale);
        }
    }
}
