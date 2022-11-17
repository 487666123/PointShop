using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PointShop.Common.Configs
{
    [Label("$Mods.PointShop.Config.PointExchange")]
    public class PointConfig : ModConfig
    {
        internal static PointConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
            ModHelper.Config = this;
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

        [Label("$Mods.PointShop.Config.PointMultiplier.Label")]
        [DefaultValue(100)]
        [Range(0, 800)]
        [Increment(10)]
        [Slider]
        public int PointMultiplier;

        [Label("$Mods.PointShop.Config.UIYAxisOffset.Label")]
        [Tooltip("$Mods.PointShop.Config.UIYAxisOffset.Tooltip")]
        [DefaultValue(5f)]
        [Range(0, 5)]
        [Slider]
        [Increment(0.5f)]
        public float UIYAxisOffset;
    }
}
