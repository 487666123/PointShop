using LootCoins.Common.Players;
using LootCoins.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using static LootCoins.Content.UI.CoinUI;
using static Terraria.ID.ContentSamples;

namespace LootCoins.Common.GlobalNPCs
{
    public class SourceNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override GlobalNPC Clone(NPC npc, NPC npcClone)
        {
            return base.Clone(npc, npcClone);
        }

        /// <summary>
        /// 0 未知，1 自然生成，2 非自然生成
        /// </summary>
        public int SpawnNPCMode = 0;

        // 在NPC生成时候
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_SpawnNPC)
            {
                SpawnNPCMode = 1;
            }
            else
            {
                SpawnNPCMode = 2;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.lastInteraction > -1 && npc.lastInteraction < 255)
            {
                Player player = Main.player[npc.lastInteraction];
                AddHuanJingFen(player, npc);
            }
        }

        public void AddHuanJingFen(Player player, NPC npc)
        {
            HuanJing huanJing = CoinPlayer.PlayerInHuanJing(player);

            CoinPlayer coinPlayer = player.GetModPlayer<CoinPlayer>();

            if (npc.damage > 0 && !npc.friendly && npc.lifeMax > 0 && npc.realLife == -1 &&
                (SpawnNPCMode == 0 || SpawnNPCMode == 1))
            {
                // 积分提示
                if (MyUtils.GetConfig().CombatJiaFen)
                {
                    CombatText.NewText(player.getRect(), new(255, 255, 0),
                    $"{LootCoins.ItemExchangeInfo[((int)huanJing)].AsObject()["name"]}{Language.GetTextValue($"Mods.LootCoins.Hint.积分1")} +" +
                    $"{BestiaryHelper.GetBestiaryStarsPriority(npc)}");
                }

                coinPlayer.HuanJingFen[(int)huanJing] += BestiaryHelper.GetBestiaryStarsPriority(npc);
            }
        }
    }
}
