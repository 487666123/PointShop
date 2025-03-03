// using PointShop.Common.Configs;
// using PointShop.Helpers.Extensions;
//
// namespace PointShop.Interface.SUIElements;
//
// public class SUIText : View
// {
//     public Func<Color> TextColor;
//     private Color _textColor, _textBorderColor;
//
//     // 用不到先注释掉
//     /*public void SetColor(Color textColor, Color textBorderColor)
//     {
//         _textColor = textColor;
//         _textBorderColor = textBorderColor;
//     }*/
//
//     private readonly bool _big;
//     private readonly float _textScale;
//     private string _text;
//     protected Vector2 TextSize;
//
//     public string Text => _text;
//
//     public SUIText SetText(string text, out Vector2 textSize)
//     {
//         _text = text;
//
//         if (_big)
//         {
//             TextSize = MyUtils.TextSize(text, true) * _textScale;
//         }
//         else
//         {
//             TextSize = MyUtils.TextSize(text) * _textScale;
//         }
//
//         textSize = TextSize;
//         return this;
//     }
//
//     public SUIText(string text, float textScale, bool big = false)
//     {
//         _big = big;
//         _textScale = textScale;
//         SetText(text, out _);
//
//         _textColor = Color.White;
//         _textBorderColor = Color.Black;
//
//         SetPadding(2f * textScale);
//         SetInnerPixels(TextSize);
//     }
//
//     protected override void DrawSelf(SpriteBatch spriteBatch)
//     {
//         base.DrawSelf(spriteBatch);
//         Vector2 innerPos = GetInnerDimensions().Position();
//         Vector2 innerSize = GetInnerDimensions().Size();
//         _textColor = TextColor?.Invoke() ?? _textColor;
//         if (_big)
//         {
//             Vector2 offset = (UIConfig.Instance.BigTextOffset * _textScale).Y() * _textScale;
//             Vector2 textPos = innerPos + (innerSize - TextSize) / 2 + offset;
//             MyUtils.DrawBigText(textPos, _text, _textColor, _textBorderColor, Vector2.Zero, _textScale);
//         }
//         else
//         {
//             Vector2 offset = (UIConfig.Instance.TextOffset * _textScale).Y() * _textScale;
//             Vector2 textPos = innerPos + (innerSize - TextSize) / 2 + offset;
//             MyUtils.DrawText(textPos, _text, _textColor, _textBorderColor, Vector2.Zero, _textScale);
//         }
//     }
// }