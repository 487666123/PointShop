using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PointShop.Helpers;
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
            /*int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Ruler"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "PointrShop: Test",
                    delegate
                    {
                        JuDrawTest(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.Game)
                );
            }*/
        }

        private static readonly Asset<Texture2D> ButtonClose = ModHelper.GetTexture("Button_Close");
        private static float rotation = 0;
        public static void JuDrawTest(SpriteBatch sb)
        {
            float scale = 1f;
            Vector2 center = ButtonClose.Size() / 2f;
            Vector2 position = Main.LocalPlayer.position - Main.screenPosition;
            position.X += Main.LocalPlayer.width / 2f;
            position.Y -= (ButtonClose.Size() * scale).Length();
            Vector2 velocity = Main.LocalPlayer.velocity;

            if (velocity.X >= 0)
            {
                for (int i = 9; i >= 0; i--)
                {
                    sb.Draw(ButtonClose.Value, position, null, Color.White * ((10f - i) / 10f),
                        rotation - i * velocity.Length() * (MathHelper.Pi / 640f), center, scale, 0, 0f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    sb.Draw(ButtonClose.Value, position, null, Color.White * ((10f - i) / 10f),
                        rotation - i * velocity.Length() * (MathHelper.Pi / 640f), center, scale, 0, 0f);
                }
            }

            // 旋转
            if (Main.LocalPlayer.velocity.X > 0)
                rotation += MathHelper.Pi / 320f * Main.LocalPlayer.velocity.Length();
            else if (Main.LocalPlayer.velocity.X < 0)
                rotation -= MathHelper.Pi / 320f * Main.LocalPlayer.velocity.Length();
            else
                rotation += Main.LocalPlayer.direction * MathHelper.Pi / 320f * velocity.Length();
        }
    }
}
