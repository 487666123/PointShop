using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

public class SUIMenuComponent : UIElementGroup
{
    public GameEnvironment GameEnvironment { get; }

    public SUIImage Icon { get; private set; }
    public UITextView Text { get; private set; }

    public SUIMenuComponent(GameEnvironment environment)
    {
        GameEnvironment = environment;

        LayoutType = LayoutType.Flexbox;
        FlexDirection = FlexDirection.Row;
        MainAlignment = MainAlignment.Center;
        CrossAlignment = CrossAlignment.Center;
        CrossContentAlignment = CrossContentAlignment.Center;
        SetPadding(12f, 0f);
        SetSize(0f, 40f, 1f);

        Icon = new SUIImage(environment.Icon)
        {
            ImageScale = new Vector2(0.95f),
            ImageAlign = new Vector2(0.5f),
            FitWidth = false,
        }.Join(this);
        Icon.SetWidth(28);

        Text = new UITextView
        {
            Text = environment.DisplayName,
            TextScale = 0.85f,
            TextAlign = new Vector2(0.5f),
            FlexGrow = 1f,
        }.Join(this);
        Text.SetHeight(0f, 1f);
    }


    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
        BackgroundColor = Color.Black * HoverTimer.Lerp(0.25f, 0.4f);
    }
}