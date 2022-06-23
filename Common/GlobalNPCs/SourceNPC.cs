using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PointShop.Common.Configs;
using PointShop.Common.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static PointShop.Content.UI.MainUI;
using static Terraria.ID.ContentSamples;

namespace PointShop.Common.GlobalNPCs
{
    public class SourceNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override GlobalNPC Clone(NPC npc, NPC npcClone) => base.Clone(npc, npcClone);
        public bool HitByLocalPlayer = false;
        private enum SpawnType
        {
            Default,
            SpawnNPC,
            Parent,
            SourceNull,
            Unknown
        }

        private SpawnType SpawnMode = SpawnType.Default;
        // 在NPC生成时候
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_SpawnNPC)
            {
                SpawnMode = SpawnType.SpawnNPC;
            }
            else if (source is EntitySource_Parent)
            {
                SpawnMode = SpawnType.Parent;
            }
            else if (source is null)
            {
                SpawnMode = SpawnType.SourceNull;
            }
            else
            {
                SpawnMode = SpawnType.Unknown;
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            HitByLocalPlayer = true;
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            HitByLocalPlayer = true;
        }

        public override void OnKill(NPC npc)
        {
            if (CanEarnPoint(npc))
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)PointShop.MessageType.EarnPoint);
                    packet.Write((byte)npc.whoAmI);
                    packet.Send();
                }
                else if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    int point = (byte)BestiaryHelper.GetBestiaryStarsPriority(npc);
                    CoinPlayer.LocalPlayerAdd(point);
                    // 积分提示
                    if (PointConfig.Get().CombatJiaFen)
                    {
                        CombatText.NewText(Main.LocalPlayer.getRect(), new(255, 255, 0),
                        $"{MyUtils.GetText("HuanJingName." + CoinPlayer.PlayerInWhere()) + MyUtils.GetText("Hint.Point")} +{point}");
                    }
                }
            }
        }

        /// <summary>
        /// 判断能否获得积分
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        private bool CanEarnPoint(NPC npc)
        {
            if (npc.netID < 0)
                return true;
            return npc.damage > 0 && !npc.friendly && npc.lifeMax > 0 && npc.realLife == -1 &&
                (SpawnMode == SpawnType.SpawnNPC || SpawnMode == SpawnType.SourceNull);
        }

        /*public override void PostDraw(NPC npc, SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            float textW = FontAssets.MouseText.Value.MeasureString(SpawnMode.ToString()).X;
            Vector2 position = npc.position - Main.screenPosition;
            position.X += npc.width / 2 - textW / 2;
            position.Y -= 40f;
            Utils.DrawBorderString(sb, SpawnMode.ToString(), position, Color.White);

            textW = FontAssets.MouseText.Value.MeasureString(npc.netID.ToString()).X;
            position = npc.position - Main.screenPosition;
            position.X += npc.width / 2 - textW / 2;
            position.Y -= 60f;
            Utils.DrawBorderString(sb, npc.netID.ToString(), position, Color.White);
        }*/

        // 绿色史莱姆颜色
        // private readonly static Color GreenSlime = new(0, 220, 40, 100);
    }
}
