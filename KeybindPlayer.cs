using PointShop.UserInterfaces;
using Terraria.GameInput;

namespace PointShop;

public class KeybindPlayer : ModPlayer
{
    public static ModKeybind PointShopUIKeybind { get; private set; }

    public override void Load()
    {
        PointShopUIKeybind = KeybindLoader.RegisterKeybind(Mod, nameof(PointShopUIKeybind), "P");
    }

    public override void Unload()
    {
        PointShopUIKeybind = null;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (PointShopUIKeybind.JustPressed)
        {
            PointShopUI.OpenUI = !PointShopUI.OpenUI;
        }
    }
}