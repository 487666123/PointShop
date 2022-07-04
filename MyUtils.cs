using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace PointShop
{
    public static class MyUtils
    {
        public static void DrawString(Vector2 position, string text, Color textColor, Color borderColor)
        {
            var sb = Main.spriteBatch;
            var font = FontAssets.MouseText.Value;
            Utils.DrawBorderStringFourWay(sb, font, text, position.X, position.Y, textColor, borderColor, Vector2.Zero, 1f);
        }

        public static void DrawString(Vector2 position, string text, Color textColor, Color borderColor, Vector2 origin, float scale)
        {
            var sb = Main.spriteBatch;
            var font = FontAssets.MouseText.Value;
            Utils.DrawBorderStringFourWay(sb, font, text, position.X, position.Y, textColor, borderColor, origin, scale);
        }

        // 绘制面板
        public static void DrawPanel(SpriteBatch sb, CalculatedStyle dimensions, Texture2D texture, Color color)
        {
            Point point = new Point((int)dimensions.X, (int)dimensions.Y);
            Point point2 = new Point(point.X + (int)dimensions.Width - 12, point.Y + (int)dimensions.Height - 12);
            int width = point2.X - point.X - 12;
            int height = point2.Y - point.Y - 12;
            sb.Draw(texture, new Rectangle(point.X, point.Y, 12, 12), new Rectangle(0, 0, 12, 12), color);
            sb.Draw(texture, new Rectangle(point2.X, point.Y, 12, 12), new Rectangle(12 + 4, 0, 12, 12), color);
            sb.Draw(texture, new Rectangle(point.X, point2.Y, 12, 12), new Rectangle(0, 12 + 4, 12, 12), color);
            sb.Draw(texture, new Rectangle(point2.X, point2.Y, 12, 12), new Rectangle(12 + 4, 12 + 4, 12, 12), color);
            sb.Draw(texture, new Rectangle(point.X + 12, point.Y, width, 12), new Rectangle(12, 0, 4, 12), color);
            sb.Draw(texture, new Rectangle(point.X + 12, point2.Y, width, 12), new Rectangle(12, 12 + 4, 4, 12), color);
            sb.Draw(texture, new Rectangle(point.X, point.Y + 12, 12, height), new Rectangle(0, 12, 12, 4), color);
            sb.Draw(texture, new Rectangle(point2.X, point.Y + 12, 12, height), new Rectangle(12 + 4, 12, 12, 4), color);
            sb.Draw(texture, new Rectangle(point.X + 12, point.Y + 12, width, height), new Rectangle(12, 12, 4, 4), color);
        }

        /// <summary>
        /// 获取MOD中的 Texture2D 资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Asset<Texture2D> GetTexture(string path)
        {
            return ModContent.Request<Texture2D>("PointShop/Images/" + path, AssetRequestMode.ImmediateLoad);
        }

        /// <summary>
        /// 获取 HJson 文字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetText(string str)
        {
            return Language.GetTextValue($"Mods.PointShop.{str}");
        }

        /// <summary>
        /// 数字转汉字（支持所有int型数字）
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NumberToChinese(int number)
        {
            string[] UNITS = { "", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千" };
            string[] NUMS = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            if (number == 0)
            {
                return NUMS[0];
            }
            string results = "";
            for (int i = number.ToString().Length - 1; i >= 0; i--)
            {
                int r = (int)(number / (Math.Pow(10, i)));
                results += NUMS[r % 10] + UNITS[i];
            }
            results = results.Replace("零十", "零")
                             .Replace("零百", "零")
                             .Replace("零千", "零")
                             .Replace("亿万", "亿");
            results = Regex.Replace(results, "零([万, 亿])", "$1");
            results = Regex.Replace(results, "零+", "零");

            if (results.StartsWith("一十"))
            {
                results = results.Substring(1);
            }
        cutzero:
            if (results.EndsWith("零"))
            {
                results = results.Substring(0, results.Length - 1);
                if (results.EndsWith("零"))
                {
                    goto cutzero;
                }
            }
            return results;
        }
    }
}
