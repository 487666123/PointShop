namespace PointShop.ShopSystem;

public class ShopItemsRegistrar : ModSystem
{
    public override void PostSetupContent()
    {
        var shopData = GetShopData();
        RegisterShopData(shopData);
    }

    public static SimpleShopData GetShopData()
    {
        var shopDataString = FilesHelper.GetString(FilesHelper.ShopDataPath);
        return FilesHelper.YAMLDeserializer.Deserialize<SimpleShopData>(shopDataString);
    }

    public static void RegisterShopData(SimpleShopData shopData)
    {
        foreach (var environment in PointShopSystem.Environments)
        {
            foreach (var shopItemGenerator in shopData.CommonItems)
            {
                if (shopItemGenerator.Generate(environment) is { } shopItem)
                {
                    environment.AddShopItem(shopItem);
                }
            }

            if (!shopData.EnvironmentShopItems.TryGetValue(environment.Name, out var simpleShopItemWrappers)) continue;
            foreach (var shopItemGenerator in shopData.EnvironmentShopItems[environment.Name])
            {
                if (shopItemGenerator.Generate(environment) is { } shopItem)
                {
                    environment.AddShopItem(shopItem);
                }
            }
        }
    }
}

public class SimpleShopData
{
    public List<SimpleShopItemGenerator> CommonItems = [];

    public Dictionary<string, List<SimpleShopItemGenerator>> EnvironmentShopItems = [];
}

public class SimpleShopItemGenerator
{
    public string Type { get; set; }
    public int Quantity { get; set; }
    public int Prices { get; set; }
    public string UnlockCondition { get; set; }

    public SimpleShopItem Generate(GameEnvironment gameEnvironment)
    {
        if (!int.TryParse(Type, out var id))
        {
            if (!ItemID.Search.TryGetId(Type, out id)) { return null; }
        }
        var item = new Item(id, Quantity);
        return new SimpleShopItem(gameEnvironment, Prices, UnlockCondition, item);
    }
}