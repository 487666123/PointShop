using PointShop.Common.Configs;
using PointShop.Common.Players;
using PointShop.Helpers.Extensions;
using PointShop.Server;
using Terraria;
using Terraria.DataStructures;
using static Terraria.ID.ContentSamples;

namespace PointShop.Common.GlobalNPCs
{
    public class PointShopNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override GlobalNPC Clone(NPC npc, NPC npcClone) => base.Clone(npc, npcClone);
        public bool HitByLocalPlayer;

        private enum SpawnType
        {
            NotSpawn,
            NatureSpawnNPC,
            Parent,
            EntitySourceIsNull,
            Unknown
        }

        private SpawnType SpawnMode = SpawnType.NotSpawn;

        private int EntitySoureType = -1;

        // 在NPC生成时候
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_SpawnNPC)
            {
                SpawnMode = SpawnType.NatureSpawnNPC;
            }
            else if (source is EntitySource_Parent)
            {
                SpawnMode = SpawnType.Parent;
                if ((source as EntitySource_Parent).Entity is NPC)
                {
                    EntitySoureType = ((source as EntitySource_Parent).Entity as NPC).type;
                }

                ;
            }
            else if (source is null)
            {
                SpawnMode = SpawnType.EntitySourceIsNull;
            }
            else
            {
                SpawnMode = SpawnType.Unknown;
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (npc.realLife == -1)
            {
                HitByLocalPlayer = true;
            }
            else
            {
                Main.npc[npc.realLife].GetGlobalNPC<PointShopNPC>().HitByLocalPlayer = true;
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (npc.realLife == -1)
            {
                HitByLocalPlayer = true;
            }
            else
            {
                Main.npc[npc.realLife].GetGlobalNPC<PointShopNPC>().HitByLocalPlayer = true;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (!CanEarnPoint(npc))
            {
                return;
            }

            switch (Main.netMode)
            {
                case NetmodeID.Server:
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)PointShop.MessageType.EarnPoint);
                    packet.Write((byte)npc.whoAmI);
                    packet.Send();
                    break;
                }
                case NetmodeID.SinglePlayer:
                {
                    int point = BestiaryHelper.GetBestiaryStarsPriority(npc);
                    CoinPlayer.BonusPoints(point);

                    if (UIConfig.Instance.TerrainCombat)
                    {
                        string terrainName = MyUtils.GetText("TerrainName." + CoinPlayer.InWhatTerrain) +
                                             MyUtils.GetText("Hint.Point");
                        Color color = TerrainColor[CoinPlayer.InWhatTerrain2Int];
                        Vector2 pos = Main.LocalPlayer.position;
                        pos.X += Main.LocalPlayer.Size.X / 2f;
                        PointUtils.PointTip(terrainName, point, pos, color, 90);
                    }

                    break;
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
            if (SpawnMode == SpawnType.NotSpawn)
            {
                if (npc.netID != 1)
                {
                    return true;
                }
            }

            if (SpawnMode == SpawnType.Parent)
            {
                if (EntitySoureType == 594)
                    return true;
            }

            return npc.damage > 0 && !npc.friendly && npc.lifeMax > 0 &&
                   (SpawnMode == SpawnType.NatureSpawnNPC || SpawnMode == SpawnType.EntitySourceIsNull);
        }

        /*private static readonly Color borderColor = new(255, 0, 0);
        public override void PostDraw(NPC npc, SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            float textW = FontAssets.MouseText.Value.MeasureString(SpawnMode.ToString()).X;
            Vector2 position = npc.position - Main.screenPosition;
            position.X += npc.width / 2 - textW / 2;
            position.Y -= 40f;
            MyUtils.DrawString(position, SpawnMode.ToString(), Color.White, TerrainColor[(int)CoinPlayer.PlayerInWhere()]);

            textW = FontAssets.MouseText.Value.MeasureString(npc.netID.ToString()).X;
            position = npc.position - Main.screenPosition;
            position.X += npc.width / 2 - textW / 2;
            position.Y -= 60f;
            MyUtils.DrawString(position, $"{npc.type}", Color.White, TerrainColor[(int)CoinPlayer.PlayerInWhere()]);

            textW = FontAssets.MouseText.Value.MeasureString($"{CanEarnPoint(npc)}").X;
            position = npc.position - Main.screenPosition;
            position.X += npc.width / 2 - textW / 2;
            position.Y -= 80f;
            MyUtils.DrawString(position, $"{CanEarnPoint(npc)}", Color.White, TerrainColor[(int)CoinPlayer.PlayerInWhere()]);
        }*/

        private static void DrawString(NPC npc, string text, float Y)
        {
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(text);
            Vector2 position = npc.position - Main.screenPosition;
            position.X += npc.width / 2 - textSize.X / 2;
            position.Y += Y - textSize.Y;
            MyUtils.DrawText(position, text, Color.White, TerrainColor[CoinPlayer.InWhatTerrain2Int]);
        }
    }
}