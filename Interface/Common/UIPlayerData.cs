using Terraria.ModLoader.IO;

namespace PointShop.Interface.Common
{
    internal class UIPlayerData : ModPlayer
    {
        public static UIPlayerData Local => Main.LocalPlayer.GetModPlayer<UIPlayerData>();

        public Vector2 PointShopPos;
        public float TerrainPosX;

        public override void LoadData(TagCompound tag)
        {
            tag.TryGet("PointShopPos", out PointShopPos);
            tag.TryGet("TerrainPosX", out TerrainPosX);
        }

        public override void SaveData(TagCompound tag)
        {
            if (UISystem.PointShopGUI != null)
            {
                tag.Add("PointShopPos", UISystem.PointShopGUI.MainPanel.Pos());
            }

            if (UISystem.TerrainGUI != null)
            {
                tag.Add("TerrainPosX", UISystem.TerrainGUI.MainPanel.Left.Pixels);
            }
        }
    }
}