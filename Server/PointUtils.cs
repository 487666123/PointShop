using PointShop.Common.Configs;
using PointShop.Common.GlobalNPCs;
using PointShop.Common.Players;

namespace PointShop.Server;

public static class ServerSystem
{
    public static void EarnPoint(int npcIndex)
    {
        NPC npc = Main.npc[npcIndex];
        PointShopNPC shopNpc = npc.GetGlobalNPC<PointShopNPC>();

        if (!shopNpc.HitByLocalPlayer)
        {
            return;
        }

        int point = (byte)ContentSamples.BestiaryHelper.GetBestiaryStarsPriority(npc);
        CoinPlayer.BonusPoints(point);
        // 积分提示
        if (!UIConfig.Instance.TerrainCombat)
        {
            return;
        }

        string text =
            $"{MyUtils.GetText("TerrainName." + CoinPlayer.InWhatTerrain) + MyUtils.GetText("Hint.Point")} +{point}";
        Color color = TerrainColor[CoinPlayer.InWhatTerrain2Int];
        TipsHelper.NewText(Main.LocalPlayer.Center, new Vector2(0, -1), color, 90, text);
    }
}