using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces.DisplayUI;

public class SUIDisplayItem : UIElementGroup
{
    public readonly GameEnvironment GameEnvironment;

    /// <summary>
    /// 环境图标
    /// </summary>
    public SUIImage Icon { get; private set; }

    /// <summary>
    /// 环境积分
    /// </summary>
    public UITextView PointsText { get; private set; }

    public SUIDisplayItem(GameEnvironment environment)
    {
        BorderRadius = new Vector4(2f);

        FlexGrow = 1f;
        SetWidth(80f, 0f);
        SetHeight(32f, 0f);
        Gap = new Vector2(4f);
        GameEnvironment = environment;
        CrossAlignment = CrossAlignment.Center;
        CrossContentAlignment = CrossContentAlignment.Center;

        Icon = new SUIImage(environment.Icon)
        {
            FitWidth = false,
            FitHeight = false,
            ImageScale = new Vector2(0.75f),
            ImageAlign = new Vector2(0.5f),
        }.Join(this);
        Icon.SetSize(32, 0f, 0f, 1f);

        PointsText = new UITextView
        {
            Text = $"{environment.GetPlayerPoints():#,##0}",
            TextScale = 0.75f,
            FlexGrow = 1f,
            FitHeight = false,
            TextAlign = new Vector2(0f, 0.5f),
        }.Join(this);
        PointsText.SetHeight(0f, 1f);
    }

    protected override void Update(GameTime gameTime)
    {
        Border = 2f;
        BorderColor = Color.Black * HoverTimer.Lerp(0.25f, 0.4f);
        BackgroundColor = Color.Black * HoverTimer.Lerp(0.25f, 0.4f);

        PointsText.Text = $"{GameEnvironment.GetPlayerPoints():#,##0}";

        base.Update(gameTime);
    }
}