namespace PointShop.Common.Data
{
    public class TerrainInfo
    {
        public enum Terrain : byte
        {
            SenLin,
            TianKong,
            DongXue,
            DiYu,
            CongLin,
            HaiYang,
            XueDi,
            ShaMo,
            FuHua,
            XingHong,
            ShenSheng,
            DiLao,
            Count
        }

        public static readonly Color[] TerrainColor = new Color[]
        {
            new(28, 216, 94),
            new(255, 225, 143),
            new(128, 77, 57),
            new(160, 35, 0),
            new(121, 176, 24),
            new(48, 91, 191),
            new(51, 211, 255),
            new(153, 65, 31),
            new(83, 45, 117),
            new(128, 32, 32),
            new(0, 167, 209),
            new(35, 71, 117),
            Color.White
        };
    }
}
