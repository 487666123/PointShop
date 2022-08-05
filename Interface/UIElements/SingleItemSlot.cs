using PointShop.Common.Animations;
using PointShop.Common.Players;
using PointShop.Common.Systems;
using PointShop.Interface;

namespace PointShop.ModUI.UIElements
{
    public class SingleItemSlot : UIElement
    {
        public readonly static Texture2D Locking = ModHelper.GetTexture("BossIcons/Lock").Value;
        public readonly static Texture2D BossIcons1 = ModHelper.GetTexture("BossIcons/Map_Icon_Skeletron").Value;
        public readonly static Texture2D BossIcons2 = ModHelper.GetTexture("BossIcons/Map_Icon_Wall_of_Flesh").Value;
        public readonly static Texture2D BossIcons3 = ModHelper.GetTexture("BossIcons/Map_Icon_Skeletron_Prime").Value;
        public readonly static Texture2D BossIcons4 = ModHelper.GetTexture("BossIcons/Map_Icon_Plantera").Value;
        public readonly static Texture2D InventoryHover = ModHelper.GetTexture("Inventory_Hover").Value;

        public int value;
        public int mode;
        public Terrain terrain;
        public Item item;
        public float textureSize = 28f;
        public Texture2D ItemTexture;
        public Color backgroundColor = new Color(63, 82, 151) * 0.7f;
        public UIImage LockTexture;
        public UIImage BossTexture;

        public AnimationTimer HoverTimer = new(3);

        private void SetItem(int itemType)
        {
            Main.instance.LoadItem(itemType);
            ItemTexture = TextureAssets.Item[itemType].Value;
            item = new(itemType);
        }

        public SingleItemSlot(int type, int value, Terrain terrain, int mode)
        {
            SetItem(type);
            Width.Set(52, 0f);
            Height.Set(52, 0f);

            this.value = value;
            this.terrain = terrain;
            this.mode = mode;

            LockTexture = new(Locking)
            {
                VAlign = 0.2f,
                HAlign = 0.2f,
                Color = Color.White * 0f,
                ImageScale = 0.7f
            };

            BossTexture = new(BossIcons1)
            {
                VAlign = 0.8f,
                HAlign = 0.8f,
                Color = Color.White * 0f,
                ImageScale = 0.65f
            };
            Append(BossTexture);
            Append(LockTexture);
        }

        public void Play()
        {
            // 不能兑换直接退出
            if (UnlockItem())
            {
                Main.NewText(ModHelper.GetText("Hint.Locked"), Color.Red);
                return;
            }

            CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
            if (coinPlayer.Point[(int)UISystem.PointShopGUI.terrain] >= value)
            {
                coinPlayer.Point[(int)UISystem.PointShopGUI.terrain] -= value;
                Main.NewText(ModHelper.GetText("Hint.Success"), new Color(0x00, 0x99, 0xff));
                Main.LocalPlayer.QuickSpawnItem(null, item.Clone());
            }
            else
            {
                Main.NewText(ModHelper.GetText("Hint.NotPoint"), Color.Red);
            }
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
            // 鼠标放到它上面显示物品介绍
            if (IsMouseHovering)
            {
                if (!item.IsAir)
                {
                    Main.hoverItemName = item.Name;
                    Main.HoverItem = item.Clone();
                }
            }


            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = dimensions.Position();
            Vector2 size = dimensions.Size();

            Color border = Color.Lerp(ModColor.ButtonBorder, ModColor.ButtonBorderHover, HoverTimer.Schedule);

            PixelShader.DrawBox(Main.UIScaleMatrix, position, size, 10, 3, border, ModColor.ButtonBackground);

            bool locking = UnlockItem();
            // 绘制物品
            DrawItemInternal(sb, item, locking ? Color.White * 0.5f : Color.White, GetDimensions(), 30);

            // 绘制锁定标志
            if (locking)
            {
                LockTexture.Color = Color.White;
                BossTexture.Color = Color.White;
            }
            else
            {
                LockTexture.Color = Color.Transparent;
                BossTexture.Color = Color.Transparent;
            }
        }

        public static void DrawItemInternal(SpriteBatch sb, Item Item, Color lightColor, CalculatedStyle dimensions, float ItemSize = 30f)
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

        private bool UnlockItem()
        {
            if (mode == 1 && !NPC.downedBoss3)
            {
                BossTexture.SetImage(BossIcons1);
                return true;
            }
            else if (mode == 2 && !Main.hardMode)
            {
                BossTexture.SetImage(BossIcons2);
                return true;
            }
            else if (mode == 3 && !NPC.downedMechBossAny)
            {
                BossTexture.SetImage(BossIcons3);
                return true;
            }
            else if (mode == 4 && !NPC.downedPlantBoss)
            {
                BossTexture.SetImage(BossIcons4);
                return true;
            }
            return false;
        }
    }

}
