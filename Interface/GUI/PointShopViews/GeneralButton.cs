using PointShop.Common.Animations;
using PointShop.Common.Configs;
using PointShop.Helpers.Extensions;

namespace PointShop.Interface.GUI.PointShopViews
{
    public class GeneralButton : View
    {
        private static float _iconMaxWidth;

        private readonly AnimationTimer _hoverTimer;
        private readonly Texture2D _texture;

        private string _text;
        private Vector2 _textSize;

        public Func<string> RealTimeText;
        public bool IconMode;

        private void SetText(string value)
        {
            _text = value;
            _textSize = MyUtils.TextSize(value);
        }

        public GeneralButton(Texture2D texture, string text)
        {
            _hoverTimer = new AnimationTimer(3);
            _texture = texture;

            if (_texture.Size().X > _iconMaxWidth)
            {
                _iconMaxWidth = _texture.Size().X;
            }

            SetText(text);
            SetPadding(10f);
            Rounded = new Vector4(10f);
            Border = 2;
        }

        public override void Update(GameTime gameTime)
        {
            _hoverTimer.Update();
            base.Update(gameTime);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            _hoverTimer.Open();
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            _hoverTimer.Close();
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            BgColor = Color.Lerp(UIColor.ButtonBg, UIColor.ButtonBgHover, _hoverTimer.Schedule);
            BorderColor = Color.Lerp(UIColor.ButtonBorder, UIColor.ButtonBorderHover, _hoverTimer.Schedule);
            base.DrawSelf(sb);

            if (RealTimeText?.Invoke() is { } text && text != _text)
            {
                SetText(text);
            }

            Vector2 innerPos = GetInnerDimensions().Position();
            Vector2 innerSize = GetInnerDimensions().Size();
            sb.Draw(_texture, innerPos + (new Vector2(_iconMaxWidth, innerSize.Y) - _texture.Size()) / 2f, Color.White);

            Vector2 textPos = innerPos + new Vector2(_iconMaxWidth, 0);
            Vector2 textSize = innerSize - new Vector2(_iconMaxWidth, 0);
            MyUtils.DrawText(textPos + (textSize - _textSize) / 2f + UIConfig.Instance.TextOffset.Y(),
                _text, Color.White, Color.Black);
        }
    }
}