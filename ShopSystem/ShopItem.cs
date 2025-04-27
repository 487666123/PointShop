namespace PointShop.ShopSystem;

/// <summary>
/// 商品
/// </summary>
public class ShopItem : IEventHandlerHolder
{
    /// <summary>
    /// 添加此商品的 Mod（并非为此商品的所属 Mod）
    /// </summary>
    public Mod Mod { get; }

    List<object> IEventHandlerHolder.ActiveHandlers { get; } = [];

    /// <summary>
    /// 商品的图标
    /// </summary>
    public virtual Asset<Texture2D> Icon { get; }

    /// <summary>
    /// 商品的显示名称
    /// </summary>
    public virtual string DisplayName
    {
        get => field;
        set => field = value;
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
            IsUnlock = condition.IsUnlock;
            condition.StateChanged.AddHandler(this, (_, args) => IsUnlock = args.Value);
        }

        PointShopSystem.OnPricesMultiplierChanged.AddHandler(this,
            (_, args) => Prices = OriginalPrices * args.Value);
    }

    /// <summary>
    /// 此商品所在的环境
    /// </summary>
    public GameEnvironment Parent { get; }

    public readonly WeakEventManager<EventArgs<double>> PricesChanged = new();
    public readonly WeakEventManager<EventArgs<bool>> UnlockStateChanged = new();

    /// <summary>
    /// 商品的原始价格
    /// </summary>
    public double OriginalPrices { get; }

    /// <summary>
    /// 商品调整后的价格
    /// </summary>
    public double Prices
    {
        get => field;
        set
        {
            if (field == value) return;
            field = value;
            PricesChanged.Raise(new EventArgs<double>(field));
        }
    }

    public bool IsUnlock
    {
        get => field;
        set
        {
            if (field == value) return;
            field = value;
            UnlockStateChanged.Raise(new EventArgs<bool>(field));
        }
    }

    /// <summary>
    /// 商品解锁条件（查询名称）
    /// </summary>
    public readonly string UnlockCondition;

    /// <summary>
    /// 获取解锁条件，可能没有解锁条件
    /// </summary>
    /// <param name="unlockCondition">解锁条件</param>
    /// <returns>是否有解锁条件（通常没有解锁条件可以直接购买）</returns>
    public bool TryGetUnlockCondition(out UnlockCondition unlockCondition)
    {
        return PointShopSystem.TryGetUnlockCondition(UnlockCondition, out unlockCondition);
    }

    /// <summary>
    /// 本地玩家进入世界时调用
    /// </summary>
    public virtual void OnEnterWorld() { }

    /// <summary>
    /// 本地更新
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void Update(GameTime gameTime)
    {
        PricesChanged.Update(gameTime);
        UnlockStateChanged.Update(gameTime);
    }

    /// <summary>
    /// 可以直接调用方法购买此物品（它会调用父元素的 <see cref="GameEnvironment.PurchaseItems"/>）
    /// </summary>
    public virtual void Buy() => Parent.PurchaseItems(this);

    /// <summary>
    /// 当环境中的 <see cref="GameEnvironment.PurchaseItems"/> 方法对购买校验通过后，调用此方法以给予玩家奖励
    /// </summary>
    /// <param name="player">购买商品的玩家（通常是本地玩家）</param>
    public virtual void GetRewards(Player player) { }
}

/// <summary>
/// 简单商品，即 游戏道具 商品
/// </summary>
/// <param name="mod"></param>
/// <param name="gameEnvironment"></param>
/// <param name="prices"></param>
/// <param name="conditionName"></param>
/// <param name="item"></param>
public class SimpleShopItem(Mod mod, GameEnvironment gameEnvironment, int prices, string conditionName, Item item)
    : ShopItem(mod, gameEnvironment, prices, conditionName)
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

        player?.QuickSpawnItem(null, Item.type, Item.stack);
    }
}