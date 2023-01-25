using PointShop.Common.Configs;

namespace PointShop.Helpers
{
    public static class MyUtils
    {
        // 获取 Mod 配置信息
        public static PointConfig Config { get; set; }

        // 获取文字大小，正常文字
        public static Vector2 TextSize(string text, bool big = false)
        {
            if (big)
            {
                return FontAssets.DeathText.Value.MeasureString(text);
            }

            return FontAssets.MouseText.Value.MeasureString(text);
        }

        // 绘制文字
        public static void DrawText(Vector2 pos, string text, Color textColor, Color borderColor)
        {
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, text, pos.X, pos.Y, textColor,
                borderColor, Vector2.Zero);
        }

        // 绘制文字
        public static void DrawText(Vector2 pos, string text, Color textColor, Color borderColor, Vector2 origin,
            float scale)
        {
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, text, pos.X, pos.Y, textColor,
                borderColor, origin, scale);
        }

        // 绘制文字
        public static void DrawBigText(Vector2 pos, string text, Color textColor, Color borderColor, Vector2 origin,
            float scale)
        {
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.DeathText.Value, text, pos.X, pos.Y, textColor,
                borderColor, origin, scale);
        }

        // 获取贴图资源
        public static Asset<Texture2D> GetTexture(string path)
        {
            return ModContent.Request<Texture2D>("PointShop/Images/" + path, AssetRequestMode.ImmediateLoad);
        }

        public static Asset<Effect> GetEffect(string path)
        {
            return ModContent.Request<Effect>("PointShop/Effects/" + path, AssetRequestMode.ImmediateLoad);
        }

        // 获取翻译资源
        public static string GetText(string str)
        {
            return Language.GetTextValue($"Mods.PointShop.{str}");
        }
    }
}