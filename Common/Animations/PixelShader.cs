using Microsoft.Xna.Framework.Graphics;

namespace PointShop.Common.Animations
{
    public class PixelShader : ModSystem
    {
        internal static Effect Fork;
        internal static Effect Box;
        internal static Effect RoundRectangle;
        internal static Texture2D Transparent;

        public static void DrawFork(Vector2 position, float size, float radius, Color backgroundColor, float border, Color borderColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            Effect effect = Fork;
            effect.Parameters[nameof(size)].SetValue(size);
            effect.Parameters[nameof(border)].SetValue(border);
            effect.Parameters[nameof(radius)].SetValue(radius);
            effect.Parameters[nameof(borderColor)].SetValue(borderColor.ToVector4());
            effect.Parameters[nameof(backgroundColor)].SetValue(backgroundColor.ToVector4());
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, effect, Main.UIScaleMatrix);
            sb.Draw(Transparent, position, null, Color.White, 0, new(0), size, 0, 1f);
            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
        }

        public static void DrawRoundRectangle(Vector2 position, Vector2 size, float round, Color backgroundColor, float border = 0, Color borderColor = new())
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            Effect effect = RoundRectangle;
            effect.Parameters[nameof(size)].SetValue(size);
            effect.Parameters[nameof(round)].SetValue(round);
            effect.Parameters[nameof(border)].SetValue(border);
            effect.Parameters[nameof(backgroundColor)].SetValue(backgroundColor.ToVector4());
            effect.Parameters[nameof(borderColor)].SetValue(borderColor.ToVector4());
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, effect, Main.UIScaleMatrix);
            sb.Draw(Transparent, position, null, Color.White, 0, new(0), size, 0, 1f);
            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
        }

        public static void DrawBox(Vector2 position, Vector2 size, float radius, float border, Color borderColor, Color background)
        {
            DrawBox(position, size, radius, border, borderColor, borderColor, background, background);
        }

        public static void DrawBox(Vector2 position, Vector2 size, float radius, float border, Color borderColor1, Color borderColor2, Color background1, Color background2)
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
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, Box, Main.UIScaleMatrix);
            sb.Draw(Transparent, position, null, Color.White, 0, new(0), size, 0, 1f);
            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
        }

        public override void Load()
        {
            Fork = ModHelper.GetEffect("Fork").Value;
            Box = ModHelper.GetEffect("Box").Value;
            RoundRectangle = ModHelper.GetEffect(nameof(RoundRectangle)).Value;
            Transparent = ModHelper.GetTexture("0").Value;
        }

        public override void Unload()
        {
            Fork = null;
            Box = null;
            RoundRectangle = null;
            Transparent = null;
        }
    }
}
