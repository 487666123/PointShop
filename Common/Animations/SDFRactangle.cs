using System.Collections.Generic;

namespace PointShop.Common.Animations;

public class SDFRectangle
{
    private static readonly GraphicsDevice GraphicsDevice = Main.graphics.GraphicsDevice;

    public static Matrix GetMatrix(bool ui)
    {
        if (ui)
        {
            GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;

            float width;
            float height;

            RenderTargetBinding[] renderTargetBinding = graphicsDevice.GetRenderTargets();
            if (renderTargetBinding.Length > 0 && renderTargetBinding[0].RenderTarget is Texture2D texture2D)
            {
                width = texture2D.Width;
                height = texture2D.Height;
            }
            else
            {
                width = graphicsDevice.PresentationParameters.BackBufferWidth;
                height = graphicsDevice.PresentationParameters.BackBufferHeight;
            }

            return Matrix.CreateOrthographicOffCenter(0, width / Main.UIScale, height / Main.UIScale, 0, 0, 1);
        }
        else
        {
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Vector2 offset = screenSize * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom) / 2;
            return Matrix.CreateOrthographicOffCenter(offset.X, Main.screenWidth - offset.X,
                Main.screenHeight - offset.Y, offset.Y, 0, 1);
        }
    }

    private static void BaseDrawRectangle(Vector2 pos, Vector2 size, Vector4 rounded)
    {
        size /= 2f;

        List<VertexPositionCoordRounded> vertices = new List<VertexPositionCoordRounded>();

        Vector2 coordQ1 = new Vector2(rounded.X) - size;
        Vector2 coordQ2 = new Vector2(rounded.X);
        vertices.AddRectangle(pos, size, coordQ2, coordQ1, rounded.X);

        coordQ1 = new Vector2(rounded.Y) - size;
        coordQ2 = new Vector2(rounded.Y);
        vertices.AddRectangle(pos + new Vector2(size.X, 0f), size, new Vector2(coordQ1.X, coordQ2.Y), new Vector2(coordQ2.X, coordQ1.Y), rounded.Y);

        coordQ1 = new Vector2(rounded.Z) - size;
        coordQ2 = new Vector2(rounded.Z);
        vertices.AddRectangle(pos + new Vector2(0f, size.Y), size, new Vector2(coordQ2.X, coordQ1.Y), new Vector2(coordQ1.X, coordQ2.Y), rounded.Z);


        coordQ1 = new Vector2(rounded.W) - size;
        coordQ2 = new Vector2(rounded.W);
        vertices.AddRectangle(pos + size, size, coordQ1, coordQ2, rounded.W);

        GraphicsDevice.DrawUserPrimitives(0, vertices.ToArray(), 0, vertices.Count / 3);

        Main.spriteBatch.spriteEffectPass.Apply();
    }

    public static void HasBorder(Vector2 pos, Vector2 size, Vector4 rounded, Color backgroundColor, float border,
        Color borderColor, bool ui = true)
    {
        const float innerShrinkage = 1;
        pos -= new Vector2(innerShrinkage);
        size += new Vector2(innerShrinkage * 2);
        rounded += new Vector4(innerShrinkage);
        EffectParameterCollection parameters = ShaderAssets.RoundedRectangle.Parameters;
        parameters["uTransform"].SetValue(GetMatrix(ui));
        parameters["uBackgroundColor"].SetValue(backgroundColor.ToVector4());
        parameters["uBorder"].SetValue(border);
        parameters["uBorderColor"].SetValue(borderColor.ToVector4());
        parameters["uInnerShrinkage"].SetValue(innerShrinkage);
        ShaderAssets.RoundedRectangle.CurrentTechnique.Passes[0].Apply();
        BaseDrawRectangle(pos, size, rounded);
    }

    public static void NoBorder(Vector2 pos, Vector2 size, Vector4 rounded, Color backgroundColor, bool ui = true)
    {
        const float innerShrinkage = 1;
        pos -= new Vector2(innerShrinkage);
        size += new Vector2(innerShrinkage * 2);
        rounded += new Vector4(innerShrinkage);
        ShaderAssets.RoundedRectangle.Parameters["uTransform"].SetValue(GetMatrix(ui));
        ShaderAssets.RoundedRectangle.Parameters["uBackgroundColor"].SetValue(backgroundColor.ToVector4());
        ShaderAssets.RoundedRectangle.Parameters["uInnerShrinkage"].SetValue(innerShrinkage);
        ShaderAssets.RoundedRectangle.CurrentTechnique.Passes[1].Apply();
        BaseDrawRectangle(pos, size, rounded);
    }

    public static void Shadow(Vector2 pos, Vector2 size, Vector4 rounded, Color backgroundColor, float shadow, bool ui = true)
    {
        ShaderAssets.RoundedRectangle.Parameters["uTransform"].SetValue(GetMatrix(ui));
        ShaderAssets.RoundedRectangle.Parameters["uBackgroundColor"].SetValue(backgroundColor.ToVector4());
        ShaderAssets.RoundedRectangle.Parameters["uShadowSize"].SetValue(shadow);
        ShaderAssets.RoundedRectangle.CurrentTechnique.Passes[2].Apply();
        BaseDrawRectangle(pos, size, rounded + new Vector4(shadow));
    }
}