namespace PointShop.Helpers
{
    public static class CalculatedStyleExtensions
    {
        public static bool Contains(this CalculatedStyle calculatedStyle, Vector2 position)
        {
            if (calculatedStyle.X <= position.X && position.X < calculatedStyle.X + calculatedStyle.Width &&
                calculatedStyle.Y <= position.Y)
            {
                return position.Y < calculatedStyle.Y + calculatedStyle.Height;
            }

            return false;
        }
        
        public static Vector2 Size(this CalculatedStyle dimensions)
        {
            return new Vector2(dimensions.Width, dimensions.Height);
        }
    }
}
