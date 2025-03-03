namespace PointShop;

public class PointShop : Mod
{
    public override string Name => "PointShop";

    public enum MessageType : byte
    {
        EarnPoint
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        var msgType = (MessageType)reader.ReadByte();
        switch (msgType)
        {
            case MessageType.EarnPoint:
                // PointShopHelper.BonusPoints(reader.ReadByte());
                break;
            default:
                Logger.WarnFormat($"PointShop: Unknown Message type: {msgType}");
                break;
        }
    }

    public override object Call(params object[] args)
    {
        if (args.Length == 0) return null;
        switch (args[0])
        {
            // 注册环境
            case nameof(RegisterGameEnvironment):
            {
                if (args.Length == 7)
                {
                    RegisterGameEnvironment(args[1], args[2], args[3], args[4], args[5], args[5]);
                }
                break;
            }
            // 注册条件
            case nameof(RegisterCondition):
            {
                if (args.Length == 3)
                {
                    RegisterCondition(args[1], args[2]);
                }
                break;
            }
            // 注册商品
            case nameof(AddShopItem):
            {
                if (args.Length == 4)
                {
                    AddShopItem(args[1], args[2], args[3]);
                }
                break;
            }
            default:
            {
                Logger.WarnFormat($"PointShop: Unknown Call: {args[0]}");
                break;
            }
        }

        return null;
    }

    public static void RegisterGameEnvironment(object modObj, object iconObj, object nameObj, object conditionObj, object priorityObj, object colorObj)
    {
        if (modObj is not Mod mod || iconObj is not Asset<Texture2D> icon || nameObj is not string name ||
         conditionObj is not Func<Player, bool> condition || priorityObj is not int priority || colorObj is not Color color) return;

        PointShopSystem.RegisterGameEnvironment(mod, icon, name, condition, priority, color);
    }

    public static void RegisterCondition(object nameObj, object conditionObj)
    {
        if (nameObj is not string name || conditionObj is not Func<bool> condition) return;

        // PointShopSystem.RegisterUnlockCondition(name, condition);
    }

    public static void AddShopItem(object pricesObj, object unlockConditionsNameObj, object itemObj)
    {
        if (pricesObj is not int prices || unlockConditionsNameObj is not string unlockConditionsName ||
            itemObj is not Item item ||
            !PointShopSystem.TryGetGameEnvironment(unlockConditionsName, out var gameEnvironment)) return;

        gameEnvironment.AddShopItem(new SimpleShopItem(gameEnvironment, prices, unlockConditionsName, item));
    }
}

