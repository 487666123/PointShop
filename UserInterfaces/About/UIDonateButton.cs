using SilkyUIFramework.Attributes;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces.About;

/// <summary>
/// 捐赠按钮组件（图标 + 名称）。
/// 对应 XML 中的 <c>DonateButton</c> 标签。
/// </summary>
[XmlElementMapping("DonateButton")]
public class UIDonateButton : UIElementGroup
{
    /// <summary>
    /// 按钮左侧图标。
    /// </summary>
    public SUIImage Icon { get; }

    /// <summary>
    /// 按钮文字。
    /// </summary>
    public UITextView Name { get; }

    /// <summary>
    /// 创建基础按钮布局与默认视觉样式。
    /// </summary>
    public UIDonateButton()
    {
        MainAlignment = MainAlignment.Center;
        CrossAlignment = CrossAlignment.Center;

        BackgroundColor = Color.White;

        //FitHeight = true;
        Height = new Dimension(50f);
        FlexGrow = 1f;

        //SetGap(4f);
        SetPadding(8f);

        Border = 2f;
        BorderRadius = new Vector4(4f);

        FlexDirection = FlexDirection.Row;
        BorderColor = Color.Black * 0.5f;

        Icon = new SUIImage()
        {
            FitWidth = false,
            Width = new Dimension(40f),
            ImageAlign = new Vector2(0.5f),
        }.Join(this);

        Name = new UITextView()
        {
            TextScale = 0.4f,
            Padding = new Margin(2),
            TextAlign = new Vector2(0.5f),
            FlexGrow = 1,
            FlexShrink = 1,
        }.Join(this);
        Name.UseDeathText();
    }

    /// <summary>
    /// 根据 Hover 动画更新背景色。
    /// </summary>
    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
        BackgroundColor = Color.Black * HoverTimer.Lerp(0.2f, 0.3f);
    }
}
