using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Graphics2D;

namespace PointShop.UserInterfaces;

[RegisterUI("Vanilla: Radial Hotbars", "PointShop: PointShopUI")]
public partial class PointShopUI : BasicBody
{
    /// <summary>
    /// 显示 UI，状态控制
    /// </summary>
    public static bool ShowUI { get; set; }

    /// <summary>
    /// 是否启用，包括事件与绘制
    /// </summary>
    public override bool Enabled
    {
        get
        {
            if (ShowUI) return true;
            return !SwitchTimer.IsReverseCompleted;
        }
        set => ShowUI = value;
    }

    /// <summary>
    /// 商店 UI 中当前显示的环境商店内部名称
    /// </summary>
    public static string CurrentEnvironmentName
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            ShopItemTableIsDirty = true;
        }
    } = "Forest";

    /// <summary>
    /// 是否可交互（不影响绘制）
    /// </summary>
    public override bool IsInteractable => SwitchTimer.IsCompleted;

    protected override void OnInitialize()
    {
        BorderColor = SUIColor.Border;
        BackgroundColor = SUIColor.Background * 0.75f;

        InitializeComponent();

        Header.ControlTarget = this;
        Header.Title.Text = $"{LanguageHelper.GetTextByPointShop("DisplayName")}";

        MenuListScrollView.Mask.Border = 2;
        MenuListScrollView.Mask.BorderRadius = new Vector4(4);
        MenuListScrollView.Mask.BorderColor = Color.Black * 0.75f;
        MenuListScrollView.Container.HiddenBox = HiddenBox.Inner;
        MenuListScrollView.Container.Gap = Size.Zero;

        UpdateMenuList();

        SearchBar.Border = 2;
        SearchBar.BorderColor = SUIColor.Border * 0.75f;
        SearchBar.BackgroundColor = SUIColor.Background * 0.25f;

        SearchLeftText.Text = $"{LanguageHelper.GetTextByPointShop("NameFilter")}";
        SearchLeftText.BackgroundColor = SUIColor.Background * 0.5f;

        SearchBox.BackgroundColor = SUIColor.Border * 0.25f;
        SearchBox.CursorFlashColor = Color.White;
        SearchBox.ContentChanged += (sender, e) =>
        {
            _keywords = SearchBox.Text;
            ShopItemTableIsDirty = true;
        };

        ClearSearchButton.Text = $"{LanguageHelper.GetTextByPointShop("Clear")}";
        ClearSearchButton.LeftMouseDown += (_, _) => SearchBox.Text = string.Empty;

        ShopItemTableScrollView.Container.Gap = new Vector2(4);

        ShopItemTableIsDirty = true;
    }

    public readonly AnimationTimer SwitchTimer = new(3);

    protected override void UpdateStatus(GameTime gameTime)
    {
        if (ShowUI) SwitchTimer.StartUpdate();
        else SwitchTimer.StartReverseUpdate();

        SwitchTimer.Update(gameTime);

        if (ShopItemTableIsDirty)
        {
            UpdateShopItemTable();
            ShopItemTableIsDirty = false;
        }

        UseRenderTarget = SwitchTimer.IsUpdating;
        Opacity = SwitchTimer.Lerp(0f, 1f);

        var center = Bounds.Center * Main.UIScale;
        RenderTargetMatrix =
            Matrix.CreateTranslation(-center.X, -center.Y, 0) *
            Matrix.CreateScale(SwitchTimer.Lerp(0.95f, 1f), SwitchTimer.Lerp(0.95f, 1f), 1) *
            Matrix.CreateTranslation(center.X, center.Y, 0);

        base.UpdateStatus(gameTime);
    }

    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (BlurMakeSystem.BlurAvailable)
        {
            if (BlurMakeSystem.SingleBlur)
            {
                var batch = Main.spriteBatch;
                batch.End();
                BlurMakeSystem.KawaseBlur();
                batch.Begin(SpriteSortMode.Deferred, null, null, null, SilkyUI.RasterizerStateForOverflowHidden, null,
                    SilkyUI.TransformMatrix);
            }

            SDFRectangle.SampleVersion(BlurMakeSystem.BlurRenderTarget,
                Bounds.Position * Main.UIScale, Bounds.Size * Main.UIScale, BorderRadius * Main.UIScale,
                Matrix.Identity);
        }

        base.Draw(gameTime, spriteBatch);
    }
}