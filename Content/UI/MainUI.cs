using Microsoft.Xna.Framework;
using PointShop.Common.Players;
using System.Text.Json.Nodes;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PointShop.Content.UI
{
    /// <summary>
    /// 一共有三个面板，一个主面板，其他两个在主面板内，分别是菜单面板和物品展示面板
    /// </summary>
    public class MainUI : UIState
    {
        // 是否显示它
        internal static bool Visible = false;

        private static readonly int SizeWidth = 9;

        public Vector2 offset = Vector2.Zero;
        public bool dragging = false;

        public Terrain terrain;
        public UIPanel2 MainTile;
        public UIPanel2 MainPanel;
        public UIPanel2 MenuPanel;
        public UIPanel2 ItemPanel;
        public UIText TileUI;
        public UIImage LogoImage;

        public override void OnInitialize()
        {
            screenWidth = Main.screenWidth;
            screenHeight = Main.screenHeight;

            // 主面板
            MainPanel = new()
            {
                PaddingTop = 8f,
                PaddingBottom = 8f,
                PaddingLeft = 10f,
                PaddingRight = 10f
            };
            MainPanel.Width.Set(ItemSlot2.Size * SizeWidth + 8 * (SizeWidth - 1) + 16 * 2 + 10 * 2, 0f);
            MainPanel.Height.Set(305f, 0f);
            MainPanel.Left.Set(Main.screenWidth / 2f - MainPanel.Width.Pixels / 2f, 0f);
            MainPanel.Top.Set(Main.screenHeight / 2f - MainPanel.Height.Pixels / 2f, 0f);

            // 显示菜单的面板
            MenuPanel = new()
            {
                PaddingTop = 14f,
                PaddingBottom = 14f,
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            MenuPanel.Width.Set(ItemSlot2.Size * SizeWidth + 8 * (SizeWidth - 1) + 16 * 2, 0f);
            MenuPanel.Height.Set(45f, 0f);
            MenuPanel.Left.Set(0f, 0f);
            MenuPanel.Top.Set(0f, 0f);

            // 循环添加菜单
            float offsetX = 0;
            for (int i = 0; i < PointShop.ItemExchangeInfo.Count; i++)
            {
                MenuPanel.Append(new UIMenuButton(PointShop.IconTextures[i], i, ref offsetX, this));
            }

            // 显示物品的面板
            ItemPanel = new()
            {
                PaddingTop = 14f,
                PaddingBottom = 14f,
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            ItemPanel.Width.Set(ItemSlot2.Size * SizeWidth + 8 * (SizeWidth - 1) + 16 * 2, 0f);
            ItemPanel.Height.Set(MainPanel.Height.Pixels - MenuPanel.Height.Pixels - 8f * 2 - 10f, 0f);
            ItemPanel.Left.Set(0f, 0f);
            ItemPanel.Top.Set(MenuPanel.Height.Pixels + 6f, 0f);

            // 顺序：物品面板 → 菜单面板
            this.Append(MainPanel);
            MainPanel.Append(ItemPanel);
            MainPanel.Append(MenuPanel);


            // 主标题
            MainTile = new()
            {
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            MainTile.Width.Set(MainPanel.Width.Pixels, 0f);
            MainTile.Height.Set(45f, 0f);
            // 图标
            LogoImage = new(PointShop.IconTextures[(int)terrain])
            {
                VAlign = 0.5f
            };
            // 环境和积分
            TileUI = new(terrain + "积分：0", 0.8f)
            {
                VAlign = 0.5f
            };
            TileUI.Left.Set(LogoImage.Width.Pixels + 10f, 0f);
            // 关闭按钮
            UIImageButton BackImageButton = new(MyUtils.GetTexture("Button_Close"))
            {
                VAlign = 0.5f,
                HAlign = 1f
            };
            BackImageButton.OnClick += (evt, uie) => Visible = false;
            MainTile.Append(LogoImage);
            MainTile.Append(TileUI);
            MainTile.Append(BackImageButton);
            this.Append(MainTile);

            JsonArray ItemConfig = PointShop.ItemExchangeInfo[(int)terrain].AsObject()["items"].AsArray();
            // 在UI里面添加 Item
            UIMenuButton.LoadItemConfig(this, ItemConfig, terrain);
        }

        public override void MouseUp(UIMouseEvent evt) { dragging = false; }

        public override void MouseDown(UIMouseEvent evt)
        {
            if (MainTile.ContainsPoint(Main.MouseScreen))
            {
                dragging = true;
                offset = evt.MousePosition - MainPanel.GetDimensions().Position();
            }
        }

        // 物品表结构
        private int screenWidth = 0;
        private int screenHeight = 0;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (dragging)
            {
                MainPanel.SetPos(Main.mouseX - offset.X, Main.mouseY - offset.Y);
                MainTile.SetPos(MainPanel.Left.Pixels, MainPanel.Top.Pixels - MainTile.Height.Pixels - 10f);
            }

            // 判断鼠标是否在面板内
            if (MainPanel.ContainsPoint(Main.MouseScreen) || MainTile.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            // 当屏幕大小发生改变时候刷新面板
            if (screenWidth != Main.screenWidth || screenHeight != Main.screenHeight)
            {
                screenWidth = Main.screenWidth;
                screenHeight = Main.screenHeight;
                MainPanel.SetPos(screenWidth / 2f - MainPanel.Width.Pixels / 2f, screenHeight / 2f - MainPanel.Height.Pixels / 2f);
                MainTile.SetPos(MainPanel.Left.Pixels, MainPanel.Top.Pixels - MainTile.Height.Pixels - 10f);
            }

            // 标题文字
            TileUI.SetText($"{MyUtils.GetText($"HuanJingName.{terrain}") + MyUtils.GetText("Hint.Point")}: {CoinPlayer.GetPoint(terrain)}");
            TileUI.Recalculate();
        }

        // 地形分类
        public enum Terrain : byte
        {
            SenLin, TianKong, DongXue, DiYu, CongLin, HaiYang, XueDi, ShaMo, FuHua, XingHong, ShenSheng, DiLao, Count
        }
    }

    public static class UIUtil
    {
        public static void SetPos(this UIElement uie, float x, float y, float precentX = 0, float precentY = 0)
        {
            uie.Left.Set(x, precentX);
            uie.Top.Set(y, precentY);
            uie.Recalculate();
        }

        public static void SetSize(this UIElement uie, float width, float height, float precentWidth = 0, float precentHeight = 0)
        {
            uie.Width.Set(width, precentWidth);
            uie.Height.Set(height, precentHeight);
            uie.Recalculate();
        }
    }
}
