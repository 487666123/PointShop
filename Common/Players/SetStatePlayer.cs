using PointShop.Common.Systems;

namespace PointShop.Common.Players
{
    public class SetStatePlayer : ModPlayer
    {
        public bool loaded;
        public override void OnEnterWorld(Player player)
        {
            if (!loaded)
            {
                loaded = true;
                UISystem.PointInterface.SetState(UISystem.PointGUI = new());
                UISystem.PointShopInterface.SetState(UISystem.PointShopGUI = new());
            }
        }
    }
}
