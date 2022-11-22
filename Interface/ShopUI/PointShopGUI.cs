using ImproveGame.Interface.UIElements_Shader;
using PointShop.Common.Configs;
using PointShop.Common.Players;
using PointShop.Entitys;
using PointShop.Interface;
using PointShop.Interface.Common;
using PointShop.Interface.UIElements;
using PointShop.ModUI.UIElements;
using PointShop.UI.Common;
using System.Collections.Generic;
using Terraria.GameInput;

namespace PointShop.UI.ShopUI
{
    public class PointShopGUI : UIState
    {
        internal static bool Visible = false;
        internal int OldPriceMultiplier = PointConfig.Instance.PointMultiplier;

        public Vector2 offset = Vector2.Zero;
        public bool dragging = false;

        public Terrain terrain;
        public UITitle title;
        public UIFork Close;
        public SUIPanel MainPanel;
        public ScrollView MenuView;
        public ScrollView ItemView;

        private readonly Color background = new(44, 57, 105, 160);

        public override void OnInitialize()
        {
            RemoveAllChildren();
            screenWidth = Main.screenWidth;
            screenHeight = Main.screenHeight;

            MainPanel = new(Color.Black, background);
            MainPanel.SetPadding(12f);
            MainPanel.SetPos(Main.LocalPlayer.GetModPlayer<UIPlayerData>().PointShopPos);
            Append(MainPanel);

            MainPanel.Append(title = new(ModHelper.GetText("Config.PointExchange"), 0.5f));

            Close = new(30)
            {
                HAlign = 1f
            };
            Close.Height.Pixels = title.Height.Pixels;
            Close.OnClick += (evt, uie) => Visible = !Visible;
            MainPanel.Append(Close);

            // 菜单面板
            MainPanel.Append(MenuView = new(150, 340f));
            MenuView.Top.Pixels += title.Bottom() + 15f;

            // 菜单按钮按钮
            for (int i = 0; i < UISystem.TerrainDatas.Count; i++)
            {
                Button button = new(UISystem.Icons[i].Value, $"{UISystem.TerrainDatas[i].name}");
                button.Width.Pixels = MenuView.ScrollList.WidthInside();
                button.Height.Pixels = 50f;
                button.data[0] = i;
                button.OnClick += (evt, uie) =>
                {
                    int _terrain = (uie as Button).data[0];
                    ModifyTerrain((Terrain)_terrain);
                };
                button.OnUpdate += (uie) =>
                {
                    Button button = uie as Button;
                    int _terrain = button.data[0];
                    button.Text = $"{CoinPlayer.GetPoints((Terrain)_terrain)}";
                    button.Recalculate();
                };
                MenuView.Append2List(button);
            }

            // 物品面板
            ItemView = new(224 * 2 + 15 + 30, MenuView.Height() - MenuView.HPadding());
            ItemView.Top.Pixels = MenuView.Top();
            ItemView.Left.Pixels = MenuView.Width() + 10f;
            MainPanel.Append(ItemView);

            MainPanel.Width.Pixels = ItemView.Right() + MainPanel.HPadding();
            MainPanel.Height.Pixels = MenuView.Top() + MenuView.Height() + MainPanel.VPadding();

            // 在UI里面添加 Item
            ModifyTerrain(Terrain.SenLin);
        }

        public void ModifyTerrain(Terrain terrain)
        {
            this.terrain = terrain;
            List<ItemData> itemData = UISystem.TerrainDatas[(int)terrain].items;
            // 判断有没有数据
            if (itemData.Count > 0)
            {
                ItemView.Clear();
                for (int i = 0; i < itemData.Count; i++)
                {
                    DisplaySlot displaySlot = new(new Item(itemData[i].id), itemData[i].value, itemData[i].mode, terrain);
                    ItemView.Append2List(displaySlot);
                }
                ItemView.Recalculate();
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            dragging = false;
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (MainPanel.IsMouseHovering && !MenuView.IsMouseHovering && !ItemView.IsMouseHovering && !Close.IsMouseHovering)
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
            // 修改设置中的价格倍率刷新商店
            if (PointConfig.Instance.PointMultiplier != OldPriceMultiplier)
            {
                OldPriceMultiplier = PointConfig.Instance.PointMultiplier;
                ModifyTerrain(terrain);
            }

            bool flag = false;
            base.Update(gameTime);
            if (dragging)
            {
                MainPanel.SetPos(Main.mouseX - offset.X, Main.mouseY - offset.Y);
                flag = true;
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
                flag = true;
            }
            if (flag)
                MainPanel.Recalculate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (MainPanel.IsMouseHovering)
                PlayerInput.LockVanillaMouseScroll("ImproveGame: BigBagGUI");
        }
    }
}
