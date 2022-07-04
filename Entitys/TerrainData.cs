using System.Collections.Generic;

namespace PointShop.Entitys
{
    public class TerrainData
    {
        public string name;
        public string image;
        public List<ItemData> items;

        public TerrainData(string name, string image, List<ItemData> items)
        {
            this.name = name;
            this.image = image;
            this.items = items;
        }
    }

    public class ItemData
    {
        public int id;
        public int value;
        public int mode;

        public ItemData(int id, int value, int mode)
        {
            this.id = id;
            this.value = value;
            this.mode = mode;
        }
    }
}
