using PointShop.Common.Players;
using PointShop.Common.Systems;
using PointShop.Entitys;
using PointShop.Interface.UIElements;
using PointShop.ModUI.UIElements;
using System.Collections.Generic;
using Terraria.GameInput;

namespace PointShop.Interface.GUI
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
        public SUIPanel MainPanel;
        public ScrollView MenuView;
        public ScrollView ItemView;

        private readonly Color background = new(44, 57, 105, 160);
        public PointShopGUI()
        {
            screenWidth = Main.screenWidth;
            screenHeight = Main.screenHeight;

            MainPanel = new(Color.Black, background);
            MainPanel.SetPadding(14f);
            MainPanel.SetPos((Main.ScreenSize.ToVector2() - MainPanel.Size()) / 2f);
            Append(MainPanel);

            UIText = new(ModHelper.GetText("Config.PointExchange"), 0.5f, true);
            UIText.SetPos(10f, 10f);
            MainPanel.Append(UIText);

            ImageButton = new(ModHelper.GetTexture("Button_Close"))
            {
                HAlign = 1f
            };
            ImageButton.Left.Pixels = -10f;
            ImageButton.OnClick += (evt, uie) => Visible = !Visible;
            MainPanel.Append(ImageButton);

            // 菜单面板
            MenuView = new(190, 50 * 5 + 10 * 4 + 20);
            MenuView.Top.Pixels += ImageButton.Height() + 15f;
            MainPanel.Append(MenuView);

            // 菜单按钮按钮
            for (int i = 0; i < UISystem.TerrainDatas.Count; i++)
            {
                Button button = new(UISystem.icon[i].Value, $"{UISystem.TerrainDatas[i].name}");
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
                    button.SetText($"{UISystem.TerrainDatas[_terrain].name}: {CoinPlayer.GetPoints((Terrain)_terrain)}");
                    button.Recalculate();
                };
                MenuView.Append2List(button);
            }

            // 物品面板
            int num = 6;
            ItemView = new(52 * num + 15 * (num - 1) + 30, MenuView.Height() - MenuView.HPadding(), 10, 15);
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
            List<ItemData> itemData = UISystem.TerrainDatas[(int)terrain].items;
            // 判断有没有数据
            if (itemData.Count > 0)
            {
                ItemView.Clear();
                for (int i = 0; i < itemData.Count; i++)
                {
                    SingleItemSlot ItemSlot = new(itemData[i].id, itemData[i].value, terrain, itemData[i].mode);

                    MiniButton button = new($"{itemData[i].value}");
                    button.data[0] = (int)terrain;
                    button.Width.Pixels = ItemSlot.Width();
                    button.Top.Pixels = ItemSlot.Bottom() + 5f;
                    button.OnClick += (_, _) => ItemSlot.Play();

                    UIElement uie = new();
                    uie.Width.Pixels = ItemSlot.Width();
                    uie.Height.Pixels = button.Bottom();
                    uie.Append(ItemSlot);
                    uie.Append(button);

                    ItemView.Append2List(uie);
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
