using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Text;
using System.Text.Json.Nodes;
using Terraria.ModLoader;

namespace PointShop
{
    // Mod 主类
    public class PointShop : Mod
    {
        public static JsonArray ItemExchangeInfo;
        public static Asset<Texture2D>[] IconTextures;
        public override void Load()
        {
            // 加载配置文件
            ItemExchangeInfo = JsonNode.Parse(Encoding.UTF8.GetString(ModContent.GetFileBytes("PointShop/JSONs/ItemExchangeInfo.json"))).AsArray();
            IconTextures = new Asset<Texture2D>[ItemExchangeInfo.Count];
            for (int i = 0; i < ItemExchangeInfo.Count; i++)
            {
                IconTextures[i] = ModContent.Request<Texture2D>(
                    $"PointShop/Images/Icons/{ItemExchangeInfo[i].AsObject()["image"]}", AssetRequestMode.ImmediateLoad);
            }
        }
    }
}