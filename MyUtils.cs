using LootCoins.Common.Configs;
using System;
using System.Text.RegularExpressions;
using Terraria.ModLoader;

namespace LootCoins
{
    public class MyUtils
    {
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

        /// 获取配置
        public static CoinConfig GetConfig()
        {
            return ModContent.GetInstance<CoinConfig>();
        }
    }
}
