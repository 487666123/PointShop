using PointShop.UserInterfaces;
using Terraria.GameInput;

namespace PointShop;

public class KeybindPlayer : ModPlayer
{
    /// <summary>
    /// 积分商店 UI 快捷键
    /// </summary>
    public static ModKeybind PointShopUIKeybind { get; private set; }

    public override void Load()
    {
        PointShopUIKeybind = KeybindLoader.RegisterKeybind(Mod, nameof(PointShopUIKeybind), "Mouse4");
    }

    public override void Unload()
    {
        PointShopUIKeybind = null;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (PointShopUIKeybind.JustPressed)
        {
            PointShopUI.SwitchStatus = !PointShopUI.SwitchStatus;
        }
    }
}