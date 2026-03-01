using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;

namespace PointShop.UserInterfaces;

[RegisterUI]
public partial class PointShopUI : BaseBody
{
    public static bool IsShow { get; set; }

    /// <summary>
    /// 是否启用，包括事件与绘制
    /// </summary>
    public override bool Enabled
    {
        get
        {
            if (IsShow) return true;
            return !SwitchTimer.IsReverseCompleted;
        }
        set => IsShow = value;
    }

    /// <summary>
    /// 商店 UI 中当前显示的环境商店内部名称
    /// </summary>
    public static string CurrentEnvironmentName
    {
        get; set
        {
            if (field == value) return;
            field = value;
            ShopItemTableIsDirty = true;
        }
    } = "Forest";

    /// <summary> 是否可交互 (不影响绘制) </summary>
    public override bool IsInteractable => SwitchTimer.IsCompleted;

    protected override void OnInitialize()
    {
        EnableBlur = true;
        BorderColor = SUIColor.Border;
        BackgroundColor = SUIColor.Background * 0.75f;
        OverflowHidden = true;
        IndependentRenderTarget = true;

        InitializeComponent();

        Header.ControlTarget = this;
        Header.Title.Text = $"{LanguageHelper.GetTextByPointShop("DisplayName")}";

        Header.CloseButton.LeftMouseDown += delegate { IsShow = false; };

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

        SearchBox.Placeholder = LanguageHelper.GetTextByPointShop("ItemSearchTips").Value;
        SearchBox.BackgroundColor = SUIColor.Border * 0.25f;
        SearchBox.CursorFlashColor = Color.White;
        SearchBox.ContentChanged += (sender, e) =>
        {
            _keywords = SearchBox.Text;
            ShopItemTableIsDirty = true;
        };

        ClearSearchButton.Text = $"{LanguageHelper.GetTextByPointShop("Clear")}";
        ClearSearchButton.LeftMouseDown += (_, _) => SearchBox.Text = string.Empty;

        ShopItemTableScrollView.Container.SetGap(4);

        ShopItemTableIsDirty = true;
    }

    public readonly AnimationTimer SwitchTimer = new(3);

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);


        if (IsShow) SwitchTimer.StartUpdate();
        else SwitchTimer.StartReverseUpdate();
        SwitchTimer.Update(gameTime);
        UseRenderTarget = SwitchTimer.IsUpdating;

        UpdateShopItemTable();
    }

    protected override void DrawWithRenderTarget(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Opacity = SwitchTimer.Lerp(0f, 1f);
        var center = Bounds.Center * Main.UIScale;
        RenderTargetMatrix =
            Matrix.CreateTranslation(-center.X, -center.Y, 0) *
            Matrix.CreateScale(SwitchTimer.Lerp(0.95f, 1f), SwitchTimer.Lerp(0.95f, 1f), 1) *
            Matrix.CreateTranslation(center.X, center.Y, 0);

        base.DrawWithRenderTarget(gameTime, spriteBatch);
    }
}