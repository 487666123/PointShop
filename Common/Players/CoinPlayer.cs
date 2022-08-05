using Terraria.ModLoader.IO;

namespace PointShop.Common.Players
{
    // 保存环境得分
    public class CoinPlayer : ModPlayer
    {
        public int[] Point = new int[(int)Terrain.Count];

        public static int GetPoints(Terrain huanJing) => GetPlayer.Point[(int)huanJing];

        /// <summary>
        /// 奖励积分
        /// </summary>
        /// <param name="point">为哪个环境奖励积分</param>
        public static void BonusPoints(int point) => GetPlayer.Point[InWhatTerrain2Int] += point;

        public static CoinPlayer GetPlayer => Main.LocalPlayer.GetModPlayer<CoinPlayer>();

        public static int InWhatTerrain2Int => (int)InWhatTerrain;

        public static Terrain InWhatTerrain
        {
            get
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

        public override void SaveData(TagCompound tag)
        {
            tag.Add("HuanJingFen", Point);
        }

        public override void LoadData(TagCompound tag)
        {
            if (!tag.TryGet("HuanJingFen", out Point))
                Point = new int[(int)Terrain.Count];
        }
    }
}
