using SilkyUIFramework.BasicComponents;
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
        if (!PointShopSystem.TryGetGameEnvironment(CurrentEnvironmentName, out var environment)) return;

        ShopItemTableScrollView.Container.RemoveAllChildren();

        var items = environment.ShopItemList;

        foreach (var item in items)
        {
            if (!ShopItemFilters(item)) continue;

            if (item is SimpleShopItem simpleShopItem)
            {
                ShopItemTableScrollView.Container.AppendChild(new SUISimpleShopItem(simpleShopItem));
                //new SUISimpleShopItem(simpleShopItem).Join(ShopItemTable.Container);
            }
            else
            {
                ShopItemTableScrollView.Container.AppendChild(new SUIShopItemComponent(item));
                // new SUIShopItemComponent(item).Join(ShopItemTable.Container);
            }
        }
    }
}