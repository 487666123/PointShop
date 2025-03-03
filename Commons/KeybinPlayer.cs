using Terraria.GameInput;

namespace PointShop.Commons;

public class KeybinPlayer : ModPlayer
{
    public static ModKeybind RandomBuffKeybind { get; private set; }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (RandomBuffKeybind.JustPressed)
        {
            // ShopGUI.Visible = !ShopGUI.Visible;
        }
    }

    public override void Load()
    {
        var keybinName = Language.GetText("Mods.PointShop.Keybin.SuperVault").Value;
        RandomBuffKeybind = KeybindLoader.RegisterKeybind(Mod, keybinName, "P");
    }

    public override void Unload()
    {
        RandomBuffKeybind = null;
    }
}