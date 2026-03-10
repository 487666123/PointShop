using PointShop.ShopData;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Extensions;
using SilkyUIFramework.Graphics2D;

namespace PointShop.UserInterfaces.About;

/// <summary>
/// 捐赠与关于面板。
/// 展示作者支持入口（Ko-Fi、爱发电）以及捐赠者列表区域。
/// </summary>
[RegisterUI(priority: 1)]
public partial class DonateUI : BaseBody
{
    /// <summary>
    /// Ko-Fi 支持链接。
    /// </summary>
    public const string KoFi_Link = "https://ko-fi.com/sundev";

    /// <summary>
    /// 爱发电支持链接。
    /// </summary>
    public const string Aifadian_Link = "https://afdian.com/a/tMLZero";

    public override IEnumerable<UIView> BlurElements => [MainPanel, RightPanel];

    public override bool ContainsPoint(Vector2 point) => MainPanel.ContainsPoint(point) || RightPanel.ContainsPoint(point);

    /// <summary>
    /// 初始化 Donate UI 的视觉样式、文案和交互行为。
    /// </summary>
    protected override void OnInitialize()
    {
        InitializeComponent();

        // 默认关闭，仅在 Footer 的 About 按钮触发时打开。
        Enabled = false;
        EnableBlur = true;
        OverflowHidden = false;

        MainPanel.BorderColor = SUIColor.Border;
        MainPanel.BackgroundColor = SUIColor.Background * 0.75f;

        RightPanel.BorderColor = SUIColor.Border;
        RightPanel.BackgroundColor = SUIColor.Background * 0.75f;

        // 允许通过标题栏拖拽窗口。
        Header.ControlTarget = this;

        Header.Title.Text = $"{LanguageHelper.GetTextByPointShop("About")}";
        Header.CloseButton.LeftMouseDown += delegate { Enabled = false; };

        QQFankui.Icon.ImageScale = new Vector2(0.8f);
        QQLianji.Icon.ImageScale = new Vector2(0.8f);
        QQFankui.Icon.Texture2D = ModAsset.QQ;
        QQLianji.Icon.Texture2D = ModAsset.QQ;
        QQFankui.Name.Text = "交流反馈";
        QQLianji.Name.Text = "联机交流";

        QQFankui.LeftMouseDown += delegate { Utils.OpenToURL("https://qm.qq.com/q/Vz7DmEOaQe"); };
        QQLianji.LeftMouseDown += delegate { Utils.OpenToURL("https://qm.qq.com/q/CQsK9QEW78"); };

        Kofi.Icon.Texture2D = ModAsset.kofi;
        AFDian.Icon.Texture2D = ModAsset.afdian;

        Kofi.Name.Text = "Ko-Fi";
        AFDian.Name.Text = "爱发电";

        // 点击后直接打开外部赞助页面。
        Kofi.LeftMouseDown += delegate { Utils.OpenToURL(KoFi_Link); };
        AFDian.LeftMouseDown += delegate { Utils.OpenToURL(Aifadian_Link); };

        InitializeDonors();

        Support.Text = LanguageHelper.GetTextByPointShop("SupportTheAuthor").Value;
        DonorList.Text = LanguageHelper.GetTextByPointShop("DonorList").Value;
        Acknowledgments.Text = LanguageHelper.GetTextByPointShop("Acknowledgments").Value;

        if (bool.TryParse(LanguageHelper.GetTextByPointShop("ShowGroupLinks").Value, out var showGroupLinks) && showGroupLinks) return;
        FankuiContainer.Invalid = true;
    }

    void InitializeDonors()
    {
        var dornorData = FileHelper.DeserializeYaml<DonorData>(FileHelper.GetString(FileHelper.DonorDataPath));

        foreach (var dornor in dornorData.Donors)
        {
            if (dornor.Effect.Equals("RainbowText"))
            {
                new SUIRainbowTextItem()
                {
                    NameView = { Text = dornor.Name, }
                }.Join(Donors.Container);

                continue;
            }
            else if (dornor.Effect.Equals("Windmill"))
            {
                new SUIWindmillItem()
                {
                    NameView = { Text = dornor.Name, }
                }.Join(Donors.Container);

                continue;
            }

            new SUIDonateItem()
            {
                NameView = { Text = dornor.Name, }
            }.Join(Donors.Container);
        }
    }

    /// <summary>
    /// 每帧更新样式状态。
    /// </summary>
    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        // 捐赠列表区域使用轻量透明底色，和主背景层次区分。
        Donors.BackgroundColor = Color.Transparent * 0.25f;
    }

    /// <summary>
    /// 使用 SDF 矩形绘制 RenderTarget，保证边框圆角在不同 UI 缩放下更平滑。
    /// </summary>
    protected override void DrawRenderTarget(SpriteBatch spriteBatch, RenderTarget2D renderTarget, Vector2 position)
    {
        var scale = Main.UIScale;
        spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        SDFRectangle.SampleVersion(renderTarget, position, renderTarget.SizeVec2,
            Vector2.Zero, Vector2.One, (BorderRadius - new Vector4(2)) * scale, Color.White, Matrix.Identity);
    }
}
