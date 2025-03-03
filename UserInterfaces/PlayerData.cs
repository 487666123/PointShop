using Terraria.ModLoader.IO;

namespace PointShop.Commons.Players;

internal class PlayerData : ModPlayer
{
    public static Vector2 PointShopOffset { get; set; }

    public static PlayerData Instance => Main.LocalPlayer.GetModPlayer<PlayerData>();

    public override void LoadData(TagCompound tag)
    {
        if (tag.TryGet(nameof(PointShopOffset), out Vector2 pointShopOffset))
        {
            PointShopOffset = pointShopOffset;
        }
    }

    public override void SaveData(TagCompound tag)
    {
        tag[nameof(PointShopOffset)] = PointShopOffset;
    }
}