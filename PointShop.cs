global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using PointShop.Helpers;
global using PointShop.VertexTypes;
global using ReLogic.Content;
global using System;
global using System.IO;
global using System.Linq;
global using Terraria;
global using Terraria.Audio;
global using Terraria.GameContent;
global using Terraria.ID;
global using Terraria.Localization;
global using Terraria.ModLoader;
global using Terraria.UI;
global using static PointShop.Common.Data.TerrainInfo;
using PointShop.Server;

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
                case MessageType.EarnPoint:
                    PointUtils.BonusPoints(reader.ReadByte());
                    break;
                default:
                    Logger.WarnFormat($"PointShop: Unknown Message type: {msgType}");
                    break;
            }
        }
    }
}