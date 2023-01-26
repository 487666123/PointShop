using System.Collections.Generic;
using PointShop.Common.Players;
using PointShop.Entitys;
using PointShop.Interface.BaseView;
using PointShop.Interface.Common;
using PointShop.Interface.GUI.PointShopViews;
using PointShop.Interface.SUIElements;
using PointShop.Interface.UIElements;
using Terraria.GameInput;

namespace PointShop.Interface.GUI
{
    internal class PointShopGUI : UIState
    {
        public static bool Visible;
        public static float PointMultiplier;

        private static readonly Vector2 GeneralSpacing;
        private static readonly Vector2 CardSize;

        static PointShopGUI()
        {
            GeneralSpacing = new Vector2(8f);
            CardSize = new Vector2(216f, 68f);
        }

        public SUIPanel MainPanel;
        private View _titlePanel;
        private View _contentPanel;
        private SUITitle _title;
        private SUICross _cross;
        private MenuGrid _menuGrid;
        private CardGrid _cardGrid;

        public override void OnInitialize()
        {
            MainPanel = new SUIPanel(UIColor.PanelBg, UIColor.PanelBorder)
            {
                Draggable = true,
                Shadow = 40f,
                ShadowColor = UIColor.PanelBorder * 0.5f
            };
            MainPanel.SetPadding(0f);
            MainPanel.SetPosPixels(Main.LocalPlayer.GetModPlayer<UIPlayerData>().PointShopPos);
            MainPanel.Join(this);

            _titlePanel = new View()
            {
                DragIgnore = true,
                BgColor = UIColor.TitleBg2,
                Border = 2f,
                BorderColor = UIColor.PanelBorder,
                Rounded = new Vector4(10f, 10f, 0f, 0f),
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(50f, 0f)
            };
            _titlePanel.SetPadding(0f);
            _titlePanel.Join(MainPanel);

            _title = new SUITitle(MyUtils.GetText("Config.PointExchange"), 0.5f)
            {
                VAlign = 0.5f
            };
            _title.Join(_titlePanel);

            _cross = new SUICross(24f)
            {
                HAlign = 1f,
                VAlign = 0.5f,
                Rounded = new Vector4(0f, 10f, 0f, 0f),
                BorderColor = UIColor.PanelBorder,
                BgColor = UIColor.TitleBg2
            };
            _cross.OnClick += (_, _) => Visible = !Visible;
            _cross.Join(_titlePanel);

            _contentPanel = new View
            {
                BorderColor = UIColor.PanelBorder,
                Relative = RelativeMode.Vertical
            };
            _contentPanel.SetPadding(10f);
            _contentPanel.Join(MainPanel);

            // 菜单面板
            _menuGrid = new MenuGrid();
            _menuGrid.Join(_contentPanel);

            // 物品面板
            _cardGrid = new CardGrid
            {
                Relative = RelativeMode.Horizontal,
                Spacing = 10f.Xy()
            };
            _menuGrid.SetMenu(_cardGrid.SetItems);
            _cardGrid.SetItems(default);
            _cardGrid.Join(_contentPanel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (MainPanel.IsMouseHovering)
            {
                PlayerInput.LockVanillaMouseScroll("PointShop: PointShop GUI");
            }

            bool recalculate = false;

            Vector2 contentSize = new Vector2(_cardGrid.RightPixels(), _cardGrid.BottomPixels());
            if (_contentPanel.GetInnerPixel() != contentSize)
            {
                recalculate = true;
                _contentPanel.SetInnerPixels(contentSize);
            }

            Vector2 panelSize = new Vector2(_contentPanel.RightPixels(), _contentPanel.BottomPixels());
            if (MainPanel.GetInnerPixel() != panelSize)
            {
                recalculate = true;
                MainPanel.SetInnerPixels(panelSize);
            }

            if (recalculate)
                MainPanel.Recalculate();
        }
    }
}