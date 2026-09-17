using SilkyUIFramework.Common.Tweening;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces.DisplayUI;

public partial class SUIDisplayItem : UIElementGroup
{
    public readonly GameEnvironment GameEnvironment;

    public SUIDisplayItem(GameEnvironment environment)
    {
        InitializeComponent();

        GameEnvironment = environment;

        Icon.Texture2D = environment.Icon;

        ToStyle(Color.Black * 0.25f, Color.Black * 0.5f);
    }

    protected override void Update(GameTime gameTime)
    {
        PointsText.Text = $"{GameEnvironment.GetPlayerPoints():#,##0}";
        base.Update(gameTime);
    }

    public override void OnMouseEnter(UIMouseEvent evt)
    {
        base.OnMouseEnter(evt);
        ToStyle(Color.Black * 0.375f, Color.Black * 0.75f);
    }

    public override void OnMouseLeave(UIMouseEvent evt)
    {
        base.OnMouseLeave(evt);
        ToStyle(Color.Black * 0.25f, Color.Black * 0.5f);
    }

    private Tween Tween { get; set; }

    public void ToStyle(Color background, Color borderColor, bool animation = true)
    {
        Tween?.Kill();

        if (!animation)
        {
            BackgroundColor = background;
            BorderColor = borderColor;
            return;
        }

        var tween = Tween = CreateTween().Parallel().SetEase(EaseType.Out).SetTrans(TransitionType.Expo);
        tween.BgColorTo(this, background, 0.2f);
        tween.BorderColorTo(this, borderColor, 0.2f);
    }
}