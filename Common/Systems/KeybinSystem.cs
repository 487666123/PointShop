using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

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
