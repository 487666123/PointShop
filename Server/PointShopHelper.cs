using System.Collections.Generic;
using PointShop.Common.Configs;
using PointShop.Common.GlobalNPCs;
using PointShop.Common.Players;
using PointShop.Helpers.Extensions;

namespace PointShop.Server;

public static class PointUtils
{
    public static void BonusPoints(int npcIndex)
    {
        NPC npc = Main.npc[npcIndex];
        PointShopNPC shopNpc = npc.GetGlobalNPC<PointShopNPC>();

        if (!shopNpc.HitByLocalPlayer)
        {
            return;
        }

        int point = ContentSamples.BestiaryHelper.GetBestiaryStarsPriority(npc);
        CoinPlayer.BonusPoints(point);

        if (!UIConfig.Instance.TerrainCombat)
        {
            return;
        }

        string terrainName = MyUtils.GetText("TerrainName." + CoinPlayer.InWhatTerrain) +
                             MyUtils.GetText("Hint.Point");
        Color color = TerrainColor[CoinPlayer.InWhatTerrain2Int];
        PointTip(terrainName, point, Main.LocalPlayer.Center, color, 90);
    }

    public static void PointTip(string terrainName, int point, Vector2 center, Color color,
        int duration)
    {
        switch (UIConfig.Instance.PointTipMode)
        {
            case PointTipMod.NoDisplay:
                return;
            case PointTipMod.Stack:
            {
                IEnumerable<PopupText> popupTexts = Main.popupText.Where(popupText => popupText is { active: true });
                foreach (PopupText popupText in popupTexts)
                {
                    if (!popupText.name?.StartsWith(terrainName) ?? true)
                    {
                        continue;
                    }

                    point += (int)popupText.coinValue;
                    popupText.active = false;
                }

                break;
            }
        }

        string text = $"{terrainName} +{point}";
        if (UIConfig.Instance.PointTipMode == PointTipMod.WhatIsThis)
        {
            text = $"{MyUtils.GetText("Hint.Merit")} -{point * 100}";
            color = Color.Red;
        }

        var request = new AdvancedPopupRequest
        {
            Text = text,
            Color = color,
            Velocity = -3f.Y(),
            DurationInFrames = duration,
        };

        int index = PopupText.NewText(request, center);
        Main.popupText[index].coinValue = point;
    }
}