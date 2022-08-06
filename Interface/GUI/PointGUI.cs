using Microsoft.Xna.Framework.Graphics;
using PointShop.Common.Players;
using PointShop.Common.Systems;
using PointShop.Helpers;
using PointShop.Interface.UIElements;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PointShop.Interface.GUI
{
    public class PointGUI : UIState
    {
        public static bool Visible => ModHelper.Config.TerrainPanel && !PointShopGUI.Visible;

        public SUIPanel MainPanel;
        public UIImage logo;
        public UIText tileText;
        public UIImageButton button;

        private readonly Color background = new(44, 57, 105, 160);
        public override void OnInitialize()
        {
            RemoveAllChildren();
            // 主面板
            MainPanel = new(Color.Black, background)
            {
                PaddingTop = 0f,
                PaddingBottom = 0f,
                PaddingLeft = 16f,
                PaddingRight = 16f,
                HAlign = 0.5f
            };
            MainPanel.Height.Set(45f, 0f);
            MainPanel.Width.Set(200f, 0f);
            MainPanel.Top.Set(20f, 0f);

            logo = new(UISystem.Icons[0])
            {
                VAlign = 0.5f
            };

            tileText = new("", 0.8f)
            {
                VAlign = 0.5f
            };
            tileText.Left.Set(logo.Width.Pixels + 10f, 0f);

            button = new(ModContent.Request<Texture2D>("PointShop/Images/ButtonPlay", AssetRequestMode.ImmediateLoad))
            {
                VAlign = 0.5f,
                HAlign = 1f
            };
            button.OnClick += Button_OnClick;

            Append(MainPanel);
            MainPanel.Append(logo);
            MainPanel.Append(tileText);
            MainPanel.Append(button);
        }

        private void Button_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            PointShopGUI.Visible = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.myPlayer > -1 && Main.myPlayer < 255)
            {
                Player player = Main.LocalPlayer;
                CoinPlayer coinPlayer = player.GetModPlayer<CoinPlayer>();
                Terrain huanJing = CoinPlayer.InWhatTerrain;
                logo.SetImage(UISystem.Icons[(int)huanJing]);
                tileText.Left.Set(logo.Width.Pixels + 10f, 0f);
                tileText.SetText(ModHelper.GetText("HuanJingName." + huanJing) + ModHelper.GetText("Hint.Point") + " : " + coinPlayer.Point[(int)huanJing]);
            }
            Recalculate();
        }
    }
}
