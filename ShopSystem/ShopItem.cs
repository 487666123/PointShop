namespace PointShop.ShopSystem;

/// <summary>
/// 商品
/// </summary>
public partial class ShopItem : IEventHandlerHolder
{
    public Mod Mod { get; }
    List<object> IEventHandlerHolder.ActiveHandlers { get; } = [];

    public virtual Asset<Texture2D> Icon { get; }
    private string _displayName = "特辣的海藻";
    public virtual string DisplayName { get => _displayName; set => _displayName = value; }
    public GameEnvironment Parent { get; }

    public WeakEventManager<EventArgs<double>> PricesChanged = new();
    public WeakEventManager<EventArgs<bool>> UnlockStateChanged = new();

    public double OriginalPrices;
    public partial double Prices { get; set; }
    public partial double Prices
    {
        get => field;
        set
        {
            if (field == value) return;
            field = value;
            PricesChanged.Raise(new(field));
        }
    }

    public readonly string UnlockCondition;
    public bool TryGetUnlockCondition(out UnlockCondition unlockCondition)
    {
        return PointShopSystem.TryGetUnlockCondition(UnlockCondition, out unlockCondition);
    }

    public ShopItem(Mod mod, GameEnvironment gameEnvironment, int prices, string unlockCondition)
    {
        Mod = mod;
        Parent = gameEnvironment;
        OriginalPrices = prices;
        Prices = prices * PointShopSystem.PricesMultiplier;
        UnlockCondition = unlockCondition;

        IsUnlock = true;

        if (TryGetUnlockCondition(out var condition))
        {
            condition.StateChanged.AddHandler(this,
                (_, args) => IsUnlock = args.Value);
        }

        PointShopSystem.OnPricesMultiplierChanged.AddHandler(this,
            (_, args) => Prices = OriginalPrices * args.Value);
    }

    public virtual void OnEnterWorld() { }
    public virtual void Update(GameTime gameTime)
    {
        PricesChanged.Update(gameTime);
        UnlockStateChanged.Update(gameTime);
    }

    public virtual void Buy() => Parent.PurchaseItems(this);

    private bool _isUnlock = true;
    public bool IsUnlock
    {
        get => _isUnlock;
        set
        {
            if (_isUnlock == value) return;
            _isUnlock = value;
            UnlockStateChanged.Raise(new(_isUnlock));
        }
    }

    /// <summary>
    /// 获得奖励
    /// </summary>
    /// <param name="player">一般是本地玩家</param>
    public virtual void GetRewards(Player player) { }
}

public class SimpleShopItem(Mod mod, GameEnvironment gameEnvironment, int prices, string conditionName, Item item) : ShopItem(mod, gameEnvironment, prices, conditionName)
{
    public Item Item { get; } = item;

    public override Asset<Texture2D> Icon
    {
        get
        {
            Main.instance.LoadItem(Item.type);
            return TextureAssets.Item[Item.type];
        }
    }

    public override string DisplayName => Item.Name;

    public override void GetRewards(Player player)
    {
        base.GetRewards(player);
        if (player is null) return;

        player.QuickSpawnItem(null, Item.type, Item.stack);
    }
}