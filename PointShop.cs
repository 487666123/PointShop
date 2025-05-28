using PointShop.Registrar;
using Terraria.WorldBuilding;

namespace PointShop;

public class PointShop : Mod
{
    private class DisplayNameUpdater : ModSystem
    {
        /// <summary>
        /// 修改 DisplayName
        /// </summary>
        public override void OnLocalizationsLoaded() =>
            Instance.DisplayName = LanguageHelper.GetTextByPointShop("DisplayName").Value;
    }

    public static PointShop Instance => ModContent.GetInstance<PointShop>();

    //public enum MessageType : byte
    //{
    //    EarnPoint
    //}

    //public override void HandlePacket(BinaryReader reader, int whoAmI)
    //{
    //    var msgType = (MessageType)reader.ReadByte();
    //    switch (msgType)
    //    {
    //        case MessageType.EarnPoint:
    //            // PointShopHelper.BonusPoints(reader.ReadByte());
    //            break;
    //        default:
    //            Logger.WarnFormat($"PointShop: Unknown Message type: {msgType}");
    //            break;
    //    }
    //}

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
                    RegisterGameEnvironment(args[1], args[2], args[3], args[4], args[5], args[6]);
                }
                break;
            }
            // 注册条件
            case nameof(RegisterCondition):
            {
                if (args.Length == 5)
                {
                    RegisterCondition(args[1], args[2], args[3], args[4]);
                }
                break;
            }
            // 注册商品
            case nameof(AddShopItem):
            {
                if (args.Length == 6)
                {
                    AddShopItem(args[1], args[2], args[3], args[4], args[5]);
                }
                break;
            }
            // 添加商品通过文件
            case nameof(AddShopItemByFile):
            {
                if (args.Length == 3)
                    AddShopItemByFile(args[1], args[2]);
                break;
            }
            // 获取解锁条件
            case nameof(GetAllUnlockConditionTable):
            {
                return GetAllUnlockConditionTable();
            }
            default:
            {
                Logger.WarnFormat($"PointShop: Unknown Call: {args[0]}");
                break;
            }
        }

        return null;
    }

    public static Dictionary<string, Func<bool>> GetAllUnlockConditionTable()
    {
        return PointShopSystem.GetAllUnlockConditionTable();
    }

    public static void RegisterGameEnvironment(object modObj, object iconObj, object nameObj,
        object conditionObj, object priorityObj, object colorObj)
    {
        if (modObj is not Mod mod || iconObj is not Asset<Texture2D> icon || nameObj is not string name ||
         conditionObj is not Func<Player, bool> condition || priorityObj is not int priority || colorObj is not Color color) return;

        PointShopSystem.RegisterGameEnvironment(mod, icon, name, condition, priority, color);
    }

    public static void RegisterCondition(object modObj, object nameObj, object iconObj, object conditionObj)
    {
        if (modObj is not Mod mod || nameObj is not string name ||
            iconObj is not Asset<Texture2D> icon || conditionObj is not Func<bool> condition) return;

        PointShopSystem.RegisterUnlockCondition(mod, name, icon, condition);
    }

    public static void AddShopItem(object modObj, object gameEnvironmentNameObj, object itemObj, object pricesObj, object unlockConditionsNameObj)
    {
        if (modObj is not Mod mod || pricesObj is not int prices || unlockConditionsNameObj is not string unlockConditionsName ||
            itemObj is not Item item || gameEnvironmentNameObj is not string gameEnvironmentName ||
            !PointShopSystem.TryGetGameEnvironment(gameEnvironmentName, out var gameEnvironment)) return;

        gameEnvironment.AddShopItem(new SimpleShopItem(mod, gameEnvironment, prices, unlockConditionsName, item));
    }

    public static void AddShopItemByFile(object modObj, object yamlStringObj)
    {
        if (modObj is not Mod mod || yamlStringObj is not string yamlString) return;

        ShopItemsRegistrar.RegisterShopData(mod, ShopItemsRegistrar.ConvertYamlStringToShopData(yamlString));
    }
}

