using PointShop.Common.Players;
using PointShop.Interface.Common;
using PointShop.Interface.SUIElements;

namespace PointShop.Interface.GUI
{
    public class TerrainGUI : UIState
    {
        public static bool Visible => MyUtils.Config.TerrainPanel;

        private SUIPanel _mainPanel;
        private SUIImage _icon;
        private SUIText _pointInfo;
        private SUIImage _switch;

        public override void OnInitialize()
        {
            _mainPanel = new SUIPanel(UIColor.PanelBg, UIColor.PanelBorder)
            {
                HAlign = 0.5f, Top = 20f.Pixels()
            };
            _mainPanel.SetPadding(16f, 0f).SetSizePixels(200f, 46f);
            _mainPanel.Join(this);

            _icon = new SUIImage(UISystem.Icons[0])
            {
                VAlign = 0.5f
            };
            _icon.SetSizePixels(UISystem.Icons[0].Size());
            _icon.Join(_mainPanel);

            _pointInfo = new SUIText("Chinese: 中文", 0.8f)
            {
                VAlign = 0.5f,
                Relative = RelativeMode.Horizontal,
                Spacing = 10f.Xy()
            };
            _mainPanel.Append(_pointInfo);

            _switch = new SUIImage(UIAssets.PlayButton)
            {
                VAlign = 0.5f,
                ButtonMode = true,
                Relative = RelativeMode.Horizontal,
                Spacing = 15f.Xy()
            };
            _switch.SetSizePixels(UIAssets.PlayButton.Size());
            _switch.OnClick += (_, _) => PointShopGUI.Visible = !PointShopGUI.Visible;
            _switch.Join(_mainPanel);
        }

        public override void Update(GameTime gameTime)
        {
            bool recalculate = false;

            Player player = Main.LocalPlayer;
            CoinPlayer coinPlayer = player.GetModPlayer<CoinPlayer>();
            Terrain terrain = CoinPlayer.InWhatTerrain;

            if (_icon.Texture2D != UISystem.Icons[(int)terrain])
            {
                recalculate = true;
                _icon.Texture2D = UISystem.Icons[(int)terrain];
                _icon.SetSizePixels(_icon.Texture2D.Size());
            }

            string text =
                $"{MyUtils.GetText($"TerrainName.{terrain}")}{MyUtils.GetText("Hint.Point")}: {coinPlayer.Point[(int)terrain]}";

            if (_pointInfo.Text != text)
            {
                recalculate = true;
                _pointInfo.SetText(text, out Vector2 textSize).SetSizePixels(textSize);
            }

            float right = _switch.RightPixels();

            if (Math.Abs(_mainPanel.GetInnerPixel().X - right) > 0.000000001)
            {
                recalculate = true;
                _mainPanel.SetInnerPixels(right, 46f);
            }

            if (recalculate)
            {
                _mainPanel.Recalculate();
            }

            // 防止点击按键时使用物品
            if (_switch.IsMouseHovering)
            {
                player.mouseInterface = true;
            }
        }
    }
}