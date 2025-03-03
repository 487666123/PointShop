namespace PointShop.Commons;

/// <summary>
/// 商店物品解锁条件
/// </summary>
public class ProductUnlockCondition(string name, Func<bool> condition)
{
    public string DisplayName =>
        LanguageHelper.GetTextByPointShop($"ShopItemUnlockCondition.{Name}").Value;

    public string Name { get; } = name;

    public Func<bool> Condition { get; } = condition;
}