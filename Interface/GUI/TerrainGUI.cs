using PointShop.Common.Configs;
using PointShop.Common.Players;
using PointShop.Helpers.Extensions;
using PointShop.Interface.Common;
using PointShop.Interface.SUIElements;

namespace PointShop.Interface.GUI
{
    internal class TerrainGUI : UIState
    {
        public static bool Visible => UIConfig.Instance.TerrainPanel;

        public SUIPanel MainPanel;
        private SUIImage _icon;
        private SUIText _pointInfo;
        private SUIImage _switch;

        public override void OnInitialize()
        {
            MainPanel = new SUIPanel(UIColor.PanelBg, UIColor.PanelBorder)
            {
                DraggableX = true,
                HAlign = 0.5f,
                Top = 20f.Pixels(),
                Shadow = 10f,
                ShadowExtraSize = 10f,
                ShadowColor = UIColor.PanelBorder * 0.5f
            };
            MainPanel.SetPadding(16f, 0f).SetSizePixels(200f, 46f);
            MainPanel.SetPosPixels(UIPlayerData.Local.TerrainPosX, 20f);
            MainPanel.Join(this);

            _icon = new SUIImage(UISystem.Icons[0])
            {
                DragIgnore = true,
                VAlign = 0.5f
            };
            _icon.SetSizePixels(UISystem.Icons[0].Size());
            _icon.Join(MainPanel);

            _pointInfo = new SUIText("Chinese: 中文", 0.85f)
            {
                DragIgnore = true,
                VAlign = 0.5f,
                LayoutMode = LayoutMode.Horizontal,
                Spacing = 12f.Xy()
            };
            MainPanel.Append(_pointInfo);

            _switch = new SUIImage(UIAssets.PlayButton)
            {
                VAlign = 0.5f,
                ButtonMode = true,
                LayoutMode = LayoutMode.Horizontal,
                Spacing = 15f.Xy()
            };
            _switch.SetSizePixels(UIAssets.PlayButton.Size());
            _switch.OnLeftClick += (_, _) => PointShopGUI.Visible = !PointShopGUI.Visible;
            _switch.Join(MainPanel);
        }

        public override void Update(GameTime gameTime)
        {
            bool recalculate = GetDimensions().Size() != new Vector2(Main.screenWidth, Main.screenHeight);

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
                $"{MyUtils.GetText($"TerrainName.{terrain}")}: {coinPlayer.Point[(int)terrain]}";

            if (_pointInfo.Text != text)
            {
                recalculate = true;
                _pointInfo.SetText(text, out Vector2 textSize).SetSizePixels(textSize);
            }

            float right = _switch.RightPixels();

            if (Math.Abs(MainPanel.GetInnerPixel().X - right) > 0.000000001)
            {
                recalculate = true;
                MainPanel.SetInnerPixels(right, 46f);
            }

            if (recalculate)
            {
                Recalculate();
            }

            // 防止点击按键时使用物品
            if (_switch.IsMouseHovering)
            {
                player.mouseInterface = true;
            }
        }
    }
}