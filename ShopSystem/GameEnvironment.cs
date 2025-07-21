namespace PointShop.ShopSystem;

public class GameEnvironment(
    Mod mod, Asset<Texture2D> icon, string name, int priority, Color uniqueColor,
    PointsSharingType pointsSharingType = PointsSharingType.Average) : IComparable<GameEnvironment>
{
    /// <summary>
    /// 向商店中添加此环境的 Mod（并非为此环境所受商品的所属 Mod）
    /// </summary>
    public Mod Mod { get; } = mod;

    /// <summary>
    /// 积分分享类型
    /// </summary>
    public PointsSharingType PointsSharingType { get; } = pointsSharingType;

    public virtual string DisplayName => Language.GetText($"Mods.{Mod.Name}.Environments.{Name}").Value;

    /// <summary>
    /// 注册一个环境时，此为唯一名称
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// 影响分享顺序（如果受顺序影响），也影响 UI 中排列的顺序
    /// </summary>
    public int Priority { get; } = priority;

    /// <summary>
    /// 环境的代表图标
    /// </summary>
    public Asset<Texture2D> Icon { get; } = icon;

    /// <summary>
    /// 环境的独特颜色，可以用于任何通过颜色强调此环境的地方<br/>
    /// 使用 virtual 是为了兼容某些环境在不同地图内拥有不一样的颜色表现
    /// </summary>
    public virtual Color UniqueColor { get; set; } = uniqueColor;

    /// <summary>
    /// 购买物品必须先通过此方法
    /// </summary>
    /// <param name="player">请求此操作的玩家</param>
    /// <returns>返回 true 代表玩家已经处于此环境，可以购买其中的物品</returns>
    public virtual bool Condition(Player player) => true;

    /// <summary>
    /// 当玩家进入世界时调用此方法
    /// </summary>
    public virtual void OnEnterWorld()
    {
        foreach (var shopItem in ShopItemList)
        {
            shopItem.OnEnterWorld();
        }
    }

    /// <summary>
    /// 玩家在地图中的更新方法
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void Update(GameTime gameTime)
    {
        foreach (var shopItem in _shopItemList)
        {
            shopItem.Update(gameTime);
        }
    }

    /// <summary>
    /// 此环境中所售商品列表
    /// </summary>
    private readonly List<ShopItem> _shopItemList = [];

    /// <summary>
    /// 用于获取环境中所有商品
    /// </summary>
    public IReadOnlyList<ShopItem> ShopItemList => _shopItemList;

    /// <summary>
    /// 添加商品
    /// </summary>
    /// <param name="shopItem"></param>
    public void AddShopItem(ShopItem shopItem)
    {
        if (shopItem is null) return;
        _shopItemList.Add(shopItem);
    }

    /// <summary>
    /// 使用此方法购买此环境中的物品<br/>
    /// 如若要购买成功需满足条件：商品属于此环境、此环境的积分大于商品价格
    /// </summary>
    /// <param name="shopItem">要购买的商品</param>
    public void PurchaseItems(ShopItem shopItem)
    {
        if (shopItem is null || PointShopPlayer.Local is not { } player) return;

        // 商品属于环境, 已解锁, 并且支付成功
        if (_shopItemList.Contains(shopItem))
        {
            if (!shopItem.IsUnlock)
            {
                Main.NewText($"{LanguageHelper.GetTextByPointShop("ShopItem.Lock").Value}");
                return;
            }

            if (!player.PayPoints(Name, shopItem.Prices))
            {
                Main.NewText($"{LanguageHelper.GetTextByPointShop("ShopItem.InsufficientPoints").Value}");
                return;
            }

            shopItem.GetRewards(player.Player);
            return;
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

public class SimpleEnvironment(
    Mod mod, Asset<Texture2D> icon, string name, Func<Player, bool> condition, int priority, Color uniqueColor,
    PointsSharingType pointsSharingType = PointsSharingType.Average)
    : GameEnvironment(mod, icon, name, priority, uniqueColor, pointsSharingType)
{
    public readonly Func<Player, bool> ConditionFunc = condition;

    public override bool Condition(Player player)
    {
        return ConditionFunc?.Invoke(player) ?? false;
    }
}

/// <summary>
/// 积分分享类型，决定玩家获取积分时，每一个环境如何获取积分
/// </summary>
public enum PointsSharingType
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