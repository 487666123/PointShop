using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using PointShop.Entitys;
using PointShop.Interface.GUI;
using System.Collections.Generic;
using System.Text;
using Terraria.UI;

namespace PointShop.Common.Systems
{
    public class UISystem : ModSystem
    {
        public static List<TerrainData> TerrainDatas; // 积分商店得数据
        public static List<Asset<Texture2D>> icon; // 环境图标

        public static UserInterface PointInterface;
        public static PointGUI PointGUI;

        public static UserInterface PointShopInterface;
        public static PointShopGUI PointShopGUI;

        public override void Unload()
        {
            PointGUI = null;
            PointInterface = null;

            PointShopGUI = null;
            PointShopInterface = null;

            TerrainDatas = null;
            icon = null;
        }

        public override void Load()
        {
            TerrainDatas = JsonConvert.DeserializeObject<List<TerrainData>>(Encoding.UTF8.GetString(ModContent.GetFileBytes("PointShop/JSONs/ShopData.json")));

            icon = new();
            for (int i = 0; i < TerrainDatas.Count; i++)
            {
                icon.Add(ModContent.Request<Texture2D>($"PointShop/Images/Icons/{TerrainDatas[i].image}", AssetRequestMode.ImmediateLoad));
            }

            PointInterface = new();
            PointGUI = new();
            PointGUI.Activate();
            PointInterface.SetState(PointGUI);

            PointShopInterface = new();
            PointShopGUI = new();
            PointShopGUI.Activate();
            PointShopInterface.SetState(PointShopGUI);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (PointGUI.Visible)
            {
                PointInterface?.Update(gameTime);
            }
            if (PointShopGUI.Visible)
            {
                PointShopInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                   "PointShop: PointShopGUI",
                   delegate
                   {
                       if (PointShopGUI.Visible)
                       {
                           PointShopGUI?.Draw(Main.spriteBatch);
                       }
                       if (PointGUI.Visible)
                       {
                           PointGUI?.Draw(Main.spriteBatch);
                       }
                       return true;
                   },
                   InterfaceScaleType.UI));
            }
        }
    }
}
