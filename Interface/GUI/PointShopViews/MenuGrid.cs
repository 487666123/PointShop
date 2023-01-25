using System.Collections.Generic;
using PointShop.Common.Players;
using PointShop.Entitys;
using PointShop.Interface.BaseView;
using PointShop.Interface.UIElements;

namespace PointShop.Interface.GUI.PointShopViews;

public class MenuGrid : ScrollView
{
    private static readonly Vector2 MenuButtonSize;
    private static readonly Vector2 MenuButtonSpacing;

    static MenuGrid()
    {
        MenuButtonSize = new Vector2(120f, 46f);
        MenuButtonSpacing = new Vector2(8f);
    }

    public MenuGrid()
    {
        SetSizePixels(144f, 350f);

        ScrollBar.SetPadding(4f);
        ScrollBar.HAlign = 1f;
        ScrollBar.VAlign = 0.5f;
        ScrollBar.Left.Pixels = -2f;
        ScrollBar.Width.Pixels = 16f;
        ScrollBar.Height = new StyleDimension(-4f, 1f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (!(Math.Abs(-ScrollBar.ViewPosition - ListView.Top.Pixels) > 0.000000001f))
        {
            return;
        }

        ListView.Top.Pixels = -ScrollBar.ViewPosition;
        ListView.Recalculate();
    }

    public void SetMenu(Action<Terrain> modifyItems)
    {
        List<TerrainData> terrainData = UISystem.TerrainDatas;
        List<Texture2D> icons = UISystem.Icons;

        ListView.SetSizePixels(TotalSize(MenuButtonSize, MenuButtonSpacing, 1, terrainData.Count));
        ScrollBar.SetView(Height.Pixels, ListView.Height.Pixels);

        for (int i = 0; i < terrainData.Count; i++)
        {
            var button = new MenuButton(icons[i], $"{terrainData[i].Name}")
            {
                Relative = RelativeMode.Vertical,
                Spacing = MenuButtonSpacing
            };
            button.SetSizePixels(MenuButtonSize);
            int i1 = i;
            button.RealTimeText = () => $"{CoinPlayer.GetPoints((Terrain)i1)}";
            button.OnClick += (_, _) => modifyItems((Terrain)i1);
            button.Join(ListView);
        }
    }
}