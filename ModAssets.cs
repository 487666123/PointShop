using PointShop.Interface;

namespace PointShop
{
    public class ModAssets : ModSystem
    {
        public static Effect Fork;
        public static Effect Box;
        public static Effect RoundRect;
        public static Effect RoundRectNoBorder;

        public override void Load()
        {
            Fork = ModHelper.GetEffect(nameof(Fork)).Value;
            Box = ModHelper.GetEffect(nameof(Box)).Value;
            RoundRect = ModHelper.GetEffect(nameof(RoundRect)).Value;
            RoundRectNoBorder = ModHelper.GetEffect(nameof(RoundRectNoBorder)).Value;
        }

        public override void Unload()
        {
            Fork = null;
            Box = null;
            RoundRect = null;
            RoundRectNoBorder = null;
        }
    }
}
