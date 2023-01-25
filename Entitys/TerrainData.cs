using System.Collections.Generic;

namespace PointShop.Entitys
{
    public class TerrainData
    {
        public readonly string Name;
        public readonly string Image;
        public readonly List<ItemData> Items;

        public TerrainData(string name, string image, List<ItemData> items)
        {
            Name = name;
            Image = image;
            Items = items;
        }
    }

    public class ItemData
    {
        public readonly int Id;
        public readonly int Value;
        public readonly int Mode;

        public ItemData(int id, int value, int mode)
        {
            Id = id;
            Value = value;
            Mode = mode;
        }
    }
}
