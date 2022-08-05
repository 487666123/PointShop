global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using ReLogic.Content;
global using System;
global using System.IO;
global using System.Linq;
global using Terraria;
global using Terraria.Audio;
global using Terraria.GameContent;
global using Terraria.GameContent.UI.Elements;
global using Terraria.ID;
global using Terraria.Localization;
global using Terraria.ModLoader;
global using Terraria.UI;
global using PointShop.Helpers;
global using static PointShop.Common.Data.TerrainInfo;
using PointShop.Common.GlobalNPCs;
using PointShop.Common.Players;
using static Terraria.ID.ContentSamples;

namespace PointShop
{
    public class PointShop : Mod
    {
        public enum MessageType : byte
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
                        CoinPlayer.BonusPoints(point);
                        // 积分提示
                        if (ModHelper.Config.TerrainCombat)
                        {
                            string text = $"{ModHelper.GetText("HuanJingName." + CoinPlayer.InWhatTerrain) + ModHelper.GetText("Hint.Point")} +{point}";
                            Color color = TerrainColor[CoinPlayer.InWhatTerrain2Int];
                            TipsHelper.NewTextDirect(Main.LocalPlayer.Center, new(0, -1), color, 90, text);
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