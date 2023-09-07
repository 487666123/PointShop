using Newtonsoft.Json;
using PointShop.Entitys;
using System.Collections.Generic;
using System.Text;
using PointShop.Interface.GUI;

namespace PointShop.Interface
{
    internal class UISystem : ModSystem
    {
        public static List<TerrainData> TerrainDatas;
        public static List<Texture2D> Icons;

        public static UserInterface PointInterface;
        public static TerrainGUI TerrainGUI;

        public static UserInterface PointShopInterface;
        public static ShopGUI PointShopGUI;

        public override void Unload()
        {
            TerrainGUI = null;
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
                Icons.Add(MyUtils.GetTexture($"Icons/{t.Image}").Value);
            }

            PointInterface = new UserInterface();
            PointShopInterface = new UserInterface();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (TerrainGUI.Visible)
                PointInterface?.Update(gameTime);

            if (ShopGUI.Visible)
                PointShopInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (index == -1)
            {
                return;
            }

            layers.Insert(index, new LegacyGameInterfaceLayer(
                "PointShop: PointShopGUI", () =>
                {
                    if (ShopGUI.Visible)
                        PointShopGUI?.Draw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));

            layers.Insert(index, new LegacyGameInterfaceLayer(
                "PointShop: PointGUI", () =>
                {
                    if (TerrainGUI.Visible)
                        TerrainGUI?.Draw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));
        }
    }
}