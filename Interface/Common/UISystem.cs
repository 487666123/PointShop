using Newtonsoft.Json;
using PointShop.Entitys;
using PointShop.UI.ShopUI;
using PointShop.UI.TipUI;
using System.Collections.Generic;
using System.Text;

namespace PointShop.UI.Common
{
    public class UISystem : ModSystem
    {
        public static List<TerrainData> TerrainDatas { get; set; } // 积分商店得数据
        public static List<Asset<Texture2D>> Icons { get; set; } // 环境图标

        public static UserInterface PointInterface { get; set; }
        public static PointGUI PointGUI { get; set; }

        public static UserInterface PointShopInterface { get; set; }
        public static PointShopGUI PointShopGUI { get; set; }

        public override void Unload()
        {
            PointGUI = null;
            PointInterface = null;

            PointShopGUI = null;
            PointShopInterface = null;

            TerrainDatas = null;
            Icons = null;
        }

        public override void Load()
        {
            TerrainDatas = JsonConvert.DeserializeObject<List<TerrainData>>(Encoding.UTF8.GetString(ModContent.GetFileBytes("PointShop/JSONs/ShopData.json")));

            Icons = new();
            for (int i = 0; i < TerrainDatas.Count; i++)
            {
                Icons.Add(ModContent.Request<Texture2D>($"PointShop/Images/Icons/{TerrainDatas[i].image}", AssetRequestMode.ImmediateLoad));
            }

            if (!Main.dedServ)
            {
                PointInterface = new();
                PointShopInterface = new();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (PointGUI.Visible)
                PointInterface?.Update(gameTime);

            if (PointShopGUI.Visible)
                PointShopInterface?.Update(gameTime);
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
