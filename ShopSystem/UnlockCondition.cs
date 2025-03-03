namespace PointShop.ShopSystem;

public class UnlockCondition(Mod mod, string name, Asset<Texture2D> icon)
{
    public readonly Mod Mod = mod;
    public string Name { get; set; } = name;
    public Asset<Texture2D> Icon { get; } = icon;
    public string DisplayName => Language.GetText($"Mods.{Mod.Name}.UnlockCondition.{Name}.DisplayName").Value;
    public string Description => Language.GetText($"Mods.{Mod.Name}.UnlockCondition.{Name}.Description").Value;

    public virtual bool IsUnlock()
    {
        return true;
    }
}

public class SimpleUnlockCondition(Mod mod, string name, Asset<Texture2D> icon, Func<bool> condition) : UnlockCondition(mod, name, icon)
{
    public Func<bool> Condition { get; set; } = condition;

    public override bool IsUnlock()
    {
        if (Condition != null) return Condition();
        return base.IsUnlock();
    }
}
