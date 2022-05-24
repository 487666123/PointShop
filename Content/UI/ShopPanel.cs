using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace PointShop.Content.UI
{
    public class PointPanel : UIElement
    {
        public Texture2D borderImage;
        public Texture2D backgroundImage;
        public Color borderColor;
        public Color backgroundColor;

        public PointPanel()
        {
            borderImage = MyUtils.GetTexture("PanelBorder2").Value;
            borderColor = Color.White;
            backgroundImage = MyUtils.GetTexture("PanelBackground2").Value;
            backgroundColor = Color.White * 0.7f;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // 背景 Background
            MyUtils.DrawPanel(spriteBatch, GetDimensions(), backgroundImage,
                backgroundColor);

            // 边框 border
            MyUtils.DrawPanel(spriteBatch, GetDimensions(), borderImage,
                borderColor);
        }
    }
}
