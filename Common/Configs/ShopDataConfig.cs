using Terraria.ModLoader.Config;

namespace PointShop.Common.Configs
{
    public class ShopDataConfig : ModSystem
    {
        public const string FileName = "PointShopData.json";
        public static readonly string FullPath = Path.Combine(ConfigManager.ModConfigPath, FileName);

        public override void Load()
        {

        }
    }
}
