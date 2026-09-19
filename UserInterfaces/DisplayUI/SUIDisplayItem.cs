using SilkyUIFramework.Common.Tweening;
using SilkyUIFramework.Extensions;
using SilkyUIFramework.StyleSystem;

namespace PointShop.UserInterfaces.DisplayUI;

public partial class SUIDisplayItem : UIElementGroup
{
    public readonly GameEnvironment GameEnvironment;

    public SUIDisplayItem(GameEnvironment environment)
    {
        InitializeComponent();

        GameEnvironment = environment;

        Icon.Texture2D = environment.Icon;

        StyleSheet.SetStyle(UIElementState.Normal, new StyleDefinition()
        {
            [nameof(BackgroundColor)] = Color.Black * 0.25f,
            [nameof(BorderColor)] = Color.Black * 0.5f,
        });

        StyleSheet.SetStyle(UIElementState.Hover, new StyleDefinition()
        {
            [nameof(BackgroundColor)] = Color.Black * 0.375f,
            [nameof(BorderColor)] = Color.Black * 0.75f,
        });
    }

    protected override void Update(GameTime gameTime)
    {
        PointsText.Text = $"{GameEnvironment.GetPlayerPoints():#,##0}";
        base.Update(gameTime);
    }
}