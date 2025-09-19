using SilkyUIFramework.Attributes;
using SilkyUIFramework.Extensions;
using SilkyUIFramework.Graphics2D;

namespace PointShop.UserInterfaces;

[RegisterUI(priority: 1)]
public partial class DonateUI : BaseBody
{
    public const string KoFi_Link = "https://ko-fi.com/sundev";
    public const string AFDian_Link = "https://afdian.com/a/tMLZero";

    protected override void OnInitialize()
    {
        Enabled = false;
        EnableBlur = true;
        BorderColor = SUIColor.Border;
        BackgroundColor = SUIColor.Background * 0.75f;
        OverflowHidden = true;
        IndependentRenderTarget = true;

        InitializeComponent();

        Header.Title.Text = "捐赠 Donate";
        Header.CloseButton.LeftMouseDown += delegate { Enabled = false; };

        Kofi.Icon.Texture2D = ModAsset.kofi;
        Kofi.Icon.ImageScale = new Vector2(0.25f);
        AFDian.Icon.Texture2D = ModAsset.afdian;
        AFDian.Icon.ImageScale = new Vector2(0.55f);

        Kofi.Name.Text = "Ko-Fi";
        AFDian.Name.Text = "爱发电";

        Kofi.LeftMouseDown += delegate { Utils.OpenToURL(KoFi_Link); };

        AFDian.LeftMouseDown += delegate { Utils.OpenToURL(AFDian_Link); };

        Header.ControlTarget = this;
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
    }

    protected override void DrawRenderTarget(SpriteBatch spriteBatch, RenderTarget2D renderTarget, Vector2 position)
    {
        var scale = Main.UIScale;
        spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        SDFRectangle.SampleVersion(renderTarget, position, renderTarget.SizeVec2,
            Vector2.Zero, Vector2.One, (BorderRadius - new Vector4(2)) * scale, Color.White, Matrix.Identity);
    }
}

[XmlElementMapping("LinkButton")]
public class UILinkButton : UIElementGroup
{
    public SUIImage Icon { get; }

    public UITextView Name { get; }

    public UILinkButton()
    {
        Icon = new SUIImage()
        {
            FitWidth = false,
            FitHeight = false,
            Width = new Dimension(32f),
            Height = new Dimension(32f),
            ImageAlign = new Vector2(0.5f),
        }.Join(this);

        Name = new UITextView()
        {
            FitHeight = false,
            Height = new Dimension(20f),
            TextScale = 0.4f,
        }.Join(this);
        Name.UseDeathText();

        MainAlignment = MainAlignment.Center;
        CrossAlignment = CrossAlignment.Center;

        FlexGrow = 1f;

        FitHeight = true;
        Padding = new Margin(0f, 28f);

        Border = 2f;
        BorderRadius = new Vector4(4f);

        FlexDirection = FlexDirection.Column;
        Gap = 8;
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        BorderColor = HoverTimer.Lerp(Color.Black * 0.5f, SUIColor.Highlight);
        //BackgroundColor = Color.Lerp(Color.Black * 0.25f, SUIColor.Highlight * 0.75f, HoverTimer.Schedule * 0.25f);
    }
}
