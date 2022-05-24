using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PointShop.Common.Configs
{
    [Label("$Mods.PointShop.Config.积分兑换")]
    public class CoinConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.PointShop.Config.面板设置")]
        [Label("$Mods.PointShop.Config.积分统计面板")]
        [Tooltip("$Mods.PointShop.Config.是否显示屏幕上方的积分统计")]
        [DefaultValue(true)]
        public bool HuanJingFenPanel;

        [Label("$Mods.PointShop.Config.是否显示加分统计")]
        [Tooltip("$Mods.PointShop.Config.获取分时时候是否会被提示")]
        [DefaultValue(true)]
        public bool CombatJiaFen;
    }
}
