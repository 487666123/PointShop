using SilkyUIFramework.Helper;

namespace PointShop.ShopSystem;

public class PointShopSystem : ModSystem
{
    public static readonly WeakEventManager<EventArgs<float>> OnPricesMultiplierChanged = new();

    public static float PricesMultiplier
    {
        get;
        set
        {
            if (value == field) return;

            field = value;
            OnPricesMultiplierChanged.Raise(new EventArgs<float>(value));
        }
    }

    /// <summary> 解锁条件表 </summary>
    private static readonly Dictionary<string, UnlockCondition> _unlockConditionTable = [];

    /// <summary> 环境表 </summary>
    private static readonly Dictionary<string, GameEnvironment> _environmentTable = [];

    private static List<GameEnvironment> _environments = [];
    public static IReadOnlyList<GameEnvironment> Environments => _environments;
    public static IReadOnlyDictionary<string, UnlockCondition> UnlockConditions => _unlockConditionTable;
    /// <summary> 注册游戏环境 </summary>
    public static bool RegisterGameEnvironment(GameEnvironment environment)
    {
        if (!_environmentTable.TryAdd(environment.Name, environment)) return false;

        _environments.Add(environment);
        _environments = [.. _environments.OrderByDescending(env => env.Priority)];
        return true;
    }

    /// <summary> 注册游戏环境 </summary>
    public static bool RegisterGameEnvironment(Mod mod, Asset<Texture2D> icon, string name,
        Func<Player, bool> condition, int priority, Color uniqueColor,
        PointsSharingType type = PointsSharingType.Average)
    {
        var environment = new SimpleEnvironment(mod, icon, name, condition, priority, uniqueColor, type);
        return RegisterGameEnvironment(environment);
    }

    /// <summary> 获取游戏环境 </summary>
    public static bool TryGetGameEnvironment(string name, out GameEnvironment gameEnvironment)
    {
        if (name is null)
        {
            gameEnvironment = null;
            return false;
        }

        return _environmentTable.TryGetValue(name, out gameEnvironment);
    }

    /// <summary> 注册条件 </summary>
    public static bool RegisterUnlockCondition(Mod mod, string name, Asset<Texture2D> icon, Func<bool> unlockCondition)
    {
        return RegisterUnlockCondition(new SimpleUnlockCondition(mod, name, icon, unlockCondition));
    }

    /// <summary> 注册条件 </summary>
    public static bool RegisterUnlockCondition(UnlockCondition unlockCondition)
    {
        return _unlockConditionTable.TryAdd(unlockCondition.Name, unlockCondition);
    }

    /// <summary> 获取条件 </summary>
    public static bool TryGetUnlockCondition(string name, out UnlockCondition unlockCondition)
    {
        if (name is null)
        {
            unlockCondition = null;
            return false;
        }

        return _unlockConditionTable.TryGetValue(name, out unlockCondition);
    }

    public static Dictionary<string, Func<bool>> GetAllUnlockConditionTable()
    {
        var table = new Dictionary<string, Func<bool>>();

        foreach (var item in _unlockConditionTable)
        {
            table[item.Key] = () => item.Value.IsUnlock;
        }

        return table;
    }

    public static void OnEnterWorld()
    {
        foreach (var gameEnvironment in Environments)
        {
            gameEnvironment.OnEnterWorld();
        }
    }

    public static void Update(GameTime gameTime)
    {
        OnPricesMultiplierChanged.Update(gameTime);

        foreach (var gameEnvironment in Environments)
        {
            gameEnvironment.Update(gameTime);
        }

        foreach (var (_, unlockCondition) in _unlockConditionTable)
        {
            unlockCondition.Update(gameTime);
        }
    }

    public override void Load()
    {
        On_Main.Update += (orig, self, gameTime) =>
        {
            RuntimeSafeHelper.SafeInvoke(() => Update(gameTime));
            orig(self, gameTime);
        };
    }
}