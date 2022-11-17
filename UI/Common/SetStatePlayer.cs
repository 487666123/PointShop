using PointShop.Common.Systems;

namespace PointShop.Interface.Common
{
    public class SetStatePlayer : ModPlayer
    {
        public override void OnEnterWorld(Player player)
        {
            UISystem.PointInterface.SetState(UISystem.PointGUI = new());
            UISystem.PointShopInterface.SetState(UISystem.PointShopGUI = new());
        }
    }
}
