namespace PointShop.ShopSystem;

public class GameEnvironment(Mod mod, Asset<Texture2D> icon, string name, int priority, Color uniqueColor,
    GameEnvironmentType type = GameEnvironmentType.Average) : IComparable<GameEnvironment>
{
    public Mod Mod { get; } = mod;
    public GameEnvironmentType Type { get; } = type;
    public string DisplayName => Language.GetText($"Mods.{Mod.Name}.Environments.{Name}").Value;
    public string Name { get; } = name;
    public int Priority { get; } = priority;
    public Asset<Texture2D> Icon { get; } = icon;

    public virtual Color UniqueColor { get; set; } = uniqueColor;

    public virtual bool Condition(Player player) => true;

    public virtual void OnEnterWorld()
    {
        foreach (var shopItem in ShopItemList)
        {
            shopItem.OnEnterWorld();
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var shopItem in _shopItemList)
        {
            shopItem.Update(gameTime);
        }
    }

    private readonly List<ShopItem> _shopItemList = [];
    public IReadOnlyList<ShopItem> ShopItemList => _shopItemList;

    public void AddShopItem(ShopItem shopItem)
    {
        if (shopItem is null) return;
        _shopItemList.Add(shopItem);
    }

    public void PurchaseItems(ShopItem shopItem)
    {
        if (shopItem is null || PointShopPlayer.Local is not { } player) return;

        // 商品属于环境, 已解锁, 并且支付成功
        if (_shopItemList.Contains(shopItem))
        {
            if (!shopItem.IsUnlock)
            {
                Main.NewText($"{LanguageHelper.GetTextByPointShop("ShopItem.Lock").Value}"); return;
            }
            if (!player.PayPoints(Name, shopItem.Prices))
            {
                Main.NewText($"{LanguageHelper.GetTextByPointShop("ShopItem.InsufficientPoints").Value}"); return;
            }

            shopItem.GetRewards(player.Player); return;
        }

        Main.NewText($"{LanguageHelper.GetTextByPointShop("ShopItem.ShopItemError").Value}");
    }

    public virtual double GetPlayerPoints()
    {
        if (PointShopPlayer.Local is { } player)
        {
            return player.GetPoint(Name);
        }

        return 0;
    }

    public int CompareTo(GameEnvironment other) => -Priority.CompareTo(other.Priority);
}

public class SimpleEnvironment(Mod mod, Asset<Texture2D> icon, string name, Func<Player, bool> condition, int priority, Color uniqueColor,
    GameEnvironmentType type = GameEnvironmentType.Average) : GameEnvironment(mod, icon, name, priority, uniqueColor, type)
{
    public readonly Func<Player, bool> ConditionFunc = condition;

    public override bool Condition(Player player)
    {
        return ConditionFunc?.Invoke(player) ?? false;
    }
}

public enum GameEnvironmentType
{
    /// <summary>
    /// 平均的
    /// </summary>
    Average,

    /// <summary>
    /// 独享, 直接获取本次的所有积分 (不会影响平分的环境)
    /// </summary>
    Unique,

    /// <summary>
    /// 独享, 但是必须是优先级最高的, 否则一分没有
    /// </summary>
    Void,
}