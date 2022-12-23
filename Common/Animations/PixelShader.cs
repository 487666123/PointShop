namespace PointShop.Common.Animations
{
    public class PixelShader
    {
        internal static Effect Fork;
        internal static Effect Box;
        internal static Effect RoundRect;
        internal static Effect RoundRectNoBorder;
        internal static Texture2D Transparent;

        public static void DrawFork(Vector2 position, float size, float radius, Color backgroundColor, float border, Color borderColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            Effect effect = ModAssets.Fork;
            effect.Parameters[nameof(size)].SetValue(size);
            effect.Parameters[nameof(border)].SetValue(border);
            effect.Parameters[nameof(radius)].SetValue(radius);
            effect.Parameters[nameof(borderColor)].SetValue(borderColor.ToVector4());
            effect.Parameters[nameof(backgroundColor)].SetValue(backgroundColor.ToVector4());
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, effect, Main.UIScaleMatrix);
            sb.Draw(texture, position, null, Color.White, 0, new(0), size, 0, 1f);
            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
        }

        // DrawRoundRect 现在有两个 .fx 文件，一个带边框的，一个不带的，也许能节省性能？
        public static void DrawRoundRect(Vector2 position, Vector2 size, float round, Color backgroundColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            Effect effect = ModAssets.RoundRectNoBorder;
            effect.Parameters[nameof(size)].SetValue(size);
            effect.Parameters[nameof(round)].SetValue(round);
            effect.Parameters[nameof(backgroundColor)].SetValue(backgroundColor.ToVector4());
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, effect, Main.UIScaleMatrix);
            sb.Draw(Transparent, position, null, Color.White, 0, new(0), size, 0, 1f);
            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
        }

        public static void DrawRoundRect(Vector2 position, Vector2 size, float round, Color backgroundColor, float border, Color borderColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            Effect effect = ModAssets.RoundRect;
            effect.Parameters[nameof(size)].SetValue(size);
            effect.Parameters[nameof(round)].SetValue(round);
            effect.Parameters[nameof(border)].SetValue(border);
            effect.Parameters[nameof(backgroundColor)].SetValue(backgroundColor.ToVector4());
            effect.Parameters[nameof(borderColor)].SetValue(borderColor.ToVector4());
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, effect, Main.UIScaleMatrix);
            sb.Draw(texture, position, null, Color.White, 0, new(0), size, 0, 1f);
            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
        }

        public static void DrawBox(Vector2 position, Vector2 size, float radius, float border, Color borderColor, Color background)
        {
            SpriteBatch sb = Main.spriteBatch;
            Effect effect = ModAssets.Box;
            effect.Parameters["size"].SetValue(size);
            effect.Parameters["radius"].SetValue(radius);
            effect.Parameters["border"].SetValue(border);
            effect.Parameters["borderColor1"].SetValue(borderColor.ToVector4());
            effect.Parameters["borderColor2"].SetValue(borderColor.ToVector4());
            effect.Parameters["background1"].SetValue(background.ToVector4());
            effect.Parameters["background2"].SetValue(background.ToVector4());
            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, effect, Main.UIScaleMatrix);
            sb.Draw(texture, position, null, Color.White, 0, new(0), size, 0, 1f);
            sb.End();
            sb.Begin(0, sb.GraphicsDevice.BlendState, sb.GraphicsDevice.SamplerStates[0],
                sb.GraphicsDevice.DepthStencilState, sb.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
        }

        public override void Load()
        {
            Fork = ModHelper.GetEffect("Fork").Value;
            Box = ModHelper.GetEffect("Box").Value;
            RoundRect = ModHelper.GetEffect(nameof(RoundRect)).Value;
            RoundRectNoBorder = ModHelper.GetEffect(nameof(RoundRectNoBorder)).Value;
            Main.QueueMainThreadAction(() => Transparent = new Texture2D(Main.graphics.GraphicsDevice, 1, 1));
        }

        public override void Unload()
        {
            Fork = null;
            Box = null;
            RoundRect = null;
            RoundRectNoBorder = null;
            Transparent = null;
        }
    }
}
