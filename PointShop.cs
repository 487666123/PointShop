using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using PointShop.Entitys;
using ReLogic.Content;
using System.Collections.Generic;
using System.Text;
using Terraria.ModLoader;

namespace PointShop
{
    partial class PointShop : Mod
    {
        public static List<TerrainData> TerrainDatas; // 积分商店得数据
        public static List<Asset<Texture2D>> icon; // 环境图标

        public override void Load()
        {
            TerrainDatas ??= JsonConvert.DeserializeObject<List<TerrainData>>(Encoding.UTF8.GetString(ModContent.GetFileBytes("PointShop/JSONs/ItemExchangeInfo.json")));

            icon ??= new();
            for (int i = 0; i < TerrainDatas.Count; i++)
            {
                icon.Add(ModContent.Request<Texture2D>($"PointShop/Images/Icons/{TerrainDatas[i].image}", AssetRequestMode.ImmediateLoad));
            }
        }

        public override void Unload()
        {
            // 我也不知道这有什么用，但是写上去也不影响我什么
            TerrainDatas = null;
            icon = null;
        }
    }
}