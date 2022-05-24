using PointShop.Common.Players;
using PointShop.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static PointShop.Content.UI.CoinUI;

namespace PointShop.Content.UI
{
    public class ItemDisplaySlot : UIElement
    {
        public int value;
        public int mode;
        public HuanJing HuanJing;
        public Item item;
        public float textureSize = 30f;
        public Texture2D texture;
        public Texture2D PanelBorder;
        public Texture2D PanelBorderHover;
        public Texture2D PanelBackground;
        public UIText text;
        public UIImage lockImage;
        public UIImage bossImage;

        public Texture2D Locking;
        public Texture2D BossIcons1;
        public Texture2D BossIcons2;
        public Texture2D BossIcons3;
        public Texture2D BossIcons4;

        public ItemDisplaySlot(int itemType, int value, HuanJing HuanJing, int mode)
        {
            PanelBorder = ModContent.Request<Texture2D>("PointShop/Images/PanelBorder", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            PanelBorderHover = ModContent.Request<Texture2D>("PointShop/Images/PanelBorderHover", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            PanelBackground = ModContent.Request<Texture2D>("PointShop/Images/PanelBackground", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            texture = ModContent.Request<Texture2D>("Terraria/Images/Item_" + itemType, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            Locking = ModContent.Request<Texture2D>("PointShop/Images/BossIcons/Lock", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            BossIcons1 = ModContent.Request<Texture2D>("PointShop/Images/BossIcons/Map_Icon_Skeletron", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            BossIcons2 = ModContent.Request<Texture2D>("PointShop/Images/BossIcons/Map_Icon_Wall_of_Flesh", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            BossIcons3 = ModContent.Request<Texture2D>("PointShop/Images/BossIcons/Map_Icon_Skeletron_Prime", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            BossIcons4 = ModContent.Request<Texture2D>("PointShop/Images/BossIcons/Map_Icon_Plantera", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;


            item = new Item(itemType);
            Width.Set(55f, 0f);
            Height.Set(55f, 0f);

            this.value = value;
            this.HuanJing = HuanJing;
            this.mode = mode;

            text = new(value + "", 0.65f);
            text.VAlign = 0.2f;
            text.HAlign = 0.8f;
            Append(text);

            lockImage = new(Locking);
            lockImage.VAlign = 0.2f;
            lockImage.HAlign = 0.2f;
            lockImage.Color = Color.White * 0f;
            lockImage.ImageScale = 0.8f;

            bossImage = new(BossIcons1);
            bossImage.VAlign = 0.8f;
            bossImage.HAlign = 0.8f;
            bossImage.Color = Color.White * 0f;
            bossImage.ImageScale = 0.8f;
            Append(bossImage);
            Append(lockImage);
        }

        public override void Click(UIMouseEvent evt)
        {
            bool locking = false;
            if (mode == 1 && !NPC.downedBoss3)
            {
                locking = true;
            }
            else if (mode == 2 && !Main.hardMode)
            {
                locking = true;
            }
            else if (mode == 3 && !Main.hardMode)
            {
                locking = true;
            }
            else if (mode == 4 && !NPC.downedPlantBoss)
            {
                locking = true;
            }
            // 不能兑换直接退出
            if (locking)
            {
                Main.NewText(Language.GetTextValue($"Mods.PointShop.Hint.未解锁该物品"), Color.Red);
                return;
            }

            //
            CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
            if (coinPlayer.HuanJingFen[(int)CoinModSystem.coinUI.huanJing] >= value)
            {
                coinPlayer.HuanJingFen[(int)CoinModSystem.coinUI.huanJing] -= value;
                Main.NewText(Language.GetTextValue($"Mods.PointShop.Hint.兑换成功"), new Color(0x00, 0x99, 0xff));
                Main.LocalPlayer.QuickSpawnItem(null, item.Clone());
            }
            else
            {
                Main.NewText(Language.GetTextValue($"Mods.PointShop.Hint.兑换失败积分不足"), Color.Red);
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

            // 深色背景
            DrawPanel(sb, GetDimensions(), PanelBackground, Color.White * 0.7f);

            // 金色 Border
            if (ContainsPoint(Main.MouseScreen))
            {
                DrawPanel(sb, GetDimensions(), PanelBorderHover, Color.White);
            }
            else
            {
                DrawPanel(sb, GetDimensions(), PanelBorder, Color.White);
            }

            // 物品上锁标志
            bool locking = false;
            if (mode == 1 && !NPC.downedBoss3)
            {
                locking = true;
                bossImage.SetImage(BossIcons1);
            }
            else if (mode == 2 && !Main.hardMode)
            {
                locking = true;
                bossImage.SetImage(BossIcons2);
            }
            else if (mode == 3 && !NPC.downedMechBossAny)
            {
                locking = true;
                bossImage.SetImage(BossIcons3);
            }
            else if (mode == 4 && !NPC.downedPlantBoss)
            {
                locking = true;
                bossImage.SetImage(BossIcons4);
            }

            // 绘制物品
            Vector2 position = GetDimensions().Position();
            float size = (texture.Width > textureSize || texture.Height > textureSize) ?
                texture.Width > texture.Height ? textureSize / texture.Width : textureSize / texture.Height :
                1f;
            sb.Draw(texture,
                position + new Vector2((Width.Pixels - texture.Width * size) / 2f,
                (Height.Pixels - texture.Height * size) / 2f),
                null, Color.White * (locking ? 0.5f : 1f), 0f, Vector2.Zero, size, 0, 0f);

            // 绘制锁定标志
            if (locking)
            {
                lockImage.Color = Color.White;
                bossImage.Color = Color.White;
                text.TextColor *= 0;
                bossImage.Recalculate();
            }
            else
            {
                lockImage.Color *= 0f;
                bossImage.Color *= 0f;
                CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
                if (coinPlayer.HuanJingFen[(int)CoinModSystem.coinUI.huanJing] >= value)
                {
                    text.TextColor = Color.White;
                }
                else
                {
                    text.TextColor = Color.White * 0.5f;
                }
            }
        }

        // 绘制面板
        public static void DrawPanel(SpriteBatch sb, CalculatedStyle dimensions, Texture2D texture, Color color)
        {
            Point point = new Point((int)dimensions.X, (int)dimensions.Y);
            Point point2 = new Point(point.X + (int)dimensions.Width - 12, point.Y + (int)dimensions.Height - 12);
            int width = point2.X - point.X - 12;
            int height = point2.Y - point.Y - 12;
            sb.Draw(texture, new Rectangle(point.X, point.Y, 12, 12), new Rectangle(0, 0, 12, 12), color);
            sb.Draw(texture, new Rectangle(point2.X, point.Y, 12, 12), new Rectangle(12 + 4, 0, 12, 12), color);
            sb.Draw(texture, new Rectangle(point.X, point2.Y, 12, 12), new Rectangle(0, 12 + 4, 12, 12), color);
            sb.Draw(texture, new Rectangle(point2.X, point2.Y, 12, 12), new Rectangle(12 + 4, 12 + 4, 12, 12), color);
            sb.Draw(texture, new Rectangle(point.X + 12, point.Y, width, 12), new Rectangle(12, 0, 4, 12), color);
            sb.Draw(texture, new Rectangle(point.X + 12, point2.Y, width, 12), new Rectangle(12, 12 + 4, 4, 12), color);
            sb.Draw(texture, new Rectangle(point.X, point.Y + 12, 12, height), new Rectangle(0, 12, 12, 4), color);
            sb.Draw(texture, new Rectangle(point2.X, point.Y + 12, 12, height), new Rectangle(12 + 4, 12, 12, 4), color);
            sb.Draw(texture, new Rectangle(point.X + 12, point.Y + 12, width, height), new Rectangle(12, 12, 4, 4), color);
        }
    }
}
