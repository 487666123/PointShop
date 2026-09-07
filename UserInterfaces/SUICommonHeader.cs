using SilkyUIFramework.Attributes;
using SilkyUIFramework.Common.Tweening;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

[XmlElementMapping("CommonHeader")]
public partial class SUICommonHeader : SUIDraggableView
{
    public SUICommonHeader() : base()
    {
        InitializeComponent();
        Title.UseDeathText();

        var close = CloseButton;
        close.CrossBorderColor = SUIColor.Border * 0.75f;
        close.CrossBackgroundColor = SUIColor.Warn * 0.75f;
        close.MouseEnter += (s, e) =>
        {
            var animTween = close.CreateTween().Parallel().SetTrans(TransitionType.Back).SetEase(EaseType.Out);
            animTween.MemberTo(close, "CrossBorderColor", SUIColor.Highlight, 0.2f);
            animTween.MemberTo(close, "CrossBackgroundColor", SUIColor.Warn, 0.2f);
        };
        close.MouseLeave += (s, e) =>
        {
            var animTween = close.CreateTween().Parallel().SetTrans(TransitionType.Back).SetEase(EaseType.Out);
            animTween.MemberTo(close, "CrossBorderColor", SUIColor.Border * 0.75f, 0.2f);
            animTween.MemberTo(close, "CrossBackgroundColor", SUIColor.Warn * 0.75f, 0.2f);
        };
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
    }
}