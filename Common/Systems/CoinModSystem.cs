using PointShop.Content.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using PointShop.Common.Configs;

namespace PointShop.Common.Systems
{
    public class CoinModSystem : ModSystem
    {
        public static SwitchUI switchUI;
        public static UserInterface switchUserInterface;
        public static MainUI coinUI;
        public static UserInterface coinUserInterface;

        public override void Load()
        {
            switchUI = new();
            switchUserInterface = new();
            coinUI = new();
            coinUserInterface = new();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (PointConfig.Get().HuanJingFenPanel)
            {
                switchUserInterface?.Update(gameTime);
            }
            if (MainUI.Visible)
            {
                coinUserInterface?.Update(gameTime);
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
                       if (MainUI.Visible)
                       {
                           coinUI.Draw(Main.spriteBatch);
                       }
                       if (PointConfig.Get().HuanJingFenPanel)
                       {
                           switchUI.Draw(Main.spriteBatch);
                       }
                       return true;
                   },
                   InterfaceScaleType.UI));
            }
        }
    }
}
