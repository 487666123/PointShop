using PointShop.Commons;
using Terraria.DataStructures;

namespace PointShop.Items;

public class PointCoin : ModItem
{
    public double Points { get; set; } = 100;

    public override void SetStaticDefaults()
    {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));

        ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        ItemID.Sets.ItemNoGravity[Item.type] = true;

        Item.ResearchUnlockCount = 25;
    }

    public override void SetDefaults()
    {
        Item.width = Item.height = 18;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.sellPrice(silver: 1);
        Item.rare = ItemRarityID.Red;
    }

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        Lighting.AddLight(Item.Center, Color.Yellow.ToVector3() * 0.5f);
        base.Update(ref gravity, ref maxFallSpeed);
    }
}