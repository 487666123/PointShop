namespace PointShop;

public static class PointPopupHelper
{
    /// <summary>
    /// 寻找第一个 <see cref="PointPopup"/>
    /// </summary>
    public static PointPopup FindFirstPointPopupByEnvironment(GameEnvironment gameEnvironment)
    {
        // 原版会复用, 要检测 .name 是否等于 DisplayName
        foreach (var popupText in PopupText.popupText)
        {
            if (popupText is PointPopup { active: true } pointPopup &&
                pointPopup.GameEnvironment == gameEnvironment &&
                pointPopup.name.StartsWith(gameEnvironment.DisplayName))
            {
                return pointPopup;
            }
        }

        return null;
    }

    /// <summary> 创建一个 Popup </summary>
    public static void Create(Vector2 center, GameEnvironment environment, double points, int duration)
    {
        if (FindFirstPointPopupByEnvironment(environment) is { } pointPopup)
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

        NewPointPopup(environment, points, request, center);
    }

    public static int NewPointPopup(GameEnvironment environment, double points, AdvancedPopupRequest request, Vector2 position)
    {
        if (!Main.showItemText || Main.netMode == NetmodeID.Server) return -1;

        var index = FindInactiveOrBottom();
        if (index >= 0)
        {
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(request.Text);

            // 找到的改为 PointPopup
            if (PopupText.popupText[index] is not PointPopup popup)
            {
                popup = new(environment, points);
                PopupText.popupText[index] = popup;
            }
            PopupText.ResetText(popup);
            popup.SetNameAndPoints(environment, points);
            popup.active = true;
            popup.position = position - textSize / 2f;
            popup.displayText = request.Text;
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
    public static int FindInactiveOrBottom()
    {
        var index = -1;
        for (var i = 0; i < PopupText.popupText.Length; i++)
        {
            if (PopupText.popupText[i] != null && PopupText.popupText[i].active) continue;
            index = i;
            break;
        }

        // 没找到就拿最靠下的 (为啥不是拿最靠上的？原版就这么写的不管了)
        if (index == -1)
        {
            double bottom = Main.bottomWorld;
            for (var i = 0; i < 20; i++)
            {
                if (bottom > PopupText.popupText[i].position.Y)
                {
                    index = i;
                    bottom = PopupText.popupText[i].position.Y;
                }
            }
        }

        return index;
    }
}

/// <summary>
/// TNND, 没有虚方法
/// </summary>
public class PointPopup(GameEnvironment gameEnvironment, double points) : PopupText
{
    public GameEnvironment GameEnvironment = gameEnvironment;
    public double Points = points;

    public void SetNameAndPoints(GameEnvironment gameEnvironment, double points)
    {
        GameEnvironment = gameEnvironment;
        Points = points;
    }
}