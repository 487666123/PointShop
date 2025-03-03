namespace PointShop.ShopSystem;

/// <summary>
/// 商品
/// </summary>
public partial class ShopItem : IEventHandlerHolder
{
    List<object> IEventHandlerHolder.ActiveHandlers { get; } = [];

    public virtual Asset<Texture2D> Icon { get; }
    private string _displayName = "特辣的海藻";
    public virtual string DisplayName => _displayName;
    public GameEnvironment Parent { get; }

    public WeakEventManager<EventArgs<ShopItem>> OnPricesChanged = new();

    public double OriginalPrices;
    public partial double Prices { get; set; }
    public partial double Prices
    {
        get => field;
        set
        {
            if (field == value) return;
            field = value;
            OnPricesChanged.Raise(new EventArgs<ShopItem>(this));
        }
    }

    public readonly string UnlockCondition;
    public bool TryGetUnlockCondition(out UnlockCondition unlockCondition)
    {
        return PointShopSystem.TryGetUnlockCondition(UnlockCondition, out unlockCondition);
    }

    public ShopItem(GameEnvironment gameEnvironment, int prices, string unlockCondition)
    {
        Parent = gameEnvironment;
        OriginalPrices = prices;
        Prices = prices * PointShopSystem.PricesMultiplier;
        UnlockCondition = unlockCondition;

        PointShopSystem.OnPricesMultiplierChanged.AddHandler(this, (_, args) =>
        {
            Prices = OriginalPrices * args.Value;
        });
    }

    public virtual void OnEnterWorld() { }
    public virtual void Update(GameTime gameTime)
    {
        OnPricesChanged.Update(gameTime);
    }

    public virtual void Buy() => Parent.PurchaseItems(this);

    public virtual bool IsUnlock()
    {
        if (TryGetUnlockCondition(out var condition))
        {
            return condition.IsUnlock();
        }
        return true;
    }

    /// <summary>
    /// 获得奖励
    /// </summary>
    /// <param name="player">一般是本地玩家</param>
    public virtual void GetRewards(Player player) { }
}

public class SimpleShopItem(GameEnvironment gameEnvironment, int prices, string conditionName, Item item) : ShopItem(gameEnvironment, prices, conditionName)
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

        Item.NewItem(null, player.getRect(), Item.Clone());
    }
}