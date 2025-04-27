using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

public class SUICommonHeader : SUIDraggableView
{
    public SUICross SUICross { get; }
    public SUICommonHeader(UIElementGroup group, string name) : base(group)
    {
        LayoutType = LayoutType.Flexbox;
        FlexDirection = FlexDirection.Row;
        MainAlignment = MainAlignment.SpaceBetween;
        CrossAlignment = CrossAlignment.Center;
        FlexWrap = false;
        BackgroundColor = Color.Black * 0.25f;
        BorderRadius = new Vector4(6f, 6f, 0f, 0f);
        SetSize(0f, 40f, 1f);

        var titleText = new UITextView
        {
            Text = name,
            TextScale = 0.45f,
            TextAlign = new Vector2(0f, 0.5f),
        }.Join(this);
        titleText.SetSize(0f, 0f, 0.25f, 1f);
        titleText.UseDeathText();
        titleText.SetPadding(12f, 0f);

        SUICross = new SUICross(SUIColor.Warn * 0.75f, SUIColor.Border * 0.75f)
        {
            CrossSize = 22f,
            CrossRounded = 3.5f,
            CrossBorderHoverColor = SUIColor.Highlight,
            CrossBackgroundHoverColor = SUIColor.Warn,
            BoxSizing = BoxSizing.Content,
        }.Join(this);
        SUICross.SetSize(24, 0, 0f, 1f);
        SUICross.SetPadding(12f, 0f);
        SUICross.LeftMouseDown += delegate { PointShopUI.ShowUI = false; };
    }
}
