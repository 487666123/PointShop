using System.Linq.Expressions;
using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Common.Tweening;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces.DisplayUI;

[RegisterUI]
public class PointsDisplayWidgetUI : BaseBody
{
    public static bool Display { get; set; }

    public Dictionary<GameEnvironment, SUIDisplayItem> DisplayItemTable = [];
    public UIElementGroup Title { get; private set; }
    public SUIScrollView ScrollView { get; private set; }

    public override bool Enabled => Display;

    protected override void OnInitialize()
    {
        EnableBlur = true;
        BorderRadius = new Vector4(4f, 4f, 4f, 4f);
        Border = 2f;
        BorderColor = SUIColor.Border * 0.75f;
        BackgroundColor = SUIColor.Background * 0.5f;
        CrossAlignment = CrossAlignment.Stretch;
        CrossContentAlignment = CrossContentAlignment.Stretch;

        FitWidth = true;
        FitHeight = true;

        SetLeft(0f, 0f, 0.5f);
        SetTop(0f, 0f, 1f);
        SetGap(4f);
        SetPadding(4f);

        Title = new UIElementGroup
        {
            BorderRadius = new Vector4(2f),
            MainAlignment = MainAlignment.Center,
            CrossAlignment = CrossAlignment.Center,
            CrossContentAlignment = CrossContentAlignment.Center,
            FitHeight = true,
        }.Join(this);
        Title.SetPadding(0f, 2f);
        Title.SetWidth(0f, 1f);

        Title.AddChild(new UITextView
        {
            Text = LanguageHelper.GetTextByPointShop("DisplayName").Value,
            TextScale = 0.75f,
            TextAlign = new Vector2(0f, 0.5f),
        });

        ScrollView = new SUIScrollView(Orientation.Vertical)
        {
            Gap = new Vector2(4f),
            Container = { Gap = new Vector2(4f) }
        }.Join(this);
        ScrollView.SetPadding(0f);
        ScrollView.SetWidth(245f, 0f);
        ScrollView.SetHeight(140f, 0f);

        var environments = PointShopSystem.Environments;

        foreach (var environment in environments)
        {
            var displayItem = new SUIDisplayItem(environment);
            displayItem.Join(ScrollView.Container);
            displayItem.LeftMouseDown += (_, _) =>
            {
                if (PointShopUI.CurrentEnvironmentName == environment.Name)
                {
                    PointShopUI.Toggle();
                    return;
                }

                if (UISceneManager.Instance.TryGetInstance<PointShopUI>(out var ui))
                {
                    ui.Open();
                }
                PointShopUI.CurrentEnvironmentName = environment.Name;
            };


            DisplayItemTable[environment] = displayItem;
        }

        Top = new(-10, -1f, 1f);

        if (ScrollView?.Container is { } container)
        {
            container.Top = new(50f, 0f, 0f);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Update();
    }

    private readonly List<GameEnvironment> _lastEnvironments = [];

    private void Update()
    {
        if (ScrollView is null) return;
        if (!Main.LocalPlayer.TryGetModPlayer<PointShopPlayer>(out var player)) return;

        // 只在变化时更新
        if (_lastEnvironments.SequenceEqual(player.CurrentEnvironments)) return;

        _lastEnvironments.Clear();
        _lastEnvironments.AddRange(player.CurrentEnvironments);

        ScrollView.Container.RemoveAllChildren();

        if (player.CurrentEnvironments.Count > 0)
        {
            foreach (var item in player.CurrentEnvironments)
            {
                if (DisplayItemTable.TryGetValue(item, out var uie))
                {
                    uie.Join(ScrollView.Container);
                }
            }
        }

        foreach (var (key, displayItem) in DisplayItemTable.Where(item => !player.CurrentEnvironments.Contains(item.Key)))
        {
            displayItem.Join(ScrollView.Container);
        }
    }

    // === 状态 ===
    bool _isOpenInventory;
    Tween _animTween;

    void OpenInventory()
    {
        _animTween?.Kill();
        _animTween = CreateTween().Parallel().SetTrans(TransitionType.Back).SetEase(EaseType.Out);

        _animTween.MemberTo(this, "Top", new Anchor(20, 0f, 0f), 0.2f);

        if (ScrollView?.Container is { } container)
        {
            _animTween.MemberTo(container, nameof(container.Top), new Anchor(0f, 0f, 0f), 0.2f);
        }
    }

    void CloseInventory()
    {
        _animTween?.Kill();
        _animTween = CreateTween().Parallel().SetTrans(TransitionType.Expo).SetEase(EaseType.Out);

        _animTween.MemberTo(this, "Top", new Anchor(-10, -1f, 1f), 0.2f);

        if (ScrollView?.Container is { } container)
        {
            _animTween.MemberTo(container, "Top", new Anchor(50f, 0f, 0f), 0.2f);
        }
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        if (Main.playerInventory)
        {
            if (_isOpenInventory) return; _isOpenInventory = true;
            OpenInventory();
        }
        else
        {
            if (!_isOpenInventory) return; _isOpenInventory = false;
            CloseInventory();
        }
    }
}
