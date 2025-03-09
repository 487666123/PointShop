using SilkyUIFramework.BasicComponents;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

public partial class PointShopUI
{
    public override void Update(GameTime gameTime)
    {
        UpdatePoints();
        base.Update(gameTime);

        if (IsLayoutDirty)
        {
            Recalculate();
            IsLayoutDirty = false;
        }
    }

    private double _lastPoints;
    public void UpdatePoints()
    {
        if (!PointShopSystem.TryGetGameEnvironment(CurrentEnvironmentName, out var environment)) return;

        var displayName = environment.DisplayName;
        if (ShopFooter.EnvironmentName.Text != displayName)
        {
            ShopFooter.EnvironmentName.Text = displayName;
            MakeLayoutDirty();
        }

        var points = environment.GetPlayerPoints();
        if (_lastPoints != points)
        {
            ShopFooter.Balance.Text = $"{points:#,##0}";
            _lastPoints = points;
            MakeLayoutDirty();
        }
    }

    public void UpdateMenuList()
    {
        MakeLayoutDirty();
        var environments = PointShopSystem.Environments;

        for (int i = 0; i < environments.Count; i++)
        {
            var environment = environments[i];
            var button = new SUIMenuComponent(environment).Join(MenuList);

            button.OnLeftMouseDown += (_, _) =>
            {
                CurrentEnvironmentName = environment.Name;
                UpdateShopItemTable();
            };

            if (i + 1 != environments.Count)
            {
                SUIDividingLine.Horizontal(SUIColor.Border * 0.75f).Join(MenuList.Container);
            }
        }
    }

    private string _keywords = "";
    private bool ShopItemFilters(ShopItem shopItem) => shopItem.DisplayName.Contains(_keywords.Trim());

    /// <summary>
    /// 更新物品表格
    /// </summary>
    public void UpdateShopItemTable()
    {
        if (!PointShopSystem.TryGetGameEnvironment(CurrentEnvironmentName, out var environment)) return;
        MakeLayoutDirty();

        ShopItemTable.Container.RemoveAllChildren();

        var items = environment.ShopItemList;

        foreach (var item in environment.ShopItemList)
        {
            if (!ShopItemFilters(item)) continue;

            if (item is SimpleShopItem simpleShopItem)
            {
                ShopItemTable.Container.AppendFromView(new SUISimpleShopItem(simpleShopItem));
                // new SUISimpleShopItem(simpleShopItem).Join(ShopItemTable.Container);
            }
            else
            {
                ShopItemTable.Container.AppendFromView(new SUIShopItemComponent(item));
                // new SUIShopItemComponent(item).Join(ShopItemTable.Container);
            }
        }
    }
}