using PointShop.Common.Configs;
using PointShop.Common.GlobalNPCs;
using PointShop.Common.Players;
using System.IO;
using Terraria;
using static PointShop.UI.ShopState;
using static Terraria.ID.ContentSamples;

namespace PointShop
{
    partial class PointShop
    {
        internal enum MessageType : byte
        {
            EarnPoint
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            switch (msgType)
            {
                case MessageType.EarnPoint: // 处理积分数据
                    NPC npc = Main.npc[reader.ReadByte()];
                    SourceNPC sourceNPC = npc.GetGlobalNPC<SourceNPC>();
                    if (sourceNPC.HitByLocalPlayer)
                    {
                        int point = (byte)BestiaryHelper.GetBestiaryStarsPriority(npc);
                        CoinPlayer.LocalPlayerAdd(point);
                        // 积分提示
                        if (PointConfig.Get().CombatJiaFen)
                        {
                            CombatText.NewText(Main.LocalPlayer.getRect(), new(255, 255, 0),
                            $"{MyUtils.GetText("HuanJingName." + CoinPlayer.PlayerInWhere())}{MyUtils.GetText("Hint.Point")} +" +
                            $"{point}");
                        }
                    }
                    break;
                default:
                    Logger.WarnFormat($"PointShop: Unknown Message type: {msgType}");
                    break;
            }
        }
    }
}
