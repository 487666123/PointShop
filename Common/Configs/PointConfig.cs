using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PointShop.Common.Configs
{
    [Label("$Mods.PointShop.Config.PointExchange")]
    internal class PointConfig : ModConfig
    {
        public static PointConfig Instance;

        public override void OnLoaded()
        {
            Instance = this;
            MyUtils.Config = this;
        }

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [ReloadRequired]
        [Label("$Mods.PointShop.Config.PointMultiplier.Label")]
        [DefaultValue(100)]
        [Range(0, 800)]
        [Increment(10)]
        [Slider]
        public int PointMultiplier;
    }
}