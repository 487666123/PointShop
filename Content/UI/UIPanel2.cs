using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PointShop.Content.UI
{
    public class UIPanel2 : UIElement
    {
        public Asset<Texture2D> borderT2d;
        public Asset<Texture2D> backgroundT2d;
        public Color borderColor = Color.Black;
        public Color backgroundColor = new Color(63, 82, 151) * 0.7f;

        private void LoadTextures()
        {
            if (borderT2d is null)
            {
                borderT2d = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder", AssetRequestMode.ImmediateLoad);
            }

            if (backgroundT2d is null)
            {
                backgroundT2d = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground", AssetRequestMode.ImmediateLoad);
            }
        }

        public UIPanel2()
        {
            LoadTextures();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // 背景 Background
            MyUtils.DrawPanel(spriteBatch, GetDimensions(), backgroundT2d.Value,
                backgroundColor);

            // 边框 border
            MyUtils.DrawPanel(spriteBatch, GetDimensions(), borderT2d.Value,
                borderColor);
        }
    }
}
