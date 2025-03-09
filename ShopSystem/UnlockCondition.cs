namespace PointShop.ShopSystem;

public class UnlockCondition(Mod mod, string name, Asset<Texture2D> icon)
{
    public readonly Mod Mod = mod;
    public string Name { get; set; } = name;
    public Asset<Texture2D> Icon { get; } = icon;
    public string DisplayName => Language.GetText($"Mods.{Mod.Name}.UnlockCondition.{Name}.DisplayName").Value;
    public string Description => Language.GetText($"Mods.{Mod.Name}.UnlockCondition.{Name}.Description").Value;

    private bool _isUnlock;
    public bool IsUnlock
    {
        get => _isUnlock;
        set
        {
            if (_isUnlock == value) return;
            _isUnlock = value;
            StateChanged.Raise(new EventArgs<bool>(value));
        }
    }

    public WeakEventManager<EventArgs<bool>> StateChanged = new();

    public virtual void Update(GameTime gameTime)
    {
        StateChanged.Update(gameTime);
    }
}

public class SimpleUnlockCondition(Mod mod, string name, Asset<Texture2D> icon, Func<bool> condition) : UnlockCondition(mod, name, icon)
{
    public Func<bool> Condition { get; set; } = condition;

    public override void Update(GameTime gameTime)
    {
        IsUnlock = Condition?.Invoke() ?? true;
        base.Update(gameTime);
    }
}
