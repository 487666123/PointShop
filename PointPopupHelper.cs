namespace PointShop;

public static class PointPopupHelper
{
    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static PointPopup SearchEnvironment(string name)
    {
        foreach (var popupText in Main.popupText)
        {
            if (popupText is PointPopup { active: true } pointPopup && pointPopup.EnvironmentName.Equals(name))
            {
                return pointPopup;
            }
        }

        return null;
    }

    /// <summary>
    /// 创建一个 Popup
    /// </summary>
    public static void Create(Vector2 center, GameEnvironment environment, double points, int duration)
    {
        if (SearchEnvironment(environment.Name) is { } pointPopup)
        {
            points += pointPopup.Points;
            pointPopup.active = false;
        }

        var text = $"{environment.DisplayName} +{points:#,##0}";

        var request = new AdvancedPopupRequest
        {
            Text = text,
            Color = environment.UniqueColor,
            Velocity = { Y = -3 },
            DurationInFrames = duration,
        };

        NewPointText(environment, points, request, center);
    }

    public static int NewPointText(GameEnvironment environment, double points, AdvancedPopupRequest request, Vector2 position)
    {
        if (!Main.showItemText || Main.netMode == NetmodeID.Server) return -1;

        int index = SearchInactiveOrBottom();
        if (index >= 0)
        {
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(request.Text);

            // 找到的改为 PointPopup
            if (Main.popupText[index] is not PointPopup popup)
            {
                popup = new(environment.Name, points);
                Main.popupText[index] = popup;
            }
            PopupText.ResetText(popup);
            popup.SetNameAndPoints(environment.Name, points);
            popup.active = true;
            popup.position = position - textSize / 2f;
            popup.name = request.Text;
            popup.stack = 1L;
            popup.velocity = request.Velocity;
            popup.lifeTime = request.DurationInFrames;
            popup.context = PopupTextContext.Advanced;
            popup.freeAdvanced = true;
            popup.color = request.Color;
        }

        return index;
    }

    /// <summary>
    /// 找到不活跃的
    /// </summary>
    public static int SearchInactiveOrBottom()
    {
        int index = -1;
        for (int i = 0; i < Main.popupText.Length; i++)
        {
            if (Main.popupText[i] == null || !Main.popupText[i].active)
            {
                index = i;
                break;
            }
        }

        // 没找到就拿最靠下的 (为啥不是拿最靠上的，原版就这么写的不管了)
        if (index == -1)
        {
            double bottom = Main.bottomWorld;
            for (int i = 0; i < 20; i++)
            {
                if (bottom > Main.popupText[i].position.Y)
                {
                    index = i;
                    bottom = Main.popupText[i].position.Y;
                }
            }
        }

        return index;
    }

    // public static void PointTip(string terrainName, int point, Vector2 center, Color color,
    //     int duration)
    // {
    //     switch (UIConfig.Instance.PointTipMode)
    //     {
    //         case PointTipMod.NoDisplay:
    //             return;
    //         case PointTipMod.Stack:
    //         {
    //             var popupTexts = Main.popupText.Where(popupText => popupText is { active: true });
    //             foreach (var popupText in popupTexts)
    //             {
    //                 if (!popupText.name?.StartsWith(terrainName) ?? true)
    //                 {
    //                     continue;
    //                 }

    //                 point += (int)popupText.coinValue;
    //                 popupText.active = false;
    //             }

    //             break;
    //         }
    //         case PointTipMod.NoStack:
    //         case PointTipMod.WhatIsThis:
    //         default:
    //             break;
    //     }

    //     var text = $"{terrainName} +{point}";
    //     if (UIConfig.Instance.PointTipMode == PointTipMod.WhatIsThis)
    //     {
    //         var hintMerit = LanguageHelper.GetTextByPointShop("Hit.Merit").Value;
    //         text = $"{hintMerit} -{point * 100}";
    //         color = Color.Red;
    //     }

    //     var request = new AdvancedPopupRequest
    //     {
    //         Text = text,
    //         Color = color,
    //         Velocity = { Y = -3 },
    //         DurationInFrames = duration,
    //     };

    //     var index = PopupText.NewText(request, center);
    //     Main.popupText[index].coinValue = point;
    // }

    // public static void BonusPoints(int npcIndex)
    // {
    //     var npc = Main.npc[npcIndex];
    //     var shopNpc = npc.GetGlobalNPC<PointShopNPC>();
    //
    //     if (!shopNpc.HitByLocalPlayer)
    //     {
    //         return;
    //     }
    //
    //     var point = ContentSamples.BestiaryHelper.GetBestiaryStarsPriority(npc);
    //     CoinPlayer.BonusPoints(point);
    //
    //     if (!UIConfig.Instance.TerrainCombat)
    //     {
    //         return;
    //     }
    //
    //
    //     var terrainName = Language.GetText("Mods.PointShop.TerrainName." + CoinPlayer.InWhatTerrain).Value;
    //     var hintPoint = Language.GetText("Mods.PointShop.Hint.Point").Value;
    //     terrainName += hintPoint;
    //
    //     var color = TerrainColor[CoinPlayer.InWhatTerrain2Int];
    //     PointTip(terrainName, point, Main.LocalPlayer.Center, color, 90);
    // }
}

/// <summary>
/// TNND, 没有虚方法
/// </summary>
public class PointPopup(string environmentName, double points) : PopupText
{
    public string EnvironmentName = environmentName;
    public double Points = points;

    public void SetNameAndPoints(string environmentName, double points)
    {
        EnvironmentName = environmentName;
        Points = points;
    }
}