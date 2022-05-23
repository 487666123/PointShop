using LootCoins.Common.Systems;
using LootCoins.Content.UI;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace LootCoins.Common.Players
{
    public class KeybinPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybinSystem.RandomBuffKeybind.JustPressed)
            {
                CoinUI.Visible = !CoinUI.Visible;
            }
        }
    }
}
