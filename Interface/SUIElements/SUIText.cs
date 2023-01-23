using PointShop.Common.Configs;

namespace PointShop.Interface.SUIElements;

public class SUIText : View
{
    public Color TextColor, TextBorderColor;

    private readonly bool _big;
    private readonly float _scale;
    private string _text;
    private Vector2 _textSize;

    public string Text => _text;
    public SUIText SetText(string text, out Vector2 textSize)
    {
        _text = text;

        if (_big)
        {
            _textSize = MyUtils.TextSize(text, true) * _scale;
        }
        else
        {
            _textSize = MyUtils.TextSize(text) * _scale;
        }

        textSize = _textSize;
        return this;
    }

    public SUIText(string text, float scale, bool big = false)
    {
        _big = big;
        _scale = scale;
        SetText(text, out _);

        TextColor = Color.White;
        TextBorderColor = Color.Black;

        SetInnerPixels(_textSize);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        Vector2 innerPos = GetInnerDimensions().Position();
        Vector2 innerSize = GetInnerDimensions().Size();
        if (_big)
        {
            Vector2 offset = (PointConfig.Instance.UIYAxisOffset * 3 * _scale).Y();
            Vector2 textPos = innerPos + innerSize / 2 + offset;
            MyUtils.DrawBigText(textPos, _text, TextColor, TextBorderColor, _textSize / 2, _scale);
        }
        else
        {
            Vector2 offset = (PointConfig.Instance.UIYAxisOffset * _scale).Y();
            Vector2 textPos = innerPos + innerSize / 2 + offset;
            MyUtils.DrawText(textPos, _text, TextColor, TextBorderColor, _textSize / 2, _scale);
        }
    }
}