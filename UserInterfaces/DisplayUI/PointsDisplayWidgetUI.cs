using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
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

        ScrollView = new SUIScrollView(Direction.Vertical)
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
                    PointShopUI.IsShow = !PointShopUI.IsShow;
                    return;
                }

                PointShopUI.IsShow = true;
                PointShopUI.CurrentEnvironmentName = environment.Name;
            };


            DisplayItemTable[environment] = displayItem;
        }
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Update();
    }

    private void Update()
    {
        if (!Main.LocalPlayer.TryGetModPlayer<PointShopPlayer>(out var player)) return;
        if (ScrollView is null) return;

        ScrollView.Container.RemoveAllChildren();

        //var list = DisplayItemTable.Keys.Where(
        //    name => player.CurrentEnvironments.Any(env => env.Name.Equals(name)));

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

    protected override void UpdateStatus(GameTime gameTime)
    {
        if (Main.playerInventory) OpenInvTimer.StartUpdate();
        else OpenInvTimer.StartReverseUpdate();

        OpenInvTimer.Update(gameTime);
        base.UpdateStatus(gameTime);
        SetTop(OpenInvTimer.Lerp(-20f, 20f), OpenInvTimer.Lerp(-1f, 0f), OpenInvTimer.Lerp(1f, 0f));
    }

    public AnimationTimer OpenInvTimer = new(3);

    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }
}
