using PointShop.Common.Animations;
using PointShop.Common.Players;
using PointShop.Interface.Common;
using PointShop.Interface.ShopUI;
using PointShop.Interface.SUIElements;

namespace PointShop.Interface.TipUI
{
    public class PointGUI : UIState
    {
        public static bool Visible => MyUtils.Config.TerrainPanel;

        private SUIPanel _mainPanel;
        private SUIImage _terrainIcon;
        private SUIText _tip;
        private SUIImage _button;
        private AnimationTimer _switchTimer;

        public override void OnInitialize()
        {
            _switchTimer = new AnimationTimer();
            _mainPanel = new SUIPanel(UIColor.PanelBg, UIColor.PanelBorder)
            {
                HAlign = 0.5f, Top = 20f.Pixels()
            };
            _mainPanel.SetPadding(16f, 0f).SetSizePixels(200f, 50f);
            _mainPanel.Join(this);

            _terrainIcon = new SUIImage(UISystem.Icons[0])
            {
                VAlign = 0.5f
            };
            _terrainIcon.SetSizePixels(UISystem.Icons[0].Size());
            _terrainIcon.Join(_mainPanel);

            _tip = new SUIText("Chinese: 中文", 0.8f)
            {
                VAlign = 0.5f,
                Relative = RelativeMode.Horizontal
            };
            _mainPanel.Append(_tip);

            _button = new SUIImage(UIAssets.PlayButton)
            {
                VAlign = 0.5f,
                ButtonMode = true,
                Relative = RelativeMode.Horizontal,
                Spacing = 15f.Xy()
            };
            _button.SetSizePixels(UIAssets.PlayButton.Size());
            _button.OnClick += (_, _) => PointShopGUI.Visible = !PointShopGUI.Visible;
            _button.Join(_mainPanel);
        }

        public override void Update(GameTime gameTime)
        {
            bool recalculate = false;

            Player player = Main.LocalPlayer;
            CoinPlayer coinPlayer = player.GetModPlayer<CoinPlayer>();
            Terrain terrain = CoinPlayer.InWhatTerrain;

            string text =
                $"{MyUtils.GetText($"TerrainName.{terrain}")}{MyUtils.GetText("Hint.Point")}: {coinPlayer.Point[(int)terrain]}";

            if (_terrainIcon.Texture2D != UISystem.Icons[(int)terrain])
            {
                recalculate = true;
                _terrainIcon.Texture2D = UISystem.Icons[(int)terrain];
                _terrainIcon.SetSizePixels(_terrainIcon.Texture2D.Size());
            }

            if (_tip.Text != text)
            {
                recalculate = true;
                _tip.SetText(text, out Vector2 textSize).SetSizePixels(textSize);
            }

            float right = _button.RightPixels();

            if (Math.Abs(_mainPanel.GetInnerPixel().X - right) > 0.000000001)
            {
                recalculate = true;
                _mainPanel.SetInnerPixels(right, _mainPanel.Height.Pixels);
            }

            if (recalculate)
            {
                _mainPanel.Recalculate();
            }

            // 防止点击按键时使用物品
            if (_button.IsMouseHovering)
            {
                player.mouseInterface = true;
            }
        }
    }
}