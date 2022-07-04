using PointShop.Common.Systems;
using PointShop.Interface;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace PointShop.Common.Players
{
    public class KeybinPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybinSystem.RandomBuffKeybind.JustPressed)
            {
                PointShopGUI.Visible = !PointShopGUI.Visible;
            }
        }
    }
}
