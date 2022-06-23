using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PointShop.Common.Players;
using PointShop.Common.Systems;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using static PointShop.UI.ShopState;

namespace PointShop.UI.UIElements
{
    public class JuItemSlot : UIElement
    {
        public static readonly Texture2D Locking = MyUtils.GetTexture("BossIcons/Lock").Value;
        public readonly static Texture2D BossIcons1 = MyUtils.GetTexture("BossIcons/Map_Icon_Skeletron").Value;
        public readonly static Texture2D BossIcons2 = MyUtils.GetTexture("BossIcons/Map_Icon_Wall_of_Flesh").Value;
        public readonly static Texture2D BossIcons3 = MyUtils.GetTexture("BossIcons/Map_Icon_Skeletron_Prime").Value;
        public readonly static Texture2D BossIcons4 = MyUtils.GetTexture("BossIcons/Map_Icon_Plantera").Value;
        public readonly static float Size = 45;

        public int value;
        public int mode;
        public Terrain HuanJing;
        public Item item;
        public float textureSize = 24f;
        public Texture2D itemTexture;
        public Texture2D borderT2d;
        public Texture2D borderHoverT2d;
        public Texture2D backgroundT2d;
        public Color backgroundColor = new Color(63, 82, 151) * 0.7f;
        public UIText text;
        public UIImage lockImage;
        public UIImage BossT2d;

        private void LoadTextures(int itemType)
        {
            if (borderT2d is null)
            {
                borderT2d = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder", AssetRequestMode.ImmediateLoad).Value;
            }

            borderHoverT2d = MyUtils.GetTexture("PanelBorderHover").Value;

            if (backgroundT2d is null)
            {
                backgroundT2d = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground", AssetRequestMode.ImmediateLoad).Value;
            }

            itemTexture = Main.Assets.Request<Texture2D>("Images/Item_" + itemType, AssetRequestMode.ImmediateLoad).Value;
        }

        public JuItemSlot(int itemType, int value, Terrain HuanJing, int mode)
        {
            LoadTextures(itemType);
            item = new Item(itemType);
            Width.Set(Size, 0f);
            Height.Set(Size, 0f);

            this.value = value;
            this.HuanJing = HuanJing;
            this.mode = mode;

            text = new($"{value}", 0.6f)
            {
                VAlign = 0.2f,
                HAlign = 0.8f
            };
            Append(text);

            lockImage = new(Locking)
            {
                VAlign = 0.2f,
                HAlign = 0.2f,
                Color = Color.White * 0f,
                ImageScale = 0.7f
            };

            BossT2d = new(BossIcons1)
            {
                VAlign = 0.8f,
                HAlign = 0.8f,
                Color = Color.White * 0f,
                ImageScale = 0.65f
            };
            Append(BossT2d);
            Append(lockImage);
        }

        public override void Click(UIMouseEvent evt)
        {
            bool locking = UnlockItem(mode);

            // 不能兑换直接退出
            if (locking)
            {
                Main.NewText(MyUtils.GetText("Hint.未解锁该物品"), Color.Red);
                return;
            }

            //
            CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
            if (coinPlayer.Point[(int)CoinModSystem.coinUI.terrain] >= value)
            {
                coinPlayer.Point[(int)CoinModSystem.coinUI.terrain] -= value;
                Main.NewText(MyUtils.GetText("Hint.Success"), new Color(0x00, 0x99, 0xff));
                Main.LocalPlayer.QuickSpawnItem(null, item.Clone());
            }
            else
            {
                Main.NewText(MyUtils.GetText("Hint.NotPoint"), Color.Red);
            }
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            // 鼠标放到它上面显示物品介绍
            if (ContainsPoint(Main.MouseScreen) && item.type != ItemID.None)
            {
                Main.hoverItemName = item.Name;
                Main.HoverItem = item.Clone();
            }

            MyUtils.DrawPanel(sb, GetDimensions(), backgroundT2d, backgroundColor);
            // 金色 Border
            if (ContainsPoint(Main.MouseScreen))
            {
                MyUtils.DrawPanel(sb, GetDimensions(), borderHoverT2d, Color.White);
            }
            else
            {
                MyUtils.DrawPanel(sb, GetDimensions(), borderT2d, Color.Black);
            }

            bool locking = UnlockItem(mode);
            // 绘制物品
            Vector2 position = GetDimensions().Position();
            float size = itemTexture.Width > textureSize || itemTexture.Height > textureSize ?
                itemTexture.Width > itemTexture.Height ? textureSize / itemTexture.Width : textureSize / itemTexture.Height :
                1f;
            sb.Draw(itemTexture,
                position + new Vector2((Width.Pixels - itemTexture.Width * size) / 2f,
                (Height.Pixels - itemTexture.Height * size) / 2f),
                null, Color.White * (locking ? 0.5f : 1f), 0f, Vector2.Zero, size, 0, 0f);

            // 绘制锁定标志
            if (locking)
            {
                lockImage.Color = Color.White;
                BossT2d.Color = Color.White;
                text.TextColor *= 0;
                BossT2d.Recalculate();
            }
            else
            {
                lockImage.Color *= 0f;
                BossT2d.Color *= 0f;
                CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
                if (coinPlayer.Point[(int)CoinModSystem.coinUI.terrain] >= value)
                {
                    text.TextColor = Color.White;
                }
                else
                {
                    text.TextColor = Color.White * 0.5f;
                }
            }
        }

        private bool UnlockItem(int mode)
        {
            if (mode == 1 && !NPC.downedBoss3)
            {
                BossT2d.SetImage(BossIcons1);
                return true;
            }
            else if (mode == 2 && !Main.hardMode)
            {
                BossT2d.SetImage(BossIcons2);
                return true;
            }
            else if (mode == 3 && !NPC.downedMechBossAny)
            {
                BossT2d.SetImage(BossIcons3);
                return true;
            }
            else if (mode == 4 && !NPC.downedPlantBoss)
            {
                BossT2d.SetImage(BossIcons4);
                return true;
            }
            return false;
        }
    }
}
