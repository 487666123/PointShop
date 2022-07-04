using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using PointShop.Common.Configs;
using PointShop.Interface;

namespace PointShop.Common.Systems
{
    public class InterfaceSystem : ModSystem
    {
        public static UserInterface PointInterface;
        public static PointGUI PointGUI;
        public static UserInterface PointShopInterface;
        public static PointShopGUI PointShopGUI;

        public override void Load()
        {
            PointGUI = new();
            PointInterface = new();
            PointShopGUI = new();
            PointShopInterface = new();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (PointConfig.Get().HuanJingFenPanel)
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
                   "Test : CoinUI",
                   delegate
                   {
                       if (PointShopGUI.Visible)
                       {
                           PointShopGUI.Draw(Main.spriteBatch);
                       }
                       if (PointConfig.Get().HuanJingFenPanel)
                       {
                           PointGUI.Draw(Main.spriteBatch);
                       }
                       return true;
                   },
                   InterfaceScaleType.UI));
            }
        }
    }
}
