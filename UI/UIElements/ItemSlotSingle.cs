using PointShop.Common.Animations;
using PointShop.Common.Players;
using PointShop.Common.Systems;
using PointShop.Interface;

namespace PointShop.ModUI.UIElements
{
    public class ItemSlotSingle : UIElement
    {
        public readonly static Texture2D BossIcons1 = ModHelper.GetTexture("BossIcons/Map_Icon_Skeletron").Value;
        public readonly static Texture2D BossIcons2 = ModHelper.GetTexture("BossIcons/Map_Icon_Wall_of_Flesh").Value;
        public readonly static Texture2D BossIcons3 = ModHelper.GetTexture("BossIcons/Map_Icon_Skeletron_Prime").Value;
        public readonly static Texture2D BossIcons4 = ModHelper.GetTexture("BossIcons/Map_Icon_Plantera").Value;

        public int value;
        public int mode;
        public Item Item;
        public Texture2D ItemTexture;
        public Terrain terrain;

        public Texture2D LockTexture;
        public Vector2 LockTextureSize;

        public Texture2D BossTexture;
        public Vector2 BossTextureSize;

        public AnimationTimer HoverTimer;

        public ItemSlotSingle(Item item, int value, Terrain terrain, int mode)
        {
            Width.Set(52, 0f);
            Height.Set(52, 0f);

            this.value = value;
            this.terrain = terrain;
            this.mode = mode;

            HoverTimer = new(3);

            Item = item;
            Main.instance.LoadItem(item.type);
            ItemTexture = TextureAssets.Item[item.type].Value;

            LockTexture = ModHelper.GetTexture("BossIcons/Lock").Value;
            LockTextureSize = LockTexture.Size();

            BossTexture = GetTexture2D();
            BossTextureSize = BossTexture.Size();
        }

        public override void Update(GameTime gameTime)
        {
            HoverTimer.Update();
            base.Update(gameTime);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            HoverTimer.Open();
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            HoverTimer.Close();
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            if (IsMouseHovering && !Item.IsAir)
            {
                Main.hoverItemName = Item.Name;
                Main.HoverItem = Item.Clone();
            }

            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = dimensions.Position();
            Vector2 size = dimensions.Size();

            Color border = Color.Lerp(UIColor.BorderNotFavorited, UIColor.BorderFavorited, HoverTimer.Schedule);
            Color background = Color.Lerp(UIColor.BackgroundNotFavorited, UIColor.BackgroundFavorited, HoverTimer.Schedule);

            PixelShader.DrawBox(position, size, 10, 3, border, background);

            bool canPlay = CanPlay();
            // 绘制物品
            DrawItem(sb, Item, canPlay ? Color.White : Color.White * 0.5f, GetDimensions(), 30);

            // 绘制锁定标志
            if (!canPlay)
            {
                sb.Draw(LockTexture, position + size * 0.3f, null, Color.White, 0, LockTextureSize / 2, 0.7f, 0, 0);
                sb.Draw(BossTexture, position + size * 0.6f, null, Color.White, 0, BossTextureSize / 2, 0.65f, 0, 0);
            }
        }

        public static void DrawItem(SpriteBatch sb, Item Item, Color lightColor, CalculatedStyle dimensions, float ItemSize = 30f)
        {
            Main.instance.LoadItem(Item.type);
            var ItemTexture2D = TextureAssets.Item[Item.type];

            Rectangle rectangle;
            if (Main.itemAnimations[Item.type] is null)
                rectangle = ItemTexture2D.Frame(1, 1, 0, 0);
            else
                rectangle = Main.itemAnimations[Item.type].GetFrame(ItemTexture2D.Value);

            float size = rectangle.Width > ItemSize || rectangle.Height > ItemSize ?
                rectangle.Width > rectangle.Height ? ItemSize / rectangle.Width : ItemSize / rectangle.Height :
                1f;

            sb.Draw(ItemTexture2D.Value, dimensions.Center() - rectangle.Size() * size / 2f,
                new Rectangle?(rectangle), Item.GetAlpha(lightColor), 0f, Vector2.Zero, size,
                SpriteEffects.None, 0f);
            sb.Draw(ItemTexture2D.Value, dimensions.Center() - rectangle.Size() * size / 2f,
                new Rectangle?(rectangle), Item.GetColor(lightColor), 0f, Vector2.Zero, size,
                SpriteEffects.None, 0f);
        }

        public static void LimitSize(Texture2D texture, float MaxSize, out float scale)
        {
            scale = texture.Width > MaxSize || texture.Height > MaxSize ?
                texture.Width > texture.Height ?
                MaxSize / texture.Width : MaxSize / texture.Height : 1f;
        }

        public void Pay()
        {
            // 不能兑换直接退出
            if (!CanPlay())
            {
                Main.NewText(ModHelper.GetText("Hint.Locked"), Color.Red);
                return;
            }

            CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
            if (coinPlayer.Point[(int)UISystem.PointShopGUI.terrain] >= value)
            {
                coinPlayer.Point[(int)UISystem.PointShopGUI.terrain] -= value;
                Main.NewText($"\"{Item.Name}\" {ModHelper.GetText("Hint.Success")}", new Color(0x00, 0x99, 0xff));
                Main.LocalPlayer.QuickSpawnItem(null, Item.Clone());
            }
            else
            {
                Main.NewText($"\"{Item.Name}\" {ModHelper.GetText("Hint.NotPoint")}", Color.Red);
            }
        }

        private bool CanPlay()
        {
            if (mode == 1)
            {
                return NPC.downedBoss3;
            }
            else if (mode == 2)
            {
                return Main.hardMode;
            }
            else if (mode == 3)
            {
                return NPC.downedMechBossAny;
            }
            else if (mode == 4)
            {
                return NPC.downedPlantBoss;
            }
            return true;
        }

        private Texture2D GetTexture2D()
        {
            if (mode == 2)
            {
                return BossIcons2;
            }
            else if (mode == 3)
            {
                return BossIcons3;
            }
            else if (mode == 4)
            {
                return BossIcons4;
            }
            return BossIcons1;
        }
    }

}
