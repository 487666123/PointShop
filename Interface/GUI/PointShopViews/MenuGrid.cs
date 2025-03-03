// using System.Collections.Generic;
// using PointShop.Common.Players;
// using PointShop.Entitys;
// using PointShop.Interface.BaseView;
//
// namespace PointShop.Interface.GUI.PointShopViews;
//
// public class MenuGrid : ScrollView
// {
//     private static readonly Vector2 MenuButtonSize;
//     private static readonly Vector2 MenuButtonSpacing;
//
//     static MenuGrid()
//     {
//         MenuButtonSize = new Vector2(110f, 46f);
//         MenuButtonSpacing = new Vector2(8f);
//     }
//
//     public MenuGrid()
//     {
//         SetSizePixels(134f, 350f);
//
//         ScrollBar.SetPadding(4f);
//         ScrollBar.Width.Pixels = 16f;
//     }
//
//     public void SetMenu(Action<Terrain> modifyItems)
//     {
//         List<TerrainData> terrainData = UISystem.TerrainDatas;
//         List<Texture2D> icons = UISystem.Icons;
//
//         ListView.SetSizePixels(TotalSize(MenuButtonSize, MenuButtonSpacing, 1, terrainData.Count));
//         ScrollBar.SetView(Height.Pixels, ListView.Height.Pixels + 2);
//
//         for (int i = 0; i < terrainData.Count; i++)
//         {
//             var button = new GeneralButton(icons[i], $"{terrainData[i].Name}")
//             {
//                 LayoutMode = LayoutMode.Vertical,
//                 Spacing = MenuButtonSpacing
//             };
//             button.SetSizePixels(MenuButtonSize);
//             int i1 = i;
//             button.RealTimeText = () => $"{CoinPlayer.GetPoints((Terrain)i1)}";
//             button.OnLeftClick += (_, _) => modifyItems((Terrain)i1);
//             button.Join(ListView);
//         }
//     }
// }