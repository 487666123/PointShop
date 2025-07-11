namespace PointShop.Registrar;

public class ShopItemsRegistrar : ModSystem
{
    public override void PostSetupContent()
    {
        var shopData = GetShopData(FileHelper.GetString(FileHelper.ShopDataPath));
        RegisterShopData(Mod, shopData);
    }

    public static SimpleShopData GetShopData(string yamlDataString)
    {
        return FileHelper.YamlDeserializer.Deserialize<SimpleShopData>(yamlDataString);
    }

    public static void RegisterShopData(Mod mod, SimpleShopData shopData)
    {
        foreach (var environment in PointShopSystem.Environments)
        {
            foreach (var shopItemGenerator in shopData.CommonItems)
            {
                if (shopItemGenerator.Generate(mod, environment) is { } shopItem)
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

public class SimpleShopData
{
    public List<SimpleShopItemGenerator> CommonItems = [];

    public Dictionary<string, List<SimpleShopItemGenerator>> EnvironmentShopItems = [];
}

public class SimpleShopItemGenerator
{
    public string Type { get; set; } = "";
    public int Quantity { get; set; } = 0;
    public int Prices { get; set; } = 0;
    public string UnlockCondition { get; set; } = "";

    public SimpleShopItem Generate(Mod mod, GameEnvironment gameEnvironment)
    {
        if (!int.TryParse(Type, out var id))
        {
            if (!ItemID.Search.TryGetId(Type, out id)) { return null; }
        }
        var item = new Item(id, Quantity);
        return new SimpleShopItem(mod, gameEnvironment, Prices, UnlockCondition, item);
    }
}