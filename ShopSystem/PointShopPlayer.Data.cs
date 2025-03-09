using Terraria.ModLoader.IO;

namespace PointShop.ShopSystem;

public partial class PointShopPlayer
{
    // 积分数据
    public readonly Dictionary<string, double> PointData = [];

    /// <summary>
    /// 保存玩家积分数据
    /// </summary>
    public override void SaveData(TagCompound tag)
    {
        var pointDataTag = new TagCompound();
        foreach (var (key, value) in PointData)
        {
            pointDataTag[key] = value;
        }

        tag[nameof(PointData)] = pointDataTag;
    }

    /// <summary>
    /// 加载玩家积分数据
    /// </summary>
    public override void LoadData(TagCompound tag)
    {
        if (!tag.TryGet(nameof(PointData), out TagCompound pointDataTag)) return;

        foreach (var (key, value) in pointDataTag)
        {
            if (value is double points)
                PointData[key] = Math.Clamp(points, 0, 1L << 40);
        }
    }
}
