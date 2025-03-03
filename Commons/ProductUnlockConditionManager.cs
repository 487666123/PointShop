namespace PointShop.Commons;

/// <summary>
/// 商店物品解锁条件管理器
/// </summary>
public class ProductUnlockConditionManager
{
    public static ProductUnlockConditionManager Instance { get; } = new();

    private readonly Dictionary<string, ProductUnlockCondition> _registry = [];

    public bool Register(string name, Func<bool> condition)
    {
        var environment = new ProductUnlockCondition(name, condition);
        return _registry.TryAdd(name, environment);
    }

    public ProductUnlockCondition GetProductUnlockCondition(string name)
    {
        return _registry.GetValueOrDefault(name);
    }
}