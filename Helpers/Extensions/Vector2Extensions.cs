namespace PointShop.Helpers.Extensions;

public static class Vector2Extensions
{
    public static Vector2 Xy(this float xy)
    {
        return new Vector2(xy);
    }

    public static Vector2 X(this float x)
    {
        return new Vector2(x, 0);
    }

    public static Vector2 Y(this float y)
    {
        return new Vector2(0, y);
    }
}