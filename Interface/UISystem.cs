using Newtonsoft.Json;
using PointShop.Entitys;
using System.Collections.Generic;
using System.Text;
using PointShop.Interface.ShopUI;
using PointShop.Interface.TipUI;

namespace PointShop.Interface
{
    public class UISystem : ModSystem
    {
        public static List<TerrainData> TerrainDatas { get; set; } // 积分商店得数据
        public static List<Texture2D> Icons { get; set; } // 环境图标

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
            TerrainDatas =
                JsonConvert.DeserializeObject<List<TerrainData>>(
                    Encoding.UTF8.GetString(ModContent.GetFileBytes("PointShop/JSONs/ShopData.json")));

            if (Main.dedServ)
                return;

            Icons = new List<Texture2D>();
            foreach (var t in TerrainDatas)
            {
                Icons.Add(MyUtils.GetTexture($"Icons/{t.image}").Value);
            }

            PointInterface = new UserInterface();
            PointShopInterface = new UserInterface();
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
                    "PointShop: PointShopGUI", () =>
                    {
                        if (PointShopGUI.Visible)
                            PointShopGUI?.Draw(Main.spriteBatch);
                        return true;
                    }, InterfaceScaleType.UI));

                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "PointShop: PointGUI", () =>
                    {
                        if (PointGUI.Visible)
                            PointGUI?.Draw(Main.spriteBatch);
                        return true;
                    }, InterfaceScaleType.UI));
            }
        }
    }
}