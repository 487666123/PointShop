using System.Collections.Generic;
using PointShop.Items;
using Terraria.ModLoader.IO;

namespace PointShop.Commons;

public class PointShopPlayer : ModPlayer
{
    private Dictionary<string, double> PointData = [];


    public override bool OnPickup(Item item)
    {
        if (Player.whoAmI != Main.myPlayer || item.ModItem is not PointCoin coin) return true;

        var points = coin.Points * item.stack;
        var combatText = $"积分 +{points}";
        CombatText.NewText(Player.getRect(), Color.Red, combatText, dot: true);

        return false;
    }

    public override void SaveData(TagCompound tag)
    {
        var pointDataTag = new TagCompound();
        foreach (var (key, value) in PointData)
        {
            pointDataTag[key] = value;
        }

        tag[nameof(PointData)] = pointDataTag;
    }

    public override void LoadData(TagCompound tag)
    {
        if (!tag.TryGet(nameof(PointData), out TagCompound pointDataTag)) return;

        foreach (var (key, value) in pointDataTag)
        {
            if (value is double points)
                PointData[key] = points;
        }
    }
}