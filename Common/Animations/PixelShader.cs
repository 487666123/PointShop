namespace PointShop.Common.Animations
{
    public static class PixelShader
    {
        private struct VertexPos : IVertexType
        {
            private static readonly VertexDeclaration DefaultVertexDeclaration;

            static VertexPos()
            {
                DefaultVertexDeclaration = new VertexDeclaration(
                    new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                    new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));
            }

            public Vector2 Position;
            public Vector2 Coord;

            public VertexPos(Vector2 position, Vector2 coord)
            {
                Position = position;
                Coord = coord;
            }

            public VertexDeclaration VertexDeclaration => DefaultVertexDeclaration;
        }

        private static VertexPos[] GetVertexPos(Vector2 pos, Vector2 size)
        {
            return new VertexPos[]
            {
                new(pos, new Vector2(0, 0)),
                new(pos + new Vector2(size.X, 0), new Vector2(1, 0)),
                new(pos + new Vector2(0, size.Y), new Vector2(0, 1)),
                new(pos + new Vector2(0, size.Y), new Vector2(0, 1)),
                new(pos + new Vector2(size.X, 0), new Vector2(1, 0)),
                new(pos + size, new Vector2(1, 1))
            };
        }

        private static Matrix GetMatrix(bool ui)
        {
            if (ui)
            {
                return Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            }

            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Vector2 offset = screenSize * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom) / 2;
            return Matrix.CreateOrthographicOffCenter(offset.X, Main.screenWidth - offset.X,
                Main.screenHeight - offset.Y, offset.Y, 0, 1);
        }

        private static void BaseDraw(Vector2 pos, Vector2 size, bool ui, Action<Matrix> action)
        {
            VertexPos[] triangles = GetVertexPos(pos, size);
            action.Invoke(GetMatrix(ui));
            Main.graphics.GraphicsDevice.DrawUserPrimitives(0, triangles, 0, triangles.Length / 3);
        }

        public static void RoundedRectangle(Vector2 pos, Vector2 size, Vector4 round4, Color backgroundColor,
            float border,
            Color borderColor, bool ui = true)
        {
            const float innerShrinkage = 1;
            pos -= new Vector2(innerShrinkage);
            size += new Vector2(innerShrinkage * 2);
            round4 += new Vector4(innerShrinkage);
            BaseDraw(pos, size, ui, matrix =>
            {
                Effect effect = ShaderAssets.RoundedRectangle;
                effect.Parameters["uTransform"].SetValue(matrix);
                effect.Parameters["uSize"].SetValue(size);
                effect.Parameters["uSizeOver2"].SetValue(size / 2);
                effect.Parameters["uRounded"].SetValue(round4);
                effect.Parameters["uBackgroundColor"].SetValue(backgroundColor.ToVector4());
                effect.Parameters["uBorder"].SetValue(border);
                effect.Parameters["uBorderColor"].SetValue(borderColor.ToVector4());
                effect.Parameters["uInnerShrinkage"].SetValue(innerShrinkage);
                effect.CurrentTechnique.Passes["HasBorder"].Apply();
            });
        }

        public static void RoundedRectangle(Vector2 pos, Vector2 size, Vector4 round4, Color backgroundColor,
            bool ui = true)
        {
            const float innerShrinkage = 1;
            pos -= new Vector2(innerShrinkage);
            size += new Vector2(innerShrinkage * 2);
            round4 += new Vector4(innerShrinkage);
            BaseDraw(pos, size, ui, matrix =>
            {
                Effect effect = ShaderAssets.RoundedRectangle;
                effect.Parameters["uTransform"].SetValue(matrix);
                effect.Parameters["uSize"].SetValue(size);
                effect.Parameters["uSizeOver2"].SetValue(size / 2);
                effect.Parameters["uRounded"].SetValue(round4);
                effect.Parameters["uBackgroundColor"].SetValue(backgroundColor.ToVector4());
                effect.Parameters["uInnerShrinkage"].SetValue(innerShrinkage);
                effect.CurrentTechnique.Passes["NoBorder"].Apply();
            });
        }

        /// <summary>
        /// 绘制叉号
        /// </summary>
        public static void Cross(Vector2 pos, float size, float round, Color backgroundColor, float border,
            Color borderColor, bool ui = true)
        {
            BaseDraw(pos, new Vector2(size), ui, matrix =>
            {
                Effect effect = ShaderAssets.Cross;
                effect.Parameters["uTransform"].SetValue(matrix);
                effect.Parameters["size"].SetValue(size);
                effect.Parameters["border"].SetValue(border);
                effect.Parameters["round"].SetValue(round);
                effect.Parameters["borderColor"].SetValue(borderColor.ToVector4());
                effect.Parameters["backgroundColor"].SetValue(backgroundColor.ToVector4());
                effect.CurrentTechnique.Passes[0].Apply();
            });
        }
    }
}