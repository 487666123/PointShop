using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace PointShop.Common.Systems
{
    public class DrawSystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Ruler"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "PointrShop: Test",
                    delegate
                    {
                        DrawTest(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.Game)
                );
            }
        }

        private static readonly Asset<Texture2D> ButtonClose = MyUtils.GetTexture("Button_Close");
        private static float rot = 0;
        public static void DrawTest(SpriteBatch sb)
        {
            Vector2 Center = ButtonClose.Size() / 2f;
            for (int i = 0; i < 10; i++)
            {
                sb.Draw(ButtonClose.Value, Main.ScreenSize.ToVector2() / 2f + new Vector2(0, -ButtonClose.Size().Length() * 4f - 10), null, Color.White * ((10f - i) / 10f), rot - i * Main.LocalPlayer.velocity.Length() * (MathHelper.PiOver4 / 80f), Center, 4f, 0, 0f);
            }
            if (Main.LocalPlayer.velocity.X > 0)
                rot += MathHelper.PiOver4 / 40f * Main.LocalPlayer.velocity.Length();
            else if (Main.LocalPlayer.velocity.X < 0)
                rot -= MathHelper.PiOver4 / 40f * Main.LocalPlayer.velocity.Length();
            else
                rot += Main.LocalPlayer.direction * MathHelper.PiOver4 / 40f * Main.LocalPlayer.velocity.Length();
        }
    }
}
