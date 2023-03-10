using PointShop.Helpers.Extensions;

namespace PointShop.Interface.SUIElements;

public class SUIImage : View
{
    public Texture2D Texture2D;
    public bool ButtonMode;

    public SUIImage(Texture2D texture2D)
    {
        Texture2D = texture2D;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        Vector2 pos = GetInnerDimensions().Position();
        Vector2 size = GetInnerDimensions().Size();
        Color color = ButtonMode ? IsMouseHovering ? Color.White : Color.White * 0.5f : Color.White;
        spriteBatch.Draw(Texture2D, pos + (size - Texture2D.Size()) / 2, color);
    }
}