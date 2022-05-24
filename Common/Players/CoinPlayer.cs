using PointShop.Common.GlobalNPCs;
using PointShop.Common.Systems;
using PointShop.Content.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static PointShop.Content.UI.CoinUI;
using static Terraria.ID.ContentSamples;

namespace PointShop.Common.Players
{
    // 保存环境得分
    public class CoinPlayer : ModPlayer
    {
        // 环境得分
        public int[] HuanJingFen = new int[(int)CoinUI.HuanJing.Count];

        public override void SaveData(TagCompound tag)
        {
            tag.Add("HuanJingFen", HuanJingFen);
        }

        public override void LoadData(TagCompound tag)
        {
            HuanJingFen = tag.Get<int[]>("HuanJingFen");
            if (HuanJingFen.Length < (int)CoinUI.HuanJing.Count)
            {
                HuanJingFen = new int[(int)CoinUI.HuanJing.Count];
            }
        }

        public override void OnEnterWorld(Player player)
        {
            CoinModSystem.switchUI.Activate();
            CoinModSystem.switchUserInterface.SetState(CoinModSystem.switchUI);
            CoinModSystem.coinUI.Activate();
            CoinModSystem.coinUserInterface.SetState(CoinModSystem.coinUI);
        }

        public static HuanJing PlayerInHuanJing(Player player)
        {
            HuanJing huanJing;
            if (player.ZoneDungeon) // 地牢
            {
                huanJing = HuanJing.DiLao;
            }
            else if (player.ZoneSkyHeight) // 天空
            {
                huanJing = HuanJing.TianKong;
            }
            else if (player.ZoneUnderworldHeight) // 地狱
            {
                huanJing = HuanJing.DiYu;
            }
            else if (player.ZoneSnow) // 雪地
            {
                huanJing = HuanJing.XueDi;
            }
            else if (player.ZoneDesert) // 沙漠
            {
                huanJing = HuanJing.ShaMo;
            }
            else if (player.ZoneJungle) // 丛林
            {
                huanJing = HuanJing.CongLin;
            }
            else if (player.ZoneBeach) // 海洋
            {
                huanJing = HuanJing.HaiYang;
            }
            else if (player.ZoneHallow) // 神圣
            {
                huanJing = HuanJing.ShenSheng;
            }
            else if (player.ZoneCorrupt) // 腐化
            {
                huanJing = HuanJing.FuHua;
            }
            else if (player.ZoneCrimson) // 猩红
            {
                huanJing = HuanJing.XingHong;
            }
            else if (player.ZoneRockLayerHeight && (player.ZoneNormalUnderground || player.ZoneNormalCaverns ||
                player.ZoneMarble || player.ZoneGranite || player.ZoneGlowshroom)) // 洞穴
            {
                huanJing = HuanJing.DongXue;
            }
            // 森林
            else
            {
                huanJing = HuanJing.SenLin;
            }
            return huanJing;
        }
    }
}
