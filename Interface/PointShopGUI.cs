using Microsoft.Xna.Framework;
using PointShop.Common.Players;
using PointShop.Entitys;
using PointShop.Interface.UIElements;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PointShop.Interface
{
    /// <summary>
    /// 一共有三个面板，一个主面板，其他两个在主面板内，分别是菜单面板和物品展示面板
    /// </summary>
    public class PointShopGUI : UIState
    {
        // 是否显示它
        internal static bool Visible = false;

        public Vector2 offset = Vector2.Zero;
        public bool dragging = false;

        public Terrain terrain;
        public UIText UIText;
        public UIImageButton ImageButton;
        public UIPanel MainPanel;
        public ScrollView MenuView;
        public ScrollView ItemView;

        public override void OnInitialize()
        {
            screenWidth = Main.screenWidth;
            screenHeight = Main.screenHeight;

            MainPanel = new();
            MainPanel.SetPadding(14f);
            MainPanel.SetPos((Main.ScreenSize.ToVector2() - MainPanel.Size()) / 2f);
            Append(MainPanel);

            UIText = new(MyUtils.GetText("Config.积分兑换"));
            UIText.Left.Pixels = 10f;
            MainPanel.Append(UIText);

            ImageButton = new(MyUtils.GetTexture("Button_Close"))
            {
                HAlign = 1f
            };
            ImageButton.Left.Pixels = -10f;
            ImageButton.OnClick += (evt, uie) => Visible = !Visible;
            MainPanel.Append(ImageButton);

            // 显示菜单的面板
            MenuView = new(190, 50 * 5 + 10 * 4 + 20);
            MenuView.Top.Pixels += ImageButton.Height() + 5f;
            MainPanel.Append(MenuView);
            // 菜单按钮按钮
            for (int i = 0; i < PointShop.TerrainDatas.Count; i++)
            {
                IconTextButton button = new(PointShop.icon[i].Value, $"{PointShop.TerrainDatas[i].name}: {CoinPlayer.GetPoint(i)}");
                button.Width.Pixels = MenuView.ScrollList.WidthInside();
                button.Height.Pixels = 50f;
                button.data[0] = i;
                button.OnClick += (evt, uie) =>
                {
                    int _terrain = (uie as IconTextButton).data[0];
                    ModifyTerrain((Terrain)_terrain);
                };
                button.OnUpdate += (uie) =>
                {
                    int _terrain = (uie as IconTextButton).data[0];
                    (uie as IconTextButton).SetText($"{PointShop.TerrainDatas[_terrain].name}: {CoinPlayer.GetPoint((Terrain)_terrain)}");
                    uie.Recalculate();
                };
                MenuView.AppendElement(button);
            }

            // 显示物品的面板
            int num = 6;
            ItemView = new(SingleItemSlot.Size * num + 10 * (num - 1) + 20f, MenuView.Height());
            ItemView.Top.Pixels = MenuView.Top();
            ItemView.Left.Pixels = MenuView.Width();
            MainPanel.Append(ItemView);

            MainPanel.Width.Pixels = MenuView.Width() + ItemView.Width() + MainPanel.HPadding();
            MainPanel.Height.Pixels = MenuView.Top() + MenuView.Height() + MainPanel.VPadding();

            // 在UI里面添加 Item
            ModifyTerrain(Terrain.SenLin);
        }

        public void ModifyTerrain(Terrain terrain)
        {
            this.terrain = terrain;
            List<ItemData> itemData = PointShop.TerrainDatas[(int)terrain].items;
            // 判断有没有数据
            if (itemData.Count > 0)
            {
                ItemView.RemoveAllElement();
                for (int i = 0; i < itemData.Count; i++)
                {
                    SingleItemSlot ItemSlot = new(itemData[i].id, itemData[i].value, terrain, itemData[i].mode);
                    ItemView.AppendElement(ItemSlot);
                }
                ItemView.Recalculate();
            }
        }

        public override void MouseUp(UIMouseEvent evt) { dragging = false; }

        public override void MouseDown(UIMouseEvent evt)
        {
            if (MainPanel.IsMouseHovering && !MenuView.IsMouseHovering && !ItemView.IsMouseHovering && !ImageButton.IsMouseHovering)
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
            if (MainPanel.IsMouseHovering)
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
            new(255, 225, 143),
            new(128, 77, 57),
            new(160, 35, 0),
            new(121, 176, 24),
            new(48, 91, 191),
            new(51, 211, 255),
            new(153, 65, 31),
            new(83, 45, 117),
            new(128, 32, 32),
            new(0, 167, 209),
            new(35, 71, 117),
            Color.White
        };
    }
}
