using PointShop.Common.Systems;
using PointShop.Interface.GUI;
using Terraria.GameInput;

namespace PointShop.Common.Players
{
    public class KeybinPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybinSystem.RandomBuffKeybind.JustPressed)
            {
                ShopGUI.Visible = !ShopGUI.Visible;
            }
        }
    }
}