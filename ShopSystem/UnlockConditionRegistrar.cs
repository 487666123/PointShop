namespace PointShop.ShopSystem;

public class UnlockConditionRegistrar : ModSystem
{
    public override void Load()
    {
        if (ModContent.GetInstance<PointShop>() is not { } pointShop) return;

        PointShopSystem.RegisterUnlockCondition(pointShop, "Hardmode", ModAsset.Hardmode, Condition.Hardmode.IsMet);
        PointShopSystem.RegisterUnlockCondition(pointShop, "DownedSkeletron", ModAsset.DownedSkeletron, Condition.DownedSkeletron.IsMet);
        PointShopSystem.RegisterUnlockCondition(pointShop, "DownedMechBossAny", ModAsset.DownedMechBossAny, Condition.DownedMechBossAny.IsMet);
        PointShopSystem.RegisterUnlockCondition(pointShop, "DownedPlantera", ModAsset.DownedPlantera, Condition.DownedPlantera.IsMet);
    }
}