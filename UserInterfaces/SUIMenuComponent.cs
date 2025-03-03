using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.BasicElements;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

public class SUIMenuComponent : View
{
    public GameEnvironment GameEnvironment { get; }

    public SUIImage Icon { get; private set; }
    public SUIText Text { get; private set; }

    public SUIMenuComponent(GameEnvironment environment)
    {
        GameEnvironment = environment;

        Display = Display.Flexbox;
        LayoutDirection = LayoutDirection.Row;
        MainAlignment = MainAlignment.Center;
        CrossAlignment = CrossAlignment.Center;
        PaddingLeft = 12f;
        PaddingRight = 12f;
        SetSize(0f, 40f, 1f);
        OnDraw += _ => { BgColor = Color.Black * HoverTimer.Lerp(0.25f, 0.4f); };

        Icon = new SUIImage(environment.Icon?.Value)
        {
            ImageScale = new Vector2(0.95f),
            ImageAlign = new Vector2(0.5f),
        }.Join(this);
        Icon.SetWidth(28);

        Text = new SUIText
        {
            DragIgnore = false,
            Text = environment.DisplayName,
            TextScale = 0.85f,
            TextAlign = new Vector2(0.5f),
            FlexWeight = { Enable = true, Value = 1f },
            SpecifyWidth = true,
        }.Join(this);
        Text.SetHeight(0f, 1f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}