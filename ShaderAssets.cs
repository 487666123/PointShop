using PointShop.Common.Animations;

namespace PointShop
{
    public class ShaderAssets : ModSystem
    {
        public static Effect Cross { get; private set; }
        public static Effect RoundedRectangle { get; private set; }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            Cross = MyUtils.GetEffect("Cross").Value;
            RoundedRectangle = MyUtils.GetEffect("RoundedRectangle").Value;
        }

        public override void Unload()
        {
            if (Main.dedServ)
                return;
            
            Cross = RoundedRectangle = null;
        }
    }
}