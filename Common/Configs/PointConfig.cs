using PointShop.Helpers;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PointShop.Common.Configs
{
    [Label("$Mods.PointShop.Config.PointExchange")]
    public class PointConfig : ModConfig
    {
        public override void OnLoaded()
        {
            ModHelper.Config = this;
        }

        /// 获取配置
        public static PointConfig Get()
        {
            return ModContent.GetInstance<PointConfig>();
        }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.PointShop.Config.PanelConfig")]
        [Label("$Mods.PointShop.Config.TerrainPanel.Label")]
        [Tooltip("$Mods.PointShop.Config.TerrainPanel.Tooltip")]
        [DefaultValue(true)]
        public bool TerrainPanel;

        [Label("$Mods.PointShop.Config.TerrainCombat.Label")]
        [Tooltip("$Mods.PointShop.Config.TerrainCombat.Tooltip")]
        [DefaultValue(true)]
        public bool TerrainCombat;
    }
}
