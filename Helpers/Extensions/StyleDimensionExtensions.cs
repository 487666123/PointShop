namespace PointShop.Helpers;

public static class StyleDimensionExtensions
{
    public static StyleDimension Pixels(this float pixels)
    {
        return new StyleDimension(pixels, 0f);
    }

    public static StyleDimension Percent(this float percent)
    {
        return new StyleDimension(0f, percent);
    }
}