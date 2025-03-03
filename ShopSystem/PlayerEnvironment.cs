using System;
using System.Collections;
using System.Collections.Generic;
using PointShop.ShopSystem;
using PointShop.UserInterfaces;
using Terraria.ModLoader.IO;

namespace PointShop.Commons;

public enum PlayerEnvironmentType
{
    /// <summary>
    /// 任意高于此优先级的环境达成, 此环境都将不获得积分
    /// </summary>
    Free,

    /// <summary>
    /// 平均的
    /// </summary>
    Average,

    /// <summary>
    /// 独享, 直接获取本次的所有积分 (不会影响平分的环境)
    /// </summary>
    Unique,
}

public class PlayerEnvironment(
    string name,
    Func<bool> condition,
    int priority,
    PlayerEnvironmentType type = PlayerEnvironmentType.Average)
{
    /// <summary> 显示名称 </summary>
    public string DisplayName =>
        LanguageHelper.GetTextByPointShop($"Environment.{Name}").Value;

    public PlayerEnvironmentType Type { get; } = type;
    public string Name { get; } = name;
    public int Priority { get; } = priority;

    public Func<bool> Condition { get; } = condition;

    public readonly List<ShopItem> ShopItems = [];

    public override string ToString()
    {
        return $"{Type}: {Name}: {Priority}";
    }
}