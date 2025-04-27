using Terraria.ModLoader.IO;

namespace PointShop.UserInterfaces;

internal class PlayerData : ModPlayer
{
    /// <summary>
    /// 快速获取实例
    /// </summary>
    public static PlayerData Instance => Main.LocalPlayer.GetModPlayer<PlayerData>();

    /// <summary>
    /// 积分商店 UI 的位置
    /// </summary>
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