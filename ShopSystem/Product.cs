namespace PointShop.ShopSystem;

public class Products
{
    /// <summary>
    /// 物品的 Type
    /// </summary>
    public int Type { get; set; } = 0;

    /// <summary>
    /// 一次兑换的数量
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// 积分价值
    /// </summary>
    public double PointsValue { get; set; } = 1f;

    /// <summary>
    /// 解锁条件的名称 (用于去对应字典中查询获取条件)
    /// </summary>
    public string UnlockConditionsName { get; set; } = string.Empty;
}