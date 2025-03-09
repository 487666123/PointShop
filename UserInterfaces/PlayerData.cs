using Terraria.ModLoader.IO;

namespace PointShop.Commons.Players;

internal class PlayerData : ModPlayer
{
    public static PlayerData Instance => Main.LocalPlayer.GetModPlayer<PlayerData>();
    public Vector2 PointShopPosition { get; set; }

    public override void LoadData(TagCompound tag)
    {
        if (tag.TryGet(nameof(PointShopPosition), out Vector2 pointShopOffset))
        {
            PointShopPosition = pointShopOffset;
        }
    }

    public override void SaveData(TagCompound tag)
    {
        tag[nameof(PointShopPosition)] = PointShopPosition;
    }
}