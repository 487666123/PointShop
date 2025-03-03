using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;

namespace PointShop;

internal class PointShopConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Slider]
    [Range(0.05f, 2f)]
    [Increment(0.05f)]
    [DefaultValue(0.5f)]
    [CustomModConfigItem(typeof(RoundFloatElement))]
    public float PointMultiplier;

    public override void OnChanged()
    {
        PointShopSystem.PricesMultiplier = PointMultiplier;
    }
}

public class RoundFloatElement : FloatElement
{
    public override void SetValue(object value)
    {
        base.SetValue(MathF.Round((float)value, 2));
    }
}