using PointShop.Common.Systems;
using PointShop.UI;
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
                ShopState.Visible = !ShopState.Visible;
            }
        }
    }
}
