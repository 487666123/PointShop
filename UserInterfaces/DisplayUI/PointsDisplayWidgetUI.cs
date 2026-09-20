using SilkyUIFramework.Attributes;
using SilkyUIFramework.Common.Tweening;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces.DisplayUI;

[RegisterUI]
public partial class PointsDisplayWidgetUI : BaseBody
{
    public Dictionary<GameEnvironment, SUIDisplayItem> DisplayItemTable = [];

    protected override void OnInitialize()
    {
        InitializeComponent();

        BorderColor = SUIColor.Border * 0.75f;
        BackgroundColor = SUIColor.Background * 0.5f;

        TitleText.Text = PSHelper.GetTextByPointShop("DisplayName").Value;

        var content = ScrollView.Container;
        content.SetTemplateColumns([GridTrack.Fr(10f), GridTrack.Fr(10f)]);
        content.SetAutoRows([GridTrack.Pixels(32f)]);

        var environments = PointShopSystem.Environments;

        foreach (var env in environments)
        {
            var displayItem = new SUIDisplayItem(env);
            displayItem.Join(ScrollView.Container);
            displayItem.LeftMouseDown += (_, _) =>
            {
                if (PointShopUI.CurrentEnvironmentName == env.Name)
                {
                    PointShopUI.Toggle();
                    return;
                }

                if (UISceneManager.Instance.TryGetInstance<PointShopUI>(out var ui)) ui.Open();
                PointShopUI.CurrentEnvironmentName = env.Name;
            };


            DisplayItemTable[env] = displayItem;
        }
    }

    private readonly List<GameEnvironment> _lastEnvironments = [];

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (ScrollView is null) return;
        if (!Main.LocalPlayer.TryGetModPlayer(out PointShopPlayer player)) return;

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

    private Tween _tween;

    private bool IsOpenInventory
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            if (field) SetStyleByTween(new Anchor(20, 0f, 0f));
            else SetStyleByTween(new Anchor(-10, -1f, 1f));
        }
    }

    private void SetStyleByTween(Anchor target)
    {
        _tween?.Kill();
        _tween = CreateTween().Parallel().SetTrans(TransitionType.Back).SetEase(EaseType.Out);
        _tween.MemberTo(this, nameof(Top), target, 0.1f);
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
        IsOpenInventory = Main.playerInventory;
    }
}
