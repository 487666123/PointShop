using PointShop.Common.Animations;
using PointShop.Common.Players;

namespace PointShop.Interface.UIElements;

public class ItemSlotSingle : View
{
    private static readonly Texture2D BossIcons1 = MyUtils.GetTexture("BossIcons/Map_Icon_Skeletron").Value;
    private static readonly Texture2D BossIcons2 = MyUtils.GetTexture("BossIcons/Map_Icon_Wall_of_Flesh").Value;
    private static readonly Texture2D BossIcons3 = MyUtils.GetTexture("BossIcons/Map_Icon_Skeletron_Prime").Value;
    private static readonly Texture2D BossIcons4 = MyUtils.GetTexture("BossIcons/Map_Icon_Plantera").Value;

    private readonly int _value;
    private readonly int _mode;
    private readonly Item _item;
    private readonly Terrain _terrain;

    private readonly Texture2D _lockTexture;
    private readonly Vector2 _lockTextureSize;

    private readonly Texture2D _bossTexture;
    private readonly Vector2 _bossTextureSize;

    private readonly AnimationTimer _hoverTimer;

    public ItemSlotSingle(Item item, int value, int mode, Terrain terrain)
    {
        SetSizePixels(52f, 52f);
        _hoverTimer = new AnimationTimer(3);

        _item = item;
        _value = value;
        _mode = mode;
        _terrain = terrain;

        _lockTexture = MyUtils.GetTexture("BossIcons/Lock").Value;
        _lockTextureSize = _lockTexture.Size();

        _bossTexture = GetBossIcon();
        _bossTextureSize = _bossTexture.Size();

        Border = 2f;
        Rounded = new Vector4(10f);
    }

    public override void Update(GameTime gameTime)
    {
        _hoverTimer.Update();
        base.Update(gameTime);
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);
        _hoverTimer.Open();
        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        base.MouseOut(evt);
        _hoverTimer.Close();
    }

    protected override void DrawSelf(SpriteBatch sb)
    {
        BorderColor = Color.Lerp(UIColor.ItemSlotBorder, UIColor.ItemSlotBorderFav, _hoverTimer.Schedule);
        BgColor = Color.Lerp(UIColor.ItemSlotBg, UIColor.ItemSlotBgFav, _hoverTimer.Schedule);

        base.DrawSelf(sb);

        if (IsMouseHovering && !_item.IsAir)
        {
            Main.hoverItemName = _item.Name;
            Main.HoverItem = _item.Clone();
        }

        Vector2 pos = GetDimensions().Position();
        Vector2 size = GetDimensions().Size();

        bool canBuyItem = CanBuyItem();
        DrawItem(sb, _item, canBuyItem ? Color.White : Color.White * 0.5f, GetInnerDimensions());

        if (canBuyItem)
        {
            return;
        }

        sb.Draw(_lockTexture, pos + size * 0.3f, null, Color.White, 0, _lockTextureSize / 2, 0.7f, 0, 0);
        sb.Draw(_bossTexture, pos + size * 0.6f, null, Color.White, 0, _bossTextureSize / 2, 0.65f, 0, 0);
    }

    private static void DrawItem(SpriteBatch sb, Item item, Color lightColor, CalculatedStyle dimensions,
        float itemSize = 32f)
    {
        Main.instance.LoadItem(item.type);
        Texture2D itemTexture2D = TextureAssets.Item[item.type].Value;

        Rectangle frame = Main.itemAnimations[item.type]?.GetFrame(itemTexture2D) ?? itemTexture2D.Frame();

        float size = frame.Width > itemSize || frame.Height > itemSize
            ? frame.Width > frame.Height ? itemSize / frame.Width : itemSize / frame.Height
            : 1f;

        sb.Draw(itemTexture2D, dimensions.Center() - frame.Size() * size / 2f,
            frame, item.GetAlpha(lightColor), 0f, Vector2.Zero, size,
            SpriteEffects.None, 0f);
        sb.Draw(itemTexture2D, dimensions.Center() - frame.Size() * size / 2f,
            frame, item.GetColor(lightColor), 0f, Vector2.Zero, size,
            SpriteEffects.None, 0f);
    }

    public void BuyItem()
    {
        if (!CanBuyItem())
        {
            Main.NewText(MyUtils.GetText("Hint.Locked"), Color.Red);
            return;
        }

        CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
        if (coinPlayer.Point[(int)_terrain] >= _value)
        {
            coinPlayer.Point[(int)_terrain] -= _value;
            Main.NewText($"\"{_item.Name}\" {MyUtils.GetText("Hint.Success")}", new Color(0x00, 0x99, 0xff));
            Main.LocalPlayer.QuickSpawnItem(null, _item.Clone());
        }
        else
        {
            Main.NewText($"\"{_item.Name}\" {MyUtils.GetText("Hint.NotPoint")}", Color.Red);
        }
    }

    private bool CanBuyItem()
    {
        return _mode switch
        {
            1 => NPC.downedBoss3,
            2 => Main.hardMode,
            3 => NPC.downedMechBossAny,
            4 => NPC.downedPlantBoss,
            _ => true
        };
    }

    private Texture2D GetBossIcon()
    {
        return _mode switch
        {
            2 => BossIcons2,
            3 => BossIcons3,
            4 => BossIcons4,
            _ => BossIcons1
        };
    }
}