// using System.Text;
// using PointShop.Common.Configs;
// using PointShop.Helpers.Extensions;
//
// namespace PointShop.Interface.SUIElements
// {
//     public class SUITitle : View
//     {
//         public Color TextColor, TextBorderColor;
//
//         private readonly string _text;
//         private readonly Vector2 _textSize;
//         private readonly float _scale;
//
//         public SUITitle(string text, float scale, bool big = true)
//         {
//             _scale = scale;
//             _text = text;
//             _textSize = MyUtils.TextSize(text, true) * _scale;
//
//             Height.Pixels = 50f;
//
//             DragIgnore = true;
//             TextColor = Color.White;
//             TextBorderColor = Color.Black;
//
//             SetPadding(20f, 0f);
//             SetInnerPixels(_textSize);
//         }
//
//         protected override void DrawSelf(SpriteBatch sb)
//         {
//             Vector2 innerPos = GetInnerDimensions().Position();
//             Vector2 innerSize = GetInnerDimensions().Size();
//             float offset = UIConfig.Instance.BigTextOffset * _scale;
//             Vector2 textPos = innerPos + (innerSize - _textSize) / 2 + offset.Y();
//             MyUtils.DrawBigText(textPos, _text, TextColor, TextBorderColor, Vector2.Zero, _scale);
//         }
//     }
// }