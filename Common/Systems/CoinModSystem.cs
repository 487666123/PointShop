using PointShop.Content.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace PointShop.Common.Systems
{
    public class CoinModSystem : ModSystem
    {
        public static SwitchUI switchUI;
        public static UserInterface switchUserInterface;
        public static CoinUI coinUI;
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
            if (MyUtils.GetConfig().HuanJingFenPanel)
            {
                switchUserInterface?.Update(gameTime);
            }
            if (CoinUI.Visible)
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
                       if (CoinUI.Visible)
                       {
                           coinUI.Draw(Main.spriteBatch);
                       }
                       if (MyUtils.GetConfig().HuanJingFenPanel)
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
