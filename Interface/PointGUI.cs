using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PointShop.Common.Players;
using PointShop.Common.Systems;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using static PointShop.Interface.PointShopGUI;

namespace PointShop.Interface
{
    public class PointGUI : UIState
    {
        public static bool Visible => !PointShopGUI.Visible;

        public UIPanel MainPanel;
        public UIImage logo;
        public UIText tileText;
        public UIImageButton button;

        public override void OnInitialize()
        {
            // 主面板
            MainPanel = new()
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

            logo = new(InterfaceSystem.icon[0])
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
                Terrain huanJing = CoinPlayer.PlayerInWhere();
                logo.SetImage(InterfaceSystem.icon[(int)huanJing]);
                tileText.Left.Set(logo.Width.Pixels + 10f, 0f);
                tileText.SetText(MyUtils.GetText("HuanJingName." + huanJing) + MyUtils.GetText("Hint.Point") + " : " + coinPlayer.Point[(int)huanJing]);
            }
            Recalculate();
        }
    }
}
