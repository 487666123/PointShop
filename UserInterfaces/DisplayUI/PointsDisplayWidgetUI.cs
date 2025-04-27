using SilkyUIFramework.Animation;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Extensions;
using SilkyUIFramework.Graphics2D;

namespace PointShop.UserInterfaces.DisplayUI;

[RegisterUI("Vanilla: Radial Hotbars", "PointShop: PointsDisplayWidget")]
public class PointsDisplayWidgetUI : BasicBody
{
    public static bool Display { get; set; }

    public Dictionary<GameEnvironment, SUIDisplayItem> DisplayItemTable = [];
    public UIElementGroup Title { get; private set; }
    public SUIScrollView ScrollView { get; private set; }

    public override bool Enabled => Display;

    protected override void OnInitialize()
    {
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

        Title.AppendChild(new UITextView
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
                    PointShopUI.ShowUI = !PointShopUI.ShowUI;
                    return;
                }

                PointShopUI.ShowUI = true;
                PointShopUI.CurrentEnvironmentName = environment.Name;
            };


            DisplayItemTable[environment] = displayItem;
        }
    }

    public void UpdateList()
    {
        if (PointShopPlayer.Local is not { } player) return;

        ScrollView.Container.RemoveAllChildren();

        var list = DisplayItemTable.Keys.Where(
            name => player.CurrentEnvironments.Any(env => env.Name.Equals(name)));

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

    protected override void Update(GameTime gameTime)
    {
        UpdateList();
        base.Update(gameTime);
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        if (Main.playerInventory)
        {
            OpenInvTimer.StartUpdate();
        }
        else
        {
            OpenInvTimer.StartReverseUpdate();
        }

        OpenInvTimer.Update(gameTime);
        base.UpdateStatus(gameTime);
        SetTop(OpenInvTimer.Lerp(-20f, 20f), OpenInvTimer.Lerp(-1f, 0f), OpenInvTimer.Lerp(1f, 0f));
    }

    public AnimationTimer OpenInvTimer = new(3);

    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (BlurMakeSystem.BlurAvailable)
        {
            if (BlurMakeSystem.SingleBlur)
            {
                var batch = Main.spriteBatch;
                batch.End();
                BlurMakeSystem.KawaseBlur();
                batch.Begin(SpriteSortMode.Deferred, null, null, null, SilkyUI.RasterizerStateForOverflowHidden, null, SilkyUI.TransformMatrix);
            }

            SDFRectangle.SampleVersion(BlurMakeSystem.BlurRenderTarget,
                Bounds.Position * Main.UIScale, Bounds.Size * Main.UIScale, BorderRadius * Main.UIScale, Matrix.Identity);
        }

        base.Draw(gameTime, spriteBatch);
    }
}
