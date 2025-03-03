using SilkyUIFramework.Attributes;
using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.BasicElements;
using SilkyUIFramework.Extensions;
using Terraria.GameInput;

namespace PointShop.UserInterfaces.DisplayUI;

[AutoloadUI("Vanilla: Radial Hotbars", "PointShop: PointShopUI")]
public class DisplayUI : BasicBody
{
    public SUIScrollView ScrollView { get; private set; }
    public override void OnInitialize()
    {
        ScrollView = new SUIScrollView(Direction.Vertical)
        {
            Positioning = SilkyUIFramework.Core.Positioning.Absolute,
            VAlign = 1f,
            Gap = new Vector2(4f),
            Container = { Gap = new Vector2(4f) }
        }.Join(this);
        ScrollView.SetPadding(4f);
        ScrollView.SetSize(140, 150);

        var environments = PointShopSystem.Environments;

        foreach (var environment in environments)
        {
            var displayItem = new SUIDisplayItem(environment);
            displayItem.Join(ScrollView.Container);
            displayItem.UpdateData();

            DisplayItems[environment.Name] = displayItem;
        }
    }

    public Dictionary<string, SUIDisplayItem> DisplayItems = [];

    public void UpdateList()
    {
        if (PointShopPlayer.Local is not { } player) return;

        var list = DisplayItems.Keys.Where(name => player.CurrentEnvironments.Any(env => env.Name.Equals(name)));

        foreach (var (key, displayItem) in DisplayItems)
        {
            displayItem.UpdateData();
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (ScrollView.IsMouseHovering) PlayerInput.LockVanillaMouseScroll("SilkyUIFramework");

        base.Update(gameTime);
    }
}

public class SUIDisplayItem : View
{
    public readonly GameEnvironment Environment;

    public SUIImage Icon { get; private set; }
    public SUIText Points { get; private set; }

    public SUIDisplayItem(GameEnvironment environment)
    {
        SetSize(0f, 32f, 1f);
        Display = Display.Flexbox;
        Gap = new Vector2(4f);
        FlexWrap = false;
        Environment = environment;

        Icon = new SUIImage(environment.Icon.Value)
        {
            ImageScale = new Vector2(0.75f),
            ImageAlign = new Vector2(0.5f),
        }.Join(this);
        Icon.SetSize(32, 0f, 0f, 1f);

        Points = new SUIText
        {
            Text = $"{environment.GetPlayerPoints():#,##0}",
            TextScale = 0.75f,
            TextAlign = new Vector2(0f, 0.5f),
            FlexWeight = { Enable = true, Value = 1 }
        }.Join(this);
        Points.SetHeight(0f, 1f);
    }

    public override void Update(GameTime gameTime)
    {
        UpdateData();
        Recalculate();
        base.Update(gameTime);
    }

    public void UpdateData()
    {
        Icon.Texture = Environment.Icon.Value;
        Points.Text = $"{Environment.GetPlayerPoints():#,##0}";
    }
}