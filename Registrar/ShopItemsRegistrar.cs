namespace PointShop.Registrar;

/// <summary>
/// 从 YAML 商店配置注册商品到各个游戏环境。
/// </summary>
public class ShopItemsRegistrar : ModSystem
{
    /// <summary>
    /// 在内容加载完成后读取商店配置并注册。
    /// </summary>
    public override void PostSetupContent()
    {
        var shopData = FileHelper.DeserializeYaml<SimpleShopData>(FileHelper.GetString(FileHelper.ShopDataPath));
        RegisterShopData(Mod, shopData);
    }

    /// <summary>
    /// 将配置中的通用商品与环境专属商品注入到已注册环境中。
    /// </summary>
    /// <param name="mod">当前 Mod 实例。</param>
    /// <param name="shopData">反序列化后的商店配置。</param>
    public static void RegisterShopData(Mod mod, SimpleShopData shopData)
    {
        foreach (var environment in PointShopSystem.Environments)
        {
            foreach (var shopItemGenerator in shopData.CommonItems)
            {
                if (shopItemGenerator.Generate(mod, environment, true) is { } shopItem)
                {
                    environment.AddShopItem(shopItem);
                }
            }

            if (!shopData.EnvironmentShopItems.TryGetValue(environment.Name, out var simpleShopItemWrappers)) continue;
            foreach (var shopItemGenerator in shopData.EnvironmentShopItems[environment.Name])
            {
                if (shopItemGenerator.Generate(mod, environment) is { } shopItem)
                {
                    environment.AddShopItem(shopItem);
                }
            }
        }
    }
}

/// <summary>
/// 商店配置根对象，对应 <c>ShopData/data.yaml</c>。
/// </summary>
public class SimpleShopData
{
    /// <summary>
    /// 对所有环境都生效的商品配置。
    /// </summary>
    public List<SimpleShopItemGenerator> CommonItems = [];

    /// <summary>
    /// 按环境名称分组的商品配置。
    /// Key 为环境 <see cref="GameEnvironment.Name"/>。
    /// </summary>
    public Dictionary<string, List<SimpleShopItemGenerator>> EnvironmentShopItems = [];
}

/// <summary>
/// 简单商品生成器，对应 YAML 中单个商品节点。
/// </summary>
public class SimpleShopItemGenerator
{
    /// <summary>
    /// 物品类型。可填物品 ID（数字）或形如 <c>ModName/ItemName</c> 的完整名称。
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// 购买一次发放的数量。
    /// </summary>
    public int Quantity { get; set; } = 0;

    /// <summary>
    /// 商品价格（基础值，会再叠加价格倍率）。
    /// </summary>
    public int Prices { get; set; } = 0;

    /// <summary>
    /// 解锁条件名称。为空时默认可购买。
    /// </summary>
    public string UnlockCondition { get; set; } = "";

    /// <summary>
    /// 根据配置生成可加入环境商店的 <see cref="SimpleShopItem"/>。
    /// </summary>
    /// <param name="mod">当前 Mod 实例。</param>
    /// <param name="gameEnvironment">商品所属环境。</param>
    /// <param name="commonItem">是否为通用商品。</param>
    /// <returns>生成成功返回商品；若 <see cref="Type"/> 无法解析则返回 <c>null</c>。</returns>
    public SimpleShopItem Generate(Mod mod, GameEnvironment gameEnvironment, bool commonItem = false)
    {
        if (!int.TryParse(Type, out var id))
        {
            if (!ItemID.Search.TryGetId(Type, out id)) { return null; }
        }

        var item = new Item(id, Quantity);
        return new SimpleShopItem(mod, gameEnvironment, Prices, UnlockCondition, item, commonItem);
    }
}
