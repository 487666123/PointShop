using Microsoft.Xna.Framework;
using PointShop.Common.Players;
using PointShop.UI.UIElements;
using System.Text.Json.Nodes;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PointShop.UI
{
    /// <summary>
    /// 一共有三个面板，一个主面板，其他两个在主面板内，分别是菜单面板和物品展示面板
    /// </summary>
    public class PointShopGUI : UIState
    {
        // 是否显示它
        internal static bool Visible = false;

        private static readonly int SizeWidth = 9;

        public Vector2 offset = Vector2.Zero;
        public bool dragging = false;

        public Terrain terrain;
        public UIElement TilePanel;
        public JuPanel MainPanel;
        public JuPanel MenuPanel;
        public JuPanel ItemPanel;
        public UIText TileUI;
        public UIImage LogoImage;

        private void OnInitialize_MainPanel()
        {
            MainPanel = new()
            {
                PaddingTop = 8f,
                PaddingBottom = 8f,
                PaddingLeft = 10f,
                PaddingRight = 10f
            };
            MainPanel.Width.Set(JuItemSlot.Size * SizeWidth + 8 * (SizeWidth - 1) + 16 * 2 + 10 * 2, 0f);
            MainPanel.Height.Set(340f, 0f);
            MainPanel.Left.Set(Main.screenWidth / 2f - MainPanel.Width.Pixels / 2f, 0f);
            MainPanel.Top.Set(Main.screenHeight / 2f - MainPanel.Height.Pixels / 2f, 0f);
            Append(MainPanel);
        }
        private void OnInitialize_TilePanel()
        {// 主标题
            TilePanel = new()
            {
                PaddingLeft = 8f,
                PaddingRight = 8f
            };
            TilePanel.Width.Set(MainPanel.Width(), 0f);
            TilePanel.Height.Set(35f, 0f);
            // 图标
            LogoImage = new(PointShop.IconTextures[(int)terrain])
            {
                VAlign = 0.5f,
                ImageScale = 0.9f
            };
            LogoImage.Left.Set(14 - LogoImage.Width() / 2f, 0);
            // 环境和积分，数字
            TileUI = new(terrain + "积分：0", 0.8f)
            {
                VAlign = 0.5f
            };
            TileUI.Left.Set(35f, 0f);
            TileUI.OnUpdate += (uie) =>
            {
                UIText _this = uie as UIText;
                _this.SetText($"{MyUtils.GetText($"HuanJingName.{terrain}") + MyUtils.GetText("Hint.Point")}: {CoinPlayer.GetPoint(terrain)}");
                _this.Recalculate();
            };
            // 关闭按钮
            UIImageButton BackImageButton = new(MyUtils.GetTexture("Button_Close"))
            {
                VAlign = 0.5f,
                HAlign = 1f
            };
            BackImageButton.OnClick += (evt, uie) => Visible = false;
            TilePanel.Append(LogoImage);
            TilePanel.Append(TileUI);
            TilePanel.Append(BackImageButton);
            MainPanel.Append(TilePanel);
        }
        private void OnInitialize_MenuPanel()
        {
            MenuPanel = new()
            {
                PaddingTop = 14f,
                PaddingBottom = 14f,
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            MenuPanel.Width.Set(JuItemSlot.Size * SizeWidth + 8 * (SizeWidth - 1) + 16 * 2, 0f);
            MenuPanel.Height.Set(45f, 0f);
            MenuPanel.Top.Set(TilePanel.Top() + TilePanel.Height() + 6f, 0f);
            MenuPanel.Left.Set(0f, 0f);

            // 循环添加菜单
            float offsetX = 0;
            for (int i = 0; i < PointShop.ItemExchangeInfo.Count; i++)
            {
                MenuPanel.Append(new JuMeunButton(PointShop.IconTextures[i], i, ref offsetX, this));
            }
        }
        private void OnInitialize_ItemPanel()
        {
            ItemPanel = new()
            {
                PaddingTop = 14f,
                PaddingBottom = 14f,
                PaddingLeft = 16f,
                PaddingRight = 16f
            };
            ItemPanel.Height.Set(MainPanel.HeightMinus() - TilePanel.Height() - MenuPanel.Height() - 12, 0f);
            ItemPanel.Width.Set(MainPanel.WidthMinus(), 0f);
            ItemPanel.Top.Set(MenuPanel.Top() + MenuPanel.Height() + 6f, 0f);
            ItemPanel.Left.Set(0f, 0f);
        }

        public override void OnInitialize()
        {
            screenWidth = Main.screenWidth;
            screenHeight = Main.screenHeight;

            // 主面板
            OnInitialize_MainPanel();

            // 标题
            OnInitialize_TilePanel();

            // 显示菜单的面板
            OnInitialize_MenuPanel();

            // 显示物品的面板
            OnInitialize_ItemPanel();

            // 顺序：物品面板 → 菜单面板，防止菜单的提示被物品面板覆盖
            MainPanel.Append(ItemPanel);
            MainPanel.Append(MenuPanel);

            JsonArray ItemConfig = PointShop.ItemExchangeInfo[(int)terrain].AsObject()["items"].AsArray();
            // 在UI里面添加 Item
            JuMeunButton.LoadItemConfig(this, ItemConfig, terrain);
        }

        public override void MouseUp(UIMouseEvent evt) { dragging = false; }

        public override void MouseDown(UIMouseEvent evt)
        {
            if (TilePanel.ContainsPoint(Main.MouseScreen))
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
            }

            // 判断鼠标是否在面板内
            if (MainPanel.ContainsPoint(Main.MouseScreen) || TilePanel.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            // 当屏幕大小发生改变时候刷新面板
            if (screenWidth != Main.screenWidth || screenHeight != Main.screenHeight)
            {
                screenWidth = Main.screenWidth;
                screenHeight = Main.screenHeight;
                MainPanel.SetPos(screenWidth / 2f - MainPanel.Width.Pixels / 2f, screenHeight / 2f - MainPanel.Height.Pixels / 2f);
            }
        }

        // 地形分类
        public enum Terrain : byte
        {
            SenLin, TianKong, DongXue, DiYu, CongLin, HaiYang, XueDi, ShaMo, FuHua, XingHong, ShenSheng, DiLao, Count
        }

        public static readonly Color[] TerrainColor = new Color[]
        {
            new(28, 216, 94),
            new(249, 255, 206),
            new(74, 67, 60),
            new(150, 33, 10),
            new(121, 176, 24),
            new(9, 61, 191),
            new(106, 210, 255),
            new(149, 80, 51),
            new(109, 90, 128),
            new(128, 44, 45),
            new(33, 171, 207),
            new(66, 84, 109),
            Color.White
        };
    }
}
