using System.Collections.Generic;
using PointShop.Entitys;
using PointShop.Interface.BaseView;
using PointShop.Interface.SUIElements;

namespace PointShop.Interface.GUI.PointShopViews;

public class CardGrid : ScrollView
{
    private static readonly Vector2 CardSize;
    private static readonly Vector2 CardSpacing;

    static CardGrid()
    {
        CardSize = new Vector2(216f, 68f);
        CardSpacing = new Vector2(8f);
    }

    public CardGrid()
    {
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

    public void SetItems(Terrain terrain)
    {
        List<ItemData> itemData = UISystem.TerrainDatas[(int)terrain].Items;

        // 判断有没有数据
        if (itemData.Count < 1)
        {
            return;
        }

        ListView.RemoveAllChildren();

        ShopCard card = null;
        foreach (ItemData data in itemData)
        {
            card = new ShopCard(new Item(data.Id), data.Value, data.Mode, terrain);
            card.Join(ListView);
        }

        ListView.SetSizePixels(TotalSize(card!.GetSizePixel(), card!.Spacing, 2,
            itemData.Count % 2 > 0 ? itemData.Count / 2 + 1 : itemData.Count / 2));
        SetSizePixels(ListView.Width.Pixels + 24f, 350f);
        ScrollBar.SetView(Height.Pixels, ListView.Height.Pixels);

        Recalculate();
    }
}