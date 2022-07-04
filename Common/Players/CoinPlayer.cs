using PointShop.Common.Systems;
using PointShop.Interface;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static PointShop.Interface.PointShopGUI;

namespace PointShop.Common.Players
{
    // 保存环境得分
    public class CoinPlayer : ModPlayer
    {
        // 环境得分
        public int[] Point = new int[(int)PointShopGUI.Terrain.Count];

        public static int GetPoint(int huanJing)
        {
            return GetPoint((Terrain)huanJing);
        }

        public static int GetPoint(Terrain huanJing)
        {
            return Main.LocalPlayer.GetModPlayer<CoinPlayer>().Point[(int)huanJing];
        }

        public static void LocalPlayerAdd(int point)
        {
            Main.LocalPlayer.GetModPlayer<CoinPlayer>().Point[(int)PlayerInWhere()] += point;
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("HuanJingFen", Point);
        }

        public override void LoadData(TagCompound tag)
        {
            Point = tag.Get<int[]>("HuanJingFen");
            if (Point.Length < (int)PointShopGUI.Terrain.Count)
            {
                Point = new int[(int)PointShopGUI.Terrain.Count];
            }
        }

        public static Terrain PlayerInWhere()
        {
            Player player = Main.LocalPlayer;
            Terrain huanJing;
            if (player.ZoneDungeon) // 地牢
            {
                huanJing = Terrain.DiLao;
            }
            else if (player.ZoneSkyHeight) // 天空
            {
                huanJing = Terrain.TianKong;
            }
            else if (player.ZoneUnderworldHeight) // 地狱
            {
                huanJing = Terrain.DiYu;
            }
            else if (player.ZoneSnow) // 雪地
            {
                huanJing = Terrain.XueDi;
            }
            else if (player.ZoneDesert) // 沙漠
            {
                huanJing = Terrain.ShaMo;
            }
            else if (player.ZoneJungle) // 丛林
            {
                huanJing = Terrain.CongLin;
            }
            else if (player.ZoneBeach) // 海洋
            {
                huanJing = Terrain.HaiYang;
            }
            else if (player.ZoneHallow) // 神圣
            {
                huanJing = Terrain.ShenSheng;
            }
            else if (player.ZoneCorrupt) // 腐化
            {
                huanJing = Terrain.FuHua;
            }
            else if (player.ZoneCrimson) // 猩红
            {
                huanJing = Terrain.XingHong;
            }
            else if (player.ZoneRockLayerHeight && (player.ZoneNormalUnderground || player.ZoneNormalCaverns ||
                player.ZoneMarble || player.ZoneGranite || player.ZoneGlowshroom)) // 洞穴
            {
                huanJing = Terrain.DongXue;
            }
            // 森林
            else
            {
                huanJing = Terrain.SenLin;
            }
            return huanJing;
        }
    }
}
