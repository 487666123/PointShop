namespace PointShop.ShopSystem;

/// <summary>
/// 处理积分的各种操作
/// </summary>
public partial class PointShopPlayer : ModPlayer
{
    public readonly List<GameEnvironment> CurrentEnvironments = [];
    public readonly List<GameEnvironment> AverageEnvironments = [];

    public override void PreUpdate()
    {
        CurrentEnvironments.Clear();
        bool first = true;
        CurrentEnvironments.AddRange(PointShopSystem.Environments.Where(env =>
        {
            if (env.PointsSharingType is PointsSharingType.Void && !first) return false;

            if (env.Condition(Player))
            {
                first = false;
                return true;
            }
            return false;
        }));
        AverageEnvironments.Clear();
        AverageEnvironments.AddRange(CurrentEnvironments.Where(env => env.PointsSharingType == PointsSharingType.Average));
    }

    //public bool TryGetItemPoints(Item item, out double points)
    //{
    //    if (Player.whoAmI != Main.myPlayer || item.ModItem is not PointCoin coin)
    //    {
    //        points = 0f;
    //        return false;
    //    }

    //    points = coin.Points * item.stack;
    //    return true;
    //}

    //public override bool OnPickup(Item item)
    //{
    //    if (!TryGetItemPoints(item, out var points)) return true;

    //    var min = Math.Clamp(Player.luck + 0.75f, 0.5f, 0.75f);
    //    var max = Math.Max(Player.luck + 1.25f, 1.25f);
    //    points *= Main.rand.NextFloat(min, max);

    //    var eachPoints = points / AverageEnvironments.Count;

    //    foreach (var env in CurrentEnvironments)
    //    {
    //        var value = env.Type switch
    //        {
    //            GameEnvironmentType.Unique or GameEnvironmentType.Void => points,
    //            GameEnvironmentType.Average or _ => eachPoints,
    //        };
    //        SoundEngine.PlaySound(SoundID.Grab, null);
    //        IncreasePoint(env.Name, value);
    //        PointPopupHelper.Create(new Vector2(Player.position.X + Player.width / 2, Player.position.Y), env, value, 90);
    //    }
    //    return false;
    //}

    /// <summary> 获取积分 </summary>
    public double GetPoint(string name)
    {
        if (PointData.TryGetValue(name, out var value))
            return value;
        return 0;
    }

    /// <summary> 增加积分 </summary>
    public double IncreasePoint(string name, double point)
    {
        if (PointData.TryGetValue(name, out var value))
        {
            return PointData[name] = value + point;
        }
        PointData[name] = point;
        return point;
    }

    /// <summary> 积分足够 </summary>
    public bool EnoughPoints(string name, double point)
    {
        if (PointData.TryGetValue(name, out var value))
        {
            return value >= point;
        }

        return false;
    }

    /// <summary> 支付积分 </summary>
    public bool PayPoints(string name, double point, int quantity = 1)
    {
        if (PointData.TryGetValue(name, out var value))
        {
            if (value >= point * quantity)
            {
                PointData[name] = value - point * quantity;
                return true;
            }
        }

        return false;
    }

    public override void OnEnterWorld()
    {
        if (Player.whoAmI != Main.myPlayer) return;
        PointShopSystem.OnEnterWorld();
    }
}