namespace PointShop.Common.Systems
{
    public class KeybinSystem : ModSystem
    {
        public static ModKeybind RandomBuffKeybind { get; private set; }

        public override void Load()
        {
            RandomBuffKeybind = KeybindLoader.RegisterKeybind(Mod, MyUtils.GetText("Keybin.SuperVault"), "P");
        }

        public override void Unload()
        {
            RandomBuffKeybind = null;
        }
    }
}
