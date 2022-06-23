using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PointShop.Common.Players;
using PointShop.Common.Systems;
using PointShop.UI.UIElements;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using static PointShop.UI.ShopState;

namespace PointShop.UI
{
    public class PointState : UIState
    {
        public static bool Visible => !ShopState.Visible;

        public JuPanel panel;
        public UIImage logo;
        public UIText tileText;
        public UIImageButton button;

        public Asset<Texture2D> logoImage;

        public override void OnInitialize()
        {
            // 加载Logo
            logoImage = PointShop.IconTextures[(int)CoinModSystem.coinUI.terrain];

            // 主面板
            panel = new()
            {
                PaddingTop = 0f,
                PaddingBottom = 0f,
                PaddingLeft = 16f,
                PaddingRight = 16f,
                HAlign = 0.5f
            };
            panel.Height.Set(45f, 0f);
            panel.Width.Set(200f, 0f);
            panel.Top.Set(20f, 0f);

            logo = new(logoImage)
            {
                VAlign = 0.5f
            };
            logo.OnUpdate += Logo_OnUpdate;

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

            Append(panel);
            panel.Append(logo);
            panel.Append(tileText);
            panel.Append(button);
        }

        private void Button_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ShopState.Visible = true;
        }

        private void Logo_OnUpdate(UIElement affectedElement)
        {
            Main.NewText(100);
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.myPlayer > -1 && Main.myPlayer < 255)
            {
                Player player = Main.LocalPlayer;
                CoinPlayer coinPlayer = player.GetModPlayer<CoinPlayer>();
                Terrain huanJing = CoinPlayer.PlayerInWhere();
                logoImage = PointShop.IconTextures[(int)huanJing];
                logo.SetImage(logoImage);
                tileText.Left.Set(logo.Width.Pixels + 10f, 0f);
                tileText.SetText(MyUtils.GetText("HuanJingName." + huanJing) + MyUtils.GetText("Hint.Point") + " : " + coinPlayer.Point[(int)huanJing]);
            }
            Recalculate();
        }
    }
}
