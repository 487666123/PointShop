using SilkyUIFramework.Extensions;
using SilkyUIFramework.StyleSystem;

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

        StyleSheet.SetStyle(UIElementState.Normal, new StyleDefinition()
        {
            [nameof(BackgroundColor)] = Color.Transparent
        });

        StyleSheet.SetStyle(UIElementState.Hover, new StyleDefinition()
        {
            [nameof(BackgroundColor)] = Color.Black * 0.25f
        });
    }

    protected override void OnEnterTree()
    {
        base.OnEnterTree();
        PointShopUI.EnvironmentNameChanged += OnEnvironmentChanged;
    }

    protected override void OnExitTree()
    {
        base.OnExitTree();
        PointShopUI.EnvironmentNameChanged -= OnEnvironmentChanged;
    }

    public void OnEnvironmentChanged(object _, string environmentName)
    {
        if (string.Equals(environmentName, GameEnvironment.Name))
            Text.TextColor = GameEnvironment.UniqueColor;
        else Text.TextColor = Color.White;
    }
}