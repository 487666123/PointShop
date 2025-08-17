using PointShop.Items;
using SilkyUIFramework.Attributes;
using SilkyUIFramework.Extensions;

namespace PointShop.UserInterfaces;

[XmlElementMapping("PointShopFooter")]
public class PointShopFooter : UIElementGroup
{
    public static bool DisplayLinks { get; set; } = true;

    public UIElementGroup BalanceContainer { get; }
    public UITextView EnvironmentName { get; }
    public UITextView Balance { get; }
    public UIView Support { get; }
    public UITextView OfficialGroupLink { get; }
    public UITextView OnlineGroupLink { get; }
    public UITextView DonationLink { get; }

    public PointShopFooter()
    {
        LayoutType = LayoutType.Flexbox;
        FlexDirection = FlexDirection.Row;
        MainAlignment = MainAlignment.Start;

        BackgroundColor = Color.Black * 0.25f;
        BorderRadius = new Vector4(0f, 0f, 6f, 6f);

        SetGap(8f);
        SetSize(0f, 30f, 1f);
        SetPadding(12f, 0f);

        BalanceContainer = new UIElementGroup
        {
            LayoutType = LayoutType.Flexbox,
            FlexDirection = FlexDirection.Row,
            FitWidth = true,
        }.Join(this);
        BalanceContainer.SetHeight(0, 1f);

        EnvironmentName = new UITextView
        {
            TextAlign = new Vector2(0.5f, 0.5f),
            TextScale = 0.8f,
            TextColor = Color.White,
            FitHeight = false,
        }.Join(BalanceContainer);
        EnvironmentName.SetHeight(0f, 1f);

        var coinSlot = new SUIItemSlot
        {
            Item = new Item(ModContent.ItemType<PointCoin>()),
            ItemAlign = new Vector2(0.5f),
            ItemScale = 0.8f,
            BackgroundColor = Color.Transparent,
            BorderColor = Color.Transparent,
            ItemInteractive = false,
        }.Join(BalanceContainer);
        coinSlot.SetSize(24f, 0f, 0f, 1f);

        // 价格
        Balance = new UITextView()
        {
            TextAlign = new Vector2(0.5f, 0.5f),
            TextScale = 0.8f,
            TextColor = Color.White,
            FitHeight = false,
        }.Join(BalanceContainer);
        Balance.SetHeight(0f, 1f);

        // 支撑
        Support = new UIView
        {
            FlexGrow = 1f,
        }.Join(this);
        Support.SetHeight(0f, 1f);

        if (DisplayLinks && bool.TryParse(LanguageHelper.GetTextByPointShop("ShowGroupLinks").Value, out var showGroupLinks) && showGroupLinks)
        {
            DonationLink = new UITextView
            {
                Text = $"爱发电",
                TextScale = 0.75f,
                TextAlign = new Vector2(0f, 0.5f),
                FitHeight = false,
            }.Join(this);
            DonationLink.SetHeight(0, 1f);
            DonationLink.OnUpdateStatus += delegate
            {
                DonationLink.TextBorderColor = DonationLink.HoverTimer.Lerp(Color.Black, SUIColor.Highlight);
            };
            DonationLink.DrawAction += delegate
            {
                if (DonationLink.IsMouseHovering) Main.hoverItemName = "可在配置中关闭显示";
            };
            DonationLink.LeftMouseDown += delegate
            {
                Utils.OpenToURL("https://afdian.com/a/tMLZero");
            };

            OnlineGroupLink = new UITextView
            {
                Text = $"联机群",
                TextScale = 0.75f,
                TextAlign = new Vector2(0f, 0.5f),
                FitHeight = false,
            }.Join(this);
            OnlineGroupLink.SetHeight(0, 1f);
            OnlineGroupLink.OnUpdateStatus += delegate
            {
                OnlineGroupLink.TextBorderColor = OnlineGroupLink.HoverTimer.Lerp(Color.Black, SUIColor.Highlight);
            };
            OnlineGroupLink.DrawAction += delegate
            {
                if (OnlineGroupLink.IsMouseHovering) Main.hoverItemName = "可在配置中关闭显示";
            };
            OnlineGroupLink.LeftMouseDown += delegate
            {
                Utils.OpenToURL("https://qm.qq.com/q/CQsK9QEW78");
            };

            OfficialGroupLink = new UITextView
            {
                Text = $"反馈群",
                TextScale = 0.75f,
                TextAlign = new Vector2(0f, 0.5f),
                FitHeight = false,
            }.Join(this);
            OfficialGroupLink.SetHeight(0, 1f);
            OfficialGroupLink.OnUpdateStatus += delegate
            {
                OfficialGroupLink.TextBorderColor = OfficialGroupLink.HoverTimer.Lerp(Color.Black, SUIColor.Highlight);
            };
            OfficialGroupLink.DrawAction += delegate
            {
                if (OfficialGroupLink.IsMouseHovering) Main.hoverItemName = "可在配置中关闭显示";
            };
            OfficialGroupLink.LeftMouseDown += delegate
            {
                Utils.OpenToURL("https://qm.qq.com/q/Vz7DmEOaQe");
            };
        }

        // 版本号
        var versionText = new UITextView
        {
            Text = $"{LanguageHelper.GetTextByPointShop("DisplayName")} {ModContent.GetInstance<PointShop>().Version}",
            TextScale = 0.75f,
            TextAlign = new Vector2(0f, 0.5f),
            FitHeight = false,
        }.Join(this);
        versionText.SetHeight(0, 1f);
        versionText.OnUpdateStatus += delegate
        {
            versionText.TextBorderColor = versionText.HoverTimer.Lerp(Color.Black, SUIColor.Highlight);
        };
        versionText.LeftMouseDown += delegate
        {
            Utils.OpenToURL("https://gitee.com/MyGoold/exchange-item");
        };
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
    }
}