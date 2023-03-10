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

    [Header("$Mods.PointShop.Config.PanelConfig")]
    [Label("$Mods.PointShop.Config.TerrainPanel.Label")]
    [Tooltip("$Mods.PointShop.Config.TerrainPanel.Tooltip")]
    [DefaultValue(true)]
    public bool TerrainPanel;

    [Label("$Mods.PointShop.Config.TerrainCombat.Label")]
    [Tooltip("$Mods.PointShop.Config.TerrainCombat.Tooltip")]
    [DefaultValue(true)]
    public bool TerrainCombat;

    [Label("$Mods.PointShop.Config.PointTipMode.Label")]
    [DefaultValue(true)]
    public PointTipMod PointTipMode;

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

public enum PointTipMod
{
    [Label("$Mods.PointShop.Config.PointTipMode.NoStack")]
    NoStack,

    [Label("$Mods.PointShop.Config.PointTipMode.Stack")]
    Stack,

    [Label("$Mods.PointShop.Config.PointTipMode.NoDisplay")]
    NoDisplay,

    [Label("$Mods.PointShop.Config.PointTipMode.WhatIsThis")]
    WhatIsThis
}