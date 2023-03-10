using System.Collections.Generic;
using PointShop.Entitys;
using PointShop.Interface.BaseView;

namespace PointShop.Interface.GUI.PointShopViews;

public class ProductCardGrid : ScrollView
{
    public ProductCardGrid()
    {
        ScrollBar.SetPadding(4f);
        ScrollBar.Width.Pixels = 16f;
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

        ProductCard card = null;
        foreach (var data in itemData)
        {
            card = new ProductCard(new Item(data.Id), data.Value, data.Mode, terrain);
            card.Join(ListView);
        }

        ListView.SetSizePixels(TotalSize(card!.GetSizePixel(), card!.Spacing, 2,
            itemData.Count % 2 > 0 ? itemData.Count / 2 + 1 : itemData.Count / 2));
        SetSizePixels(ListView.Width.Pixels + 24f, 350f);
        ScrollBar.SetView(Height.Pixels, ListView.Height.Pixels + 2);

        Recalculate();
    }
}