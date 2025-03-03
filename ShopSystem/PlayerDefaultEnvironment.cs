namespace PointShop.Commons;

public class PlayerDefaultEnvironment : ModSystem
{
    public override void Load()
    {
        if (PlayerEnvironmentManager.Instance is not { } manager) return;

        #region 森林 正常洞穴 天空 地狱 海洋 地牢

        // 森林
        manager.Register("Forest", () => true, 0, PlayerEnvironmentType.Free);

        // 正常洞穴 (不包括那些特殊环境)
        manager.Register("Caverns",
            () => Main.LocalPlayer is { ZoneNormalCaverns: true }, 1000, PlayerEnvironmentType.Free);

        // 天空
        manager.Register("Sky",
            () => Main.LocalPlayer is { ZoneSkyHeight: true }, 2000, PlayerEnvironmentType.Unique);

        // 地狱
        manager.Register("Underworld",
            () => Main.LocalPlayer is { ZoneUnderworldHeight: true }, 2000, PlayerEnvironmentType.Unique);

        // 海洋
        manager.Register("Beach",
            () => Main.LocalPlayer is { ZoneBeach: true }, 2000, PlayerEnvironmentType.Unique);

        // 地牢
        manager.Register("Dungeon",
            () => Main.LocalPlayer is { ZoneDungeon: true }, 2000, PlayerEnvironmentType.Unique);

        #endregion

        #region 大理石 花岗岩 发光蘑菇

        // 大理石
        manager.Register("Marble",
            () => Main.LocalPlayer is { ZoneMarble: true }, 2000);

        // 花岗岩
        manager.Register("Granite",
            () => Main.LocalPlayer is { ZoneGranite: true }, 2000);

        // 发光蘑菇
        manager.Register("Glowshroom",
            () => Main.LocalPlayer is { ZoneGlowshroom: true }, 2000);

        #endregion

        #region 雪地 沙漠 丛林 神圣 腐化 猩红

        // 雪地
        manager.Register("Snow",
            () => Main.LocalPlayer is { ZoneSnow: true }, 2000);

        // 沙漠
        manager.Register("Desert",
            () => Main.LocalPlayer is { ZoneDesert: true }, 2000);

        // 丛林
        manager.Register("Jungle",
            () => Main.LocalPlayer is { ZoneJungle: true }, 2000);

        // 腐化
        manager.Register("Corrupt",
            () => Main.LocalPlayer is { ZoneCorrupt: true }, 2000);

        // 猩红
        manager.Register("Crimson",
            () => Main.LocalPlayer is { ZoneCrimson: true }, 2000);

        // 神圣
        manager.Register("Hallow",
            () => Main.LocalPlayer is { ZoneHallow: true }, 2000);

        #endregion
    }
}