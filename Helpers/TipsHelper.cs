using Microsoft.Xna.Framework;

namespace PointShop.Helpers
{
    public static class TipsHelper
    {
        // PopupText
        public static int NewText(Vector2 center, Vector2 velocity, Color color, int duration, string text)
        {
            AdvancedPopupRequest request = new()
            {
                Text = text,
                Color = color,
                Velocity = velocity,
                DurationInFrames = duration
            };
            return PopupText.NewText(request, center);
        }

        public static PopupText NewTextDirect(Vector2 center, Vector2 velocity, Color color, int duration, string text)
        {
            return Main.popupText[NewText(center, velocity, color, duration, text)];
        }
    }
}
