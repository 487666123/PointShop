using System.Linq;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.Extensions;
using Terraria.GameInput;

namespace PointShop.UserInterfaces.DisplayUI;

[RegisterUI("Vanilla: Radial Hotbars", "PointShop: PointShopUI")]
public class DisplayUI : BasicBody
{

    public Dictionary<GameEnvironment, SUIDisplayItem> DisplayItemTable = [];
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
            DisplayItemTable[environment] = displayItem;
            displayItem.Join(ScrollView.Container);
        }
    }

    public void UpdateList()
    {
        if (PointShopPlayer.Local is not { } player) return;

        ScrollView.Container.RemoveAllChildren();

        var list = DisplayItemTable.Keys.Where(name => player.CurrentEnvironments.Any(env => env.Name.Equals(name)));

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

        ScrollView.Recalculate();
    }

    public override void Update(GameTime gameTime)
    {
        UpdateList();
        if (ScrollView.IsMouseHovering)
            PlayerInput.LockVanillaMouseScroll("SilkyUIFramework");
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

        Icon = new SUIImage(environment.Icon)
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
        Icon.Texture2D = Environment.Icon;
        Points.Text = $"{Environment.GetPlayerPoints():#,##0}";
        base.Update(gameTime);
    }
}