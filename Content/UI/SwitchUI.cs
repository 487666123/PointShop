using LootCoins.Common.Players;
using LootCoins.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static LootCoins.Content.UI.CoinUI;

namespace LootCoins.Content.UI
{
    public class SwitchUI : UIState
    {
        public static bool Visible => !CoinUI.Visible;

        public UIPanel panel;
        public UIImage logo;
        public UIText tileText;
        public UIImageButton button;

        public Asset<Texture2D> logoImage;

        public override void OnInitialize()
        {
            // 加载Logo
            logoImage = LootCoins.IconTextures[((int)CoinModSystem.coinUI.huanJing)];

            // 主面板
            panel = new()
            {
                PaddingTop = 0f,
                PaddingBottom = 0f,
                PaddingLeft = 16f,
                PaddingRight = 16f,
                HAlign = 0.5f
            };
            panel.Height.Set(50f, 0f);
            panel.Width.Set(220f, 0f);
            panel.Top.Set(20f, 0f);

            logo = new(logoImage)
            {
                VAlign = 0.5f
            };
            logo.OnUpdate += Logo_OnUpdate;

            tileText = new("", 0.9f)
            {
                VAlign = 0.5f
            };
            tileText.Left.Set(logo.Width.Pixels + 10f, 0f);

            button = new(ModContent.Request<Texture2D>("LootCoins/Images/ButtonPlay", AssetRequestMode.ImmediateLoad))
            {
                VAlign = 0.5f,
                HAlign = 1f
            };
            button.OnClick += Button_OnClick;

            this.Append(panel);
            panel.Append(logo);
            panel.Append(tileText);
            panel.Append(button);
        }

        private void Button_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            CoinUI.Visible = true;
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
                HuanJing huanJing = CoinPlayer.PlayerInHuanJing(player);
                logoImage = LootCoins.IconTextures[((int)huanJing)];
                logo.SetImage(logoImage);
                tileText.Left.Set(logo.Width.Pixels + 10f, 0f);
                tileText.SetText(LootCoins.ItemExchangeInfo[((int)huanJing)].AsObject()["name"].ToString() + Language.GetTextValue($"Mods.LootCoins.Hint.积分2") + coinPlayer.HuanJingFen[(int)huanJing]);
            }
            this.Recalculate();
        }
    }
}
