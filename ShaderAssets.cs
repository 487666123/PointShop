using PointShop.Common.Animations;

namespace PointShop
{
    public class ShaderAssets : ModSystem
    {
        public static Effect SDFGraphic { get; private set; }
        public static Effect RoundedRectangle { get; private set; }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            SDFGraphic = MyUtils.GetEffect("SDFGraphic").Value;
            RoundedRectangle = MyUtils.GetEffect("RoundedRectangle").Value;
        }

        public override void Unload()
        {
            if (Main.dedServ)
                return;
            
            SDFGraphic = RoundedRectangle = null;
        }
    }
}