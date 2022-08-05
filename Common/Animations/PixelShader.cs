using Microsoft.Xna.Framework.Graphics;
using PointShop.Helpers;

namespace PointShop.Common.Animations
{
    public class PixelShader : ModSystem
    {
        public static Effect Box { get; set; }
        public static Texture2D A0 { get; set; }

        public static void DrawBox(Matrix matrix, Vector2 position, Vector2 size, float radius, float border, Color borderColor, Color background)
        {
            DrawBox(matrix, position, size, radius, border, borderColor, borderColor, background, background);
        }

        public static void DrawBox(Matrix matrix, Vector2 position, Vector2 size, float radius, float border, Color borderColor1, Color borderColor2, Color background1, Color background2)
        {
            SpriteBatch sb = Main.spriteBatch;

            Box.Parameters["size"].SetValue(size);
            Box.Parameters["radius"].SetValue(radius);
            Box.Parameters["border"].SetValue(border);
            Box.Parameters["borderColor1"].SetValue(borderColor1.ToVector4());
            Box.Parameters["borderColor2"].SetValue(borderColor2.ToVector4());
            Box.Parameters["background1"].SetValue(background1.ToVector4());
            Box.Parameters["background2"].SetValue(background2.ToVector4());

            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, Box, matrix);

            sb.Draw(A0, position, null, Color.White, 0, new(0), size, 0, 1f);

            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, matrix);
        }

        public override void Load()
        {
            Box = ModHelper.GetEffect("Box").Value;
            A0 = ModHelper.GetTexture("0").Value;
        }

        public override void Unload()
        {
            Box = null;
            A0 = null;
        }
    }
}
