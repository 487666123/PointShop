using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace PointShop.Commons;

public class PlayerEnvironmentManager
{
    public static PlayerEnvironmentManager Instance { get; } = new();
    private PlayerEnvironmentManager() { }

    public IEnumerable<PlayerEnvironment> GetEnvironments() => _environments;

    private readonly List<PlayerEnvironment> _environments = [];
    private readonly Dictionary<string, PlayerEnvironment> _registry = [];

    public bool Register(string name, Func<bool> condition, int priority,
        PlayerEnvironmentType type = PlayerEnvironmentType.Average)
    {
        var environment = new PlayerEnvironment(name, condition, priority, type);
        if (!_registry.TryAdd(name, environment))
            return false;

        _environments.Add(environment);
        _environments.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        return true;
    }

    public PlayerEnvironment GetEnvironmentData(string name)
    {
        return _registry.GetValueOrDefault(name);
    }
}