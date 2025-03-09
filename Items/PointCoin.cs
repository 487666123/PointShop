using Terraria.Audio;
using Terraria;
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
        ItemID.Sets.IsAPickup[Item.type] = true;
        ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;

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

    public override bool CanPickup(Player player)
    {
        return true;
    }

    public override bool OnPickup(Player player)
    {
        if (!player.TryGetModPlayer<PointShopPlayer>(out var shopPlayer)) return true;

        var points = Points * Item.stack;

        var min = Math.Clamp(player.luck + 0.75f, 0.5f, 0.75f);
        var max = Math.Max(player.luck + 1.25f, 1.25f);
        points *= Main.rand.NextFloat(min, max);

        var eachPoints = points / shopPlayer.AverageEnvironments.Count;

        foreach (var env in shopPlayer.CurrentEnvironments)
        {
            var value = env.Type switch
            {
                GameEnvironmentType.Unique or GameEnvironmentType.Void => points,
                GameEnvironmentType.Average or _ => eachPoints,
            };
            SoundEngine.PlaySound(SoundID.Grab, null);
            shopPlayer.IncreasePoint(env.Name, value);
            PointPopupHelper.Create(new Vector2(player.position.X + player.width / 2, player.position.Y), env, value, 90);
        }
        return false;
    }

    public override void GrabRange(Player player, ref int grabRange)
    {
        grabRange += 16 * 30;
    }
}