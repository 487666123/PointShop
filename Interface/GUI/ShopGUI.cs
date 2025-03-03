// using PointShop.Helpers.Extensions;
// using PointShop.Interface.Common;
// using PointShop.Interface.GUI.PointShopViews;
// using PointShop.Interface.SUIElements;
// using Terraria.GameInput;
//
// namespace PointShop.Interface.GUI
// {
//     internal class ShopGUI : UIState
//     {
//         public static bool Visible;
//         public static float PointMultiplier;
//
//         private static readonly Vector2 GeneralSpacing;
//         private static readonly Vector2 CardSize;
//
//         static ShopGUI()
//         {
//             GeneralSpacing = new Vector2(8f);
//             CardSize = new Vector2(216f, 68f);
//         }
//
//         public SUIPanel MainPanel;
//         private View _titlePanel;
//         private View _contentPanel;
//         private SUITitle _title;
//         private SUICross _cross;
//         private MenuGrid _menuGrid;
//         private ProductCardGrid _productCardGrid;
//
//         public override void OnInitialize()
//         {
//             MainPanel = new SUIPanel(UIColor.PanelBg, UIColor.PanelBorder)
//             {
//                 Draggable = true,
//                 Shadow = 40f,
//                 ShadowExtraSize = 40f,
//                 ShadowColor = UIColor.PanelBorder * 0.5f
//             };
//             MainPanel.SetPadding(0f);
//             MainPanel.SetPosPixels(UIPlayerData.Local.PointShopPos);
//             MainPanel.Join(this);
//
//             _titlePanel = new View()
//             {
//                 DragIgnore = true,
//                 BgColor = UIColor.TitleBg,
//                 Border = 2f,
//                 BorderColor = UIColor.PanelBorder,
//                 Rounded = new Vector4(10f, 10f, 0f, 0f),
//                 Width = new StyleDimension(0f, 1f),
//                 Height = new StyleDimension(50f, 0f)
//             };
//             _titlePanel.SetPadding(0f);
//             _titlePanel.Join(MainPanel);
//
//             _title = new SUITitle(MyUtils.GetText("Config.PointsStore"), 0.5f)
//             {
//                 VAlign = 0.5f
//             };
//             _title.Join(_titlePanel);
//
//             _cross = new SUICross(24f)
//             {
//                 HAlign = 1f,
//                 VAlign = 0.5f,
//                 Rounded = new Vector4(0f, 10f, 0f, 0f),
//                 BorderColor = UIColor.PanelBorder,
//                 BgColor = UIColor.TitleBg
//             };
//             _cross.OnLeftClick += (_, _) => Visible = !Visible;
//             _cross.Join(_titlePanel);
//
//             _contentPanel = new View
//             {
//                 BorderColor = UIColor.PanelBorder,
//                 LayoutMode = LayoutMode.Vertical
//             };
//             _contentPanel.SetPadding(10f);
//             _contentPanel.Join(MainPanel);
//
//             // 菜单面板
//             _menuGrid = new MenuGrid();
//             _menuGrid.Join(_contentPanel);
//
//             // 物品面板
//             _productCardGrid = new ProductCardGrid
//             {
//                 LayoutMode = LayoutMode.Horizontal,
//                 Spacing = 10f.Xy()
//             };
//             _menuGrid.SetMenu(_productCardGrid.SetItems);
//             _productCardGrid.SetItems(default);
//             _productCardGrid.Join(_contentPanel);
//
//             // 一下是设置自动技术大小属性。设置了就意味着大小不会再改变了。
//             _contentPanel.InnerPixel =
//                 () => new Vector2(_productCardGrid.RightPixels(), _productCardGrid.BottomPixels());
//             MainPanel.InnerPixel = () => new Vector2(_contentPanel.RightPixels(), _contentPanel.BottomPixels());
//         }
//
//         public override void Update(GameTime gameTime)
//         {
//             base.Update(gameTime);
//
//             if (MainPanel.IsMouseHovering)
//             {
//                 PlayerInput.LockVanillaMouseScroll("PointShop: PointShop GUI");
//             }
//         }
//     }
// }