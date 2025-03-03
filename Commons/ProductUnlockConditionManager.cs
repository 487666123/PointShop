namespace PointShop.Commons;

/// <summary>
/// 商店物品解锁条件管理器
/// </summary>
public class ProductsUnlockConditionManager
{
    public static ProductsUnlockConditionManager Instance { get; } = new();

    private readonly Dictionary<string, ShopItemUnlockCondition> _registry = [];

    public bool Register(string name, Func<bool> condition)
    {
        var environment = new ShopItemUnlockCondition(name, condition);
        return _registry.TryAdd(name, environment);
    }

    public ShopItemUnlockCondition GetShopItemUnlockCondition(string name)
    {
        return _registry.GetValueOrDefault(name);
    }
}