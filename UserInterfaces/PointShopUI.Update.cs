using SilkyUIFramework.Common.Tweening;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

public partial class PointShopUI
{
    protected override void Update(GameTime gameTime)
    {
        UpdatePoints();
        base.Update(gameTime);
    }

    private double _lastPoints;
    public void UpdatePoints()
    {
        if (!PointShopSystem.TryGetGameEnvironment(CurrentEnvironmentName, out var environment)) return;

        var displayName = environment.DisplayName;
        if (ShopFooter.EnvironmentName.Text != displayName)
        {
            ShopFooter.EnvironmentName.Text = displayName;
        }

        var points = environment.GetPlayerPoints();
        if (_lastPoints != points)
        {
            ShopFooter.Balance.Text = $"{points:#,##0}";
            _lastPoints = points;
        }
    }

    public void UpdateMenuList()
    {
        var environments = PointShopSystem.Environments;

        for (int i = 0; i < environments.Count; i++)
        {
            var environment = environments[i];
            var button = new SUIMenuComponent(environment).Join(MenuListScrollView.Container);
            button.OnEnvironmentChanged(null, CurrentEnvironmentName);

            button.LeftMouseDown += (_, _) =>
            {
                CurrentEnvironmentName = environment.Name;
            };

            if (i + 1 != environments.Count)
            {
                SUIDividingLine.Horizontal(SUIColor.Border * 0.75f).Join(MenuListScrollView.Container);
            }
        }
    }

    private string _keywords = "";
    private bool ShopItemFilters(ShopItem shopItem) => shopItem.DisplayName.Contains(_keywords.Trim());
    public static bool ShopItemTableIsDirty { get; set; } = true;

    /// <summary>
    /// 更新物品表格
    /// </summary>
    public void UpdateShopItemTable()
    {
        if (!ShopItemTableIsDirty) return;
        ShopItemTableIsDirty = false;

        if (!PointShopSystem.TryGetGameEnvironment(CurrentEnvironmentName, out var environment)) return;

        ShopItemTableScrollView.Container.RemoveAllChildren();

        var items = environment.ShopItemList;

        var tween = CreateTween().Parallel().SetTrans(TransitionType.Expo).SetEase(EaseType.Out);
        var delay = 0f;
        foreach (var item in items.Where(ShopItemFilters))
        {
            var view = (item is SimpleShopItem simpleShopItem) ?
                new SUISimpleShopItem(simpleShopItem) :
                new SUIShopItemComponent(item);

            ShopItemTableScrollView.Container.AddChild(view);

            view.SetTop(pixels: 50f);
            tween.MemberTo(view, nameof(view.Top), new Anchor(), 0.2f).SetDelay(delay);
            delay += 0.005f;
        }
    }
}