using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PointShop.Common.Configs;

internal class UIConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public static UIConfig Instance;

    public override void OnLoaded()
    {
        base.OnLoaded();
        Instance = this;
    }

    [Label("$Mods.PointShop.Config.TextOffset.Label")]
    [Tooltip("$Mods.PointShop.Config.TextOffset.Tooltip")]
    [DefaultValue(7f)]
    [Range(0, 10f)]
    [Slider]
    [Increment(0.5f)]
    public float TextOffset;

    [Label("$Mods.PointShop.Config.BigTextOffset.Label")]
    [Tooltip("$Mods.PointShop.Config.BigTextOffset.Tooltip")]
    [DefaultValue(16f)]
    [Range(0, 20f)]
    [Slider]
    [Increment(0.5f)]
    public float BigTextOffset;
}