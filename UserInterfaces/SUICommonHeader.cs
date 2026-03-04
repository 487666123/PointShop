using SilkyUIFramework.Attributes;

namespace PointShop.UserInterfaces;

[XmlElementMapping("CommonHeader")]
public partial class SUICommonHeader : SUIDraggableView
{
    public SUICommonHeader() : base()
    {
        InitializeComponent();
        Title.UseDeathText();
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        var close = CloseButton;
        var timer = close.HoverTimer;
        close.CrossBorderColor = timer.Lerp(SUIColor.Border * 0.75f, SUIColor.Highlight);
        close.CrossBackgroundColor = timer.Lerp(SUIColor.Warn * 0.75f, SUIColor.Warn);

        base.UpdateStatus(gameTime);
    }
}