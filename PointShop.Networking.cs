using Microsoft.Xna.Framework;
using PointShop.Common.Configs;
using PointShop.Common.GlobalNPCs;
using PointShop.Common.Players;
using System.IO;
using Terraria;
using static PointShop.UI.PointShopGUI;
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
                    PointShopNPC sourceNPC = npc.GetGlobalNPC<PointShopNPC>();
                    if (sourceNPC.HitByLocalPlayer)
                    {
                        int point = (byte)BestiaryHelper.GetBestiaryStarsPriority(npc);
                        CoinPlayer.LocalPlayerAdd(point);
                        // 积分提示
                        if (PointConfig.Get().CombatJiaFen)
                        {
                            string text = $"{MyUtils.GetText("HuanJingName." + CoinPlayer.PlayerInWhere()) + MyUtils.GetText("Hint.Point")} +{point}";
                            AdvancedPopupRequest request = default;
                            request.Text = text;
                            request.DurationInFrames = 120;
                            request.Velocity = new(0, -2);
                            request.Color = TerrainColor[(int)CoinPlayer.PlayerInWhere()];
                            PopupText.NewText(request, Main.LocalPlayer.Top + new Vector2(0, -10));
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
