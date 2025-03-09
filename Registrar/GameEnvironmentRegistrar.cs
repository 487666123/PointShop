namespace PointShop.Registrar;

public class GameEnvironmentRegistrar : ModSystem
{
    /// <summary>
    /// 洞穴 大理石 花岗岩 发光蘑菇
    /// </summary>
    private static bool IsCaverns(Player player)
    {
        return player is
        { ZoneDirtLayerHeight: true } or
        { ZoneRockLayerHeight: true };
        //{ ZoneNormalCaverns: true } or
        //{ ZoneMarble: true } or
        //{ ZoneGranite: true } or
        //{ ZoneGlowshroom: true };
    }

    public override void Load()
    {
        #region 森林 正常洞穴 天空 海洋

        // 天空
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Sky, "Sky",
            player => player is { ZoneSkyHeight: true }, 2000, new(255, 225, 143), GameEnvironmentType.Average);

        // 森林
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Surface, "Forest",
            player => player is { ZoneForest: true }, 2000, new(28, 216, 94));

        // 正常洞穴 (不包括那些特殊环境)
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Caverns, "Caverns", IsCaverns, 2000, new(128, 77, 57));

        // 海洋
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Beach, "Beach",
            player => player is { ZoneBeach: true }, 2000, new(48, 91, 191), GameEnvironmentType.Average);

        #endregion

        #region 大理石 花岗岩 发光蘑菇

        // 大理石
        // PointShopSystem.Register(Mod, "Marble",
        //     () => Main.LocalPlayer is { ZoneMarble: true }, 2000, Color.Red);

        // // 花岗岩
        // PointShopSystem.Register(Mod, "Granite",
        //     () => Main.LocalPlayer is { ZoneGranite: true }, 2000, Color.Red);

        // // 发光蘑菇
        // PointShopSystem.Register(Mod, "Glowshroom",
        //     () => Main.LocalPlayer is { ZoneGlowshroom: true }, 2000, Color.Red);

        #endregion

        #region 雪地 沙漠 丛林 神圣 腐化 猩红 地牢 地狱

        // 雪地
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Snow, "Snow",
            player => player is { ZoneSnow: true }, 2000, new(51, 211, 255));

        // 沙漠
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Desert, "Desert",
            player => player is { ZoneDesert: true }, 2000, new(153, 65, 31));

        // 丛林
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Jungle, "Jungle",
            player => player is { ZoneJungle: true }, 2000, new(121, 176, 24));

        // 腐化
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Corrupt, "Corrupt",
            player => player is { ZoneCorrupt: true }, 2000, new(83, 45, 117));

        // 猩红
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Crimson, "Crimson",
            player => player is { ZoneCrimson: true }, 2000, new(128, 32, 32));

        // 神圣
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Hallow, "Hallow",
            player => player is { ZoneHallow: true }, 2000, new(0, 167, 209));

        // 地牢
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Dungeon, "Dungeon",
            player => player is { ZoneDungeon: true }, 2000, new(35, 71, 117), GameEnvironmentType.Average);

        // 地狱
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Underworld, "Underworld",
            player => player is { ZoneUnderworldHeight: true }, 2000, new(160, 35, 0), GameEnvironmentType.Average);

        #endregion

        // 虚无
        PointShopSystem.RegisterGameEnvironment(Mod, ModAsset.Void, "Void",
            player => true, 1000, new(28, 216, 94), GameEnvironmentType.Void);
    }
}