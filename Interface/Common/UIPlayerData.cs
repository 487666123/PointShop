using Terraria.ModLoader.IO;

namespace PointShop.Interface.Common
{
    internal class UIPlayerData : ModPlayer
    {
        public Vector2 PointShopPos;

        public override void LoadData(TagCompound tag)
        {
            tag.TryGet(nameof(PointShopPos), out PointShopPos);
        }

        public override void SaveData(TagCompound tag)
        {
            if (UISystem.PointShopGUI != null)
                tag.Add(nameof(PointShopPos), UISystem.PointShopGUI.MainPanel.Pos());
        }
    }
}
