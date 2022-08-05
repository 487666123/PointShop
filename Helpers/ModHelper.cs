using Microsoft.Xna.Framework.Graphics;
using PointShop.Common.Configs;
using Terraria.Localization;
using Terraria.UI;

namespace PointShop.Helpers
{
    public static class ModHelper
    {
        // 获取 Mod 配置信息
        public static PointConfig Config { get; set; }

        // 获取文字大小，正常文字
        public static Vector2 GetTextSize(string text) => FontAssets.MouseText.Value.MeasureString(text);

        // 获取文字大小，大文字
        public static Vector2 GetTextSize_Big(string text) => FontAssets.DeathText.Value.MeasureString(text);

        // 绘制文字
        public static void DrawString(Vector2 position, string text, Color textColor, Color borderColor)
        {
            var sb = Main.spriteBatch;
            var font = FontAssets.MouseText.Value;
            Utils.DrawBorderStringFourWay(sb, font, text, position.X, position.Y, textColor, borderColor, Vector2.Zero, 1f);
        }

        // 绘制文字
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

        // 获取贴图资源
        public static Asset<Texture2D> GetTexture(string path) => ModContent.Request<Texture2D>("PointShop/Images/" + path, AssetRequestMode.ImmediateLoad);
        public static Asset<Effect> GetEffect(string path) => ModContent.Request<Effect>("PointShop/Effects/" + path, AssetRequestMode.ImmediateLoad);

        // 获取翻译资源
        public static string GetText(string str) => Language.GetTextValue($"Mods.PointShop.{str}");

        /*// 数字转汉字（支持所有int型数字）
        public static string DigitalToChinese(int number)
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
        }*/

        /// <summary>
        /// 线段与矩形是否相交
        /// </summary>
        /// <param name="linePointX1"></param>
        /// <param name="linePointY1"></param>
        /// <param name="linePointX2"></param>
        /// <param name="linePointY2"></param>
        /// <param name="rectangleLeftTopX"></param>
        /// <param name="rectangleLeftTopY"></param>
        /// <param name="rectangleRightBottomX"></param>
        /// <param name="rectangleRightBottomY"></param>
        /// <returns></returns>
        public static bool isLineIntersectRectangle(float linePointX1,
                                          float linePointY1,
                                        float linePointX2,
                                         float linePointY2,
                                         float rectangleLeftTopX,
                                          float rectangleLeftTopY,
                                          float rectangleRightBottomX,
                                          float rectangleRightBottomY)
        {
            float lineHeight = linePointY1 - linePointY2;
            float lineWidth = linePointX2 - linePointX1;  // 计算叉乘 
            float c = linePointX1 * linePointY2 - linePointX2 * linePointY1;
            if (lineHeight * rectangleLeftTopX + lineWidth * rectangleLeftTopY + c >= 0 && lineHeight * rectangleRightBottomX + lineWidth * rectangleRightBottomY + c <= 0
                || lineHeight * rectangleLeftTopX + lineWidth * rectangleLeftTopY + c <= 0 && lineHeight * rectangleRightBottomX + lineWidth * rectangleRightBottomY + c >= 0
                || lineHeight * rectangleLeftTopX + lineWidth * rectangleRightBottomY + c >= 0 && lineHeight * rectangleRightBottomX + lineWidth * rectangleLeftTopY + c <= 0
                || lineHeight * rectangleLeftTopX + lineWidth * rectangleRightBottomY + c <= 0 && lineHeight * rectangleRightBottomX + lineWidth * rectangleLeftTopY + c >= 0)
            {

                if (rectangleLeftTopX > rectangleRightBottomX)
                {
                    float temp = rectangleLeftTopX;
                    rectangleLeftTopX = rectangleRightBottomX;
                    rectangleRightBottomX = temp;
                }
                if (rectangleLeftTopY < rectangleRightBottomY)
                {
                    float temp1 = rectangleLeftTopY;
                    rectangleLeftTopY = rectangleRightBottomY;
                    rectangleRightBottomY = temp1;
                }
                if (linePointX1 < rectangleLeftTopX && linePointX2 < rectangleLeftTopX
                    || linePointX1 > rectangleRightBottomX && linePointX2 > rectangleRightBottomX
                    || linePointY1 > rectangleLeftTopY && linePointY2 > rectangleLeftTopY
                    || linePointY1 < rectangleRightBottomY && linePointY2 < rectangleRightBottomY)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }


        public static bool LineAndRectangle(Vector2 lineStart, Vector2 lineEnd, Vector2 rectangleStart, Vector2 rectangleEnd)
        {
            float lineHeight = lineStart.Y - lineEnd.Y;
            float lineWidth = lineEnd.X - lineStart.X;  // 计算叉乘 
            float c = lineStart.X * lineEnd.Y - lineEnd.X * lineStart.Y;

            if (lineHeight * rectangleStart.X + lineWidth * rectangleStart.Y + c >= 0 && lineHeight * rectangleEnd.X + lineWidth * rectangleEnd.Y + c <= 0 ||
                lineHeight * rectangleStart.X + lineWidth * rectangleStart.Y + c <= 0 && lineHeight * rectangleEnd.X + lineWidth * rectangleEnd.Y + c >= 0 ||
                lineHeight * rectangleStart.X + lineWidth * rectangleEnd.Y + c >= 0 && lineHeight * rectangleEnd.X + lineWidth * rectangleStart.Y + c <= 0 ||
                lineHeight * rectangleStart.X + lineWidth * rectangleEnd.Y + c <= 0 && lineHeight * rectangleEnd.X + lineWidth * rectangleStart.Y + c >= 0)
            {
                if (rectangleStart.X > rectangleEnd.X)
                {
                    (rectangleEnd.X, rectangleStart.X) = (rectangleStart.X, rectangleEnd.X);
                }
                if (rectangleStart.Y < rectangleEnd.Y)
                {
                    (rectangleEnd.Y, rectangleStart.Y) = (rectangleStart.Y, rectangleEnd.Y);
                }
                if (lineStart.X < rectangleStart.X && lineEnd.X < rectangleStart.X ||
                    lineStart.X > rectangleEnd.X && lineEnd.X > rectangleEnd.X ||
                    lineStart.Y > rectangleStart.Y && lineEnd.Y > rectangleStart.Y ||
                    lineStart.Y < rectangleEnd.Y && lineEnd.Y < rectangleEnd.Y)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }
    }
}
