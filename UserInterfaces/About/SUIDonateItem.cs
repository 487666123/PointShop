using PointShop.UserInterfaces.Components;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces.About;

public class SUIDonateItem : UIElementGroup
{
    public UITextView NameView { get; protected set; }

    public SUIDonateItem()
    {
        FitHeight = true;
        SetPadding(6, 4);
        BorderRadius = new Vector4(4);
        BackgroundColor = Color.Black * 0.25f;

        NameView = new UITextView()
        {
            TextScale = 0.8f,
            Padding = new Margin(2),
            Text = "Defaule.Text"
        }.Join(this);
    }
}

/// <summary>
/// 彩虹文字
/// </summary>
public class SUIRainbowTextItem : SUIDonateItem
{
    private readonly RainbowTextEffect Rainbow = RainbowTextEffect.Default;

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        //Main.NewText($"123");

        var amount = gameTime.TotalGameTime.TotalSeconds % 2;
        NameView.TextColor = Rainbow.GetColorClamped((float)amount / 2);
    }
}

/// <summary>
/// 大风车
/// </summary>
public class SUIWindmillItem : SUIDonateItem
{
    public SUIWindmillItem() : base()
    {
        NameView.TextPercentOffset = new Vector2(0.5f);
        NameView.TextPercentOrigin = new Vector2(0.5f);
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        NameView.TextPercentOffset = new Vector2(0f, 0.5f);
        NameView.TextPercentOrigin = new Vector2(0f, 0.5f);

        NameView.TextRotation += MathF.PI * 0.114514f;
    }
}
