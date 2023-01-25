using PointShop.Interface.GUI;

namespace PointShop.Interface.Common
{
    public class UIPlayer : ModPlayer
    {
        public override void OnEnterWorld(Player player)
        {
            UISystem.TerrainGUI = new TerrainGUI();
            UISystem.PointInterface.SetState(UISystem.TerrainGUI);

            PointShopGUI.PointMultiplier = MyUtils.Config.PointMultiplier;
            UISystem.PointShopGUI = new PointShopGUI();
            UISystem.PointShopInterface.SetState(UISystem.PointShopGUI);
        }
    }
}