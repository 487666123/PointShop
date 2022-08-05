using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PointShop.ModUI.UIElements
{
    public class ModScrollbar : FixedUIScrollbar
    {
        public ModScrollbar(UserInterface userInterface) : base(userInterface) { }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            ScrollWheelValue = 0;
        }

        // 用于滚动动画
        private float ScrollWheelValue = 0;
        public void SetViewPosition(int ScrollWheelValue)
        {
            this.ScrollWheelValue -= ScrollWheelValue;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (ScrollWheelValue != 0)
            {
                ViewPosition += ScrollWheelValue * 0.2f;
                ScrollWheelValue *= 0.8f;
                if (MathF.Abs(ScrollWheelValue) < 0.001f)
                {
                    ViewPosition = MathF.Round(ViewPosition, 3);
                    ScrollWheelValue = 0;
                }
            }
        }
    }
}
