using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PointShop.Common.Players;
using PointShop.Common.Systems;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using static PointShop.Interface.PointShopGUI;

namespace PointShop.Interface.UIElements
{
    public class SingleItemSlot : UIElement
    {
        public readonly static Texture2D Locking = MyUtils.GetTexture("BossIcons/Lock").Value;
        public readonly static Texture2D BossIcons1 = MyUtils.GetTexture("BossIcons/Map_Icon_Skeletron").Value;
        public readonly static Texture2D BossIcons2 = MyUtils.GetTexture("BossIcons/Map_Icon_Wall_of_Flesh").Value;
        public readonly static Texture2D BossIcons3 = MyUtils.GetTexture("BossIcons/Map_Icon_Skeletron_Prime").Value;
        public readonly static Texture2D BossIcons4 = MyUtils.GetTexture("BossIcons/Map_Icon_Plantera").Value;
        public readonly static Texture2D InventoryHover = MyUtils.GetTexture("Inventory_Hover").Value;
        public readonly static int Size = TextureAssets.InventoryBack.Width();

        private bool _playSound;
        public int value;
        public int mode;
        public Terrain terrain;
        public Item item;
        public float textureSize = 28f;
        public Texture2D ItemTexture;
        public Color backgroundColor = new Color(63, 82, 151) * 0.7f;
        public UIImage LockTexture;
        public UIImage BossTexture;

        private void SetItem(int itemType)
        {
            Main.instance.LoadItem(itemType);
            ItemTexture = TextureAssets.Item[itemType].Value;
            item = new(itemType);
        }

        public SingleItemSlot(int type, int value, Terrain terrain, int mode)
        {
            SetItem(type);
            Width.Set(Size, 0f);
            Height.Set(Size, 0f);

            this.value = value;
            this.terrain = terrain;
            this.mode = mode;

            UIText PointText = new($"P: {value}", 0.6f)
            {
                VAlign = 0.8f,
                HAlign = 0.5f,
            };
            PointText.OnUpdate += (uie) =>
            {
                if (UnlockItem())
                    (uie as UIText).TextColor = Color.Transparent;
                else
                    (uie as UIText).TextColor = Color.White;
            };
            Append(PointText);

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

        public override void Click(UIMouseEvent evt)
        {
            bool locking = UnlockItem();

            // 不能兑换直接退出
            if (locking)
            {
                Main.NewText(MyUtils.GetText("Hint.Locked"), Color.Red);
                return;
            }

            CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
            if (coinPlayer.Point[(int)InterfaceSystem.PointShopGUI.terrain] >= value)
            {
                coinPlayer.Point[(int)InterfaceSystem.PointShopGUI.terrain] -= value;
                Main.NewText(MyUtils.GetText("Hint.Success"), new Color(0x00, 0x99, 0xff));
                Main.LocalPlayer.QuickSpawnItem(null, item.Clone());
            }
            else
            {
                Main.NewText(MyUtils.GetText("Hint.NotPoint"), Color.Red);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMouseHovering)
            {
                if (_playSound)
                {
                    _playSound = false;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }
            else
            {
                _playSound = true;
            }
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

            sb.Draw(TextureAssets.InventoryBack.Value, GetDimensions().Position(), Color.White);
            if (IsMouseHovering)
            {
                sb.Draw(InventoryHover, GetDimensions().Position(), Color.White);
            }

            bool locking = UnlockItem();
            // 绘制物品
            Vector2 position = GetDimensions().Position();
            LimitSize(ItemTexture, textureSize, out float scale);
            sb.Draw(ItemTexture, position + (this.Size() - ItemTexture.Size() * scale) / 2f,
                null, Color.White * (locking ? 0.5f : 1f), 0f, Vector2.Zero, scale, 0, 0f);

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
