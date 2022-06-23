using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Text;
using System.Text.Json.Nodes;
using Terraria.ModLoader;

namespace PointShop
{
    // Mod Ö÷Àà
    partial class PointShop : Mod
    {
        public static readonly JsonArray ItemExchangeInfo = JsonNode.Parse(Encoding.UTF8.GetString(ModContent.GetFileBytes("PointShop/JSONs/ItemExchangeInfo.json"))).AsArray();
        public static Asset<Texture2D>[] IconTextures = new Asset<Texture2D>[ItemExchangeInfo.Count];
        public override void Load()
        {
            for (int i = 0; i < ItemExchangeInfo.Count; i++)
            {
                IconTextures[i] = ModContent.Request<Texture2D>(
                    $"PointShop/Images/Icons/{ItemExchangeInfo[i].AsObject()["image"]}", AssetRequestMode.ImmediateLoad);
            }
        }
    }
}