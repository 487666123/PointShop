using System.ComponentModel;
using PointShop.UserInterfaces;
using PointShop.UserInterfaces.DisplayUI;
using Terraria.ModLoader.Config;

namespace PointShop;

public class PointShopClientSideConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(true)]
    public bool PointsDisplayWidget { get; set; }

    [DefaultValue(true)]
    public bool DisplayLinks { get; set; }

    public override void OnChanged()
    {

        if (UISceneManager.Instance.TryGetInstance(out PointsDisplayWidgetUI widget))
        {
            PointShopFooter.DisplayLinks = DisplayLinks;
            widget.EnableBlur = DisplayLinks;
        }
    }
}