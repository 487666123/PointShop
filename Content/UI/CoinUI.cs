using PointShop.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Text.Json.Nodes;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace PointShop.Content.UI
{
    /// <summary>
    /// 一共有三个面板，一个主面板，其他两个在主面板内，分别是菜单面板和物品展示面板
    /// </summary>
    public class CoinUI : UIState
    {
        // 是否显示它
        public static bool Visible = false;

        public Vector2 offset = Vector2.Zero;
        public bool dragging = false;

        // 环境分类
        public enum HuanJing
        {
            TianKong,
            SenLin,
            DongXue,
            DiYu,
            CongLin,
            HaiYang,
            XueDi,
            ShaMo,
            FuHua,
            XingHong,
            ShenSheng,
            DiLao,
            Count
        }

        public HuanJing huanJing;
        public UIPanel panel;
        public UIPanel MenuPanel;
        public UIPanel ItemListPanel;
        public UIText text;
        public UIPanel MainMenu;
        public UIImage LogoImage;

        public override void OnInitialize()
        {
            screenWidth = Main.screenWidth;
            screenHeight = Main.screenHeight;

            // 物品列表横向显示几个物品
            int LengthX = 9;

            // 主面板
            panel = new()
            {
                PaddingTop = 14f,
                PaddingBottom = 14f,
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            panel.Width.Set(55 * LengthX + 14 * (LengthX - 1) + 16 * 4, 0f);
            panel.Height.Set(400f, 0f);
            panel.Left.Set(Main.screenWidth / 2f - panel.Width.Pixels / 2f, 0f);
            panel.Top.Set(Main.screenHeight / 2f - panel.Height.Pixels / 2f, 0f);

            // 显示菜单的面板
            MenuPanel = new()
            {
                PaddingTop = 14f,
                PaddingBottom = 14f,
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            MenuPanel.Width.Set(55 * LengthX + 14 * (LengthX - 1) + 16 * 2, 0f);
            MenuPanel.Height.Set(50f, 0f);
            MenuPanel.Left.Set(0f, 0f);
            MenuPanel.Top.Set(0f, 0f);

            // 循环添加菜单
            float offsetX = 0;
            for (int i = 0; i < PointShop.ItemExchangeInfo.Count; i++)
            {
                MenuPanel.Append(new MenuButton(PointShop.IconTextures[i], i, ref offsetX, this));
            }

            // 显示物品的面板
            ItemListPanel = new()
            {
                PaddingTop = 14f,
                PaddingBottom = 14f,
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            ItemListPanel.Width.Set(55 * LengthX + 14 * (LengthX - 1) + 16 * 2, 0f);
            ItemListPanel.Height.Set(panel.Height.Pixels - MenuPanel.Height.Pixels - 14f * 3, 0f);
            ItemListPanel.Left.Set(0f, 0f);
            ItemListPanel.Top.Set(50f + 14f, 0f);

            // 顺序：物品面板 → 菜单面板
            this.Append(panel);
            panel.Append(ItemListPanel);
            panel.Append(MenuPanel);


            // 主菜单
            MainMenu = new UIPanel()
            {
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            MainMenu.Width.Set(panel.Width.Pixels, 0f);
            MainMenu.Height.Set(50, 0f);
            // Logo
            LogoImage = new(PointShop.IconTextures[((int)huanJing)])
            {
                VAlign = 0.5f
            };
            // 显示得分
            text = new(huanJing + "积分：0", 1f)
            {
                VAlign = 0.5f
            };
            text.Left.Set(LogoImage.Width.Pixels + 10f, 0f);
            // 关闭按钮
            UIImageButton BackImageButton = new(ModContent.Request<Texture2D>("PointShop/Images/Button_Back", AssetRequestMode.ImmediateLoad));
            BackImageButton.VAlign = 0.5f;
            BackImageButton.HAlign = 1f;
            BackImageButton.OnClick += BackImageButton_OnClick;
            MainMenu.Append(LogoImage);
            MainMenu.Append(text);
            MainMenu.Append(BackImageButton);
            this.Append(MainMenu);

            JsonArray jsonArray = PointShop.ItemExchangeInfo[((int)huanJing)].AsObject()["items"].AsArray();
            // 在UI里面添加 Item
            MenuButton.AddItemInCoinUI(this, jsonArray, huanJing);
        }

        private void BackImageButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Visible = false;
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            dragging = false;
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            if ((panel.ContainsPoint(Main.MouseScreen) || MainMenu.ContainsPoint(Main.MouseScreen)) &&
                !MenuPanel.ContainsPoint(Main.MouseScreen) && !ItemListPanel.ContainsPoint(Main.MouseScreen))
            {
                dragging = true;
                offset = new Vector2(evt.MousePosition.X - panel.Left.Pixels, evt.MousePosition.Y - panel.Top.Pixels);
            }
        }

        // 物品表结构
        private int screenWidth = 0;
        private int screenHeight = 0;

        public override void Update(GameTime gameTime)
        {
            if (dragging)
            {
                panel.Left.Set(Main.mouseX - offset.X, 0f);
                panel.Top.Set(Main.mouseY - offset.Y, 0f);
                panel.Recalculate();
                MainMenu.Left.Set(panel.Left.Pixels, 0f);
                MainMenu.Top.Set(panel.Top.Pixels - 50f - 16f, 0f);
                MainMenu.Recalculate();
            }

            // 判断鼠标是否在面板内
            if (panel.ContainsPoint(Main.MouseScreen) || MainMenu.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            // 当屏幕大小发生改变时候刷新面板
            if (screenWidth != Main.screenWidth || screenHeight != Main.screenHeight)
            {
                screenWidth = Main.screenWidth;
                screenHeight = Main.screenHeight;
                panel.Left.Set(screenWidth / 2f - panel.Width.Pixels / 2f, 0f);
                panel.Top.Set(screenHeight / 2f - panel.Height.Pixels / 2f, 0f);
                panel.Recalculate();
                MainMenu.Left.Set(panel.Left.Pixels, 0f);
                MainMenu.Top.Set(panel.Top.Pixels - 50f - 16f, 0f);
                MainMenu.Recalculate();
            }

            // 提示文字
            CoinPlayer coinPlayer = Main.LocalPlayer.GetModPlayer<CoinPlayer>();
            text.SetText(PointShop.ItemExchangeInfo[((int)huanJing)].AsObject()["name"].ToString() +
                Language.GetTextValue($"Mods.PointShop.Hint.积分2") + coinPlayer.HuanJingFen[(int)huanJing]);
            text.Recalculate();
        }
    }
}
