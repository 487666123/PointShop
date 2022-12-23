using PointShop.Common.Players;
using PointShop.Interface.UIElements;
using PointShop.UI.Common;
using PointShop.UI.ShopUI;

namespace PointShop.UI.TipUI
{
    public class PointGUI : UIState
    {
        internal static Asset<Texture2D> PlayButton = ModContent.Request<Texture2D>("PointShop/Images/ButtonPlay", AssetRequestMode.ImmediateLoad);
        public static bool Visible => ModHelper.Config.TerrainPanel;

        public SUIPanel MainPanel;
        public UIImage Logo;
        public ModUIText title;
        public UIImageButton button;

        private readonly Color background = new(44, 57, 105, 160);
        public override void OnInitialize()
        {
            RemoveAllChildren();
            // 主面板
            Append(MainPanel = new(Color.Black, background)
            {
                PaddingTop = 0f,
                PaddingBottom = 0f,
                PaddingLeft = 16f,
                PaddingRight = 16f,
                HAlign = 0.5f
            });
            MainPanel.Height.Pixels = 45f;
            MainPanel.Width.Pixels = 200f;
            MainPanel.Top.Pixels = 20f;

            MainPanel.Append(Logo = new(UISystem.Icons[0]) { VAlign = 0.5f });

            MainPanel.Append(title = new("中文:Chinese", Color.White, 0.8f) { VAlign = 0.5f });
            title.Top.Pixels = 1;
            title.Left.Pixels = Logo.Right() + 10f;

            MainPanel.Append(button = new(PlayButton) { VAlign = 0.5f, HAlign = 1f });
            button.OnClick += Button_OnClick;

        }

        private void Button_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            // 控制开关，而不是只开
            PointShopGUI.Visible = !PointShopGUI.Visible;
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer;
            CoinPlayer coinPlayer = player.GetModPlayer<CoinPlayer>();
            Terrain huanJing = CoinPlayer.InWhatTerrain;
            Logo.SetImage(UISystem.Icons[(int)huanJing]);
            title.Left.Set(Logo.Width.Pixels + 10f, 0f);
            title.text = ModHelper.GetText("HuanJingName." + huanJing) + ModHelper.GetText("Hint.Point") + ": " + coinPlayer.Point[(int)huanJing];
            Recalculate();
            
            // 防止点击按键时使用物品
            if (button.IsMouseHovering)
            {
                player.mouseInterface = true;
            }
        }
    }
}
