using PointShop.Interface.UIElements;

namespace PointShop.ModUI.UIElements
{
    public class ScrollView : UIElement
    {
        public ScrollList ScrollList;
        public ZeroScrollbar Scrollbar;

        public ScrollView(float Width, float Height, float Padding = 10f, float HSpacing = 10f, float VSpacing = 10f)
        {
            this.Width.Pixels = Width + Padding * 2;
            this.Height.Pixels = Height + Padding * 2;
            SetPadding(Padding);

            OverflowHidden = true;

            Append(ScrollList = new(HSpacing, VSpacing));
            ScrollList.Width.Pixels = Width - 30f;

            Append(Scrollbar = new()
            {
                HAlign = 1,
                VAlign = 0.5f
            });
            Scrollbar.Left.Pixels = -1;
            Scrollbar.Height.Pixels = Height - 0.5f;
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            Scrollbar.BufferViewPosition += evt.ScrollWheelValue;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMouseHovering)
            {
                Main.LocalPlayer.lastMouseInterface = true;
            }
            ScrollList.Top.Pixels = -Scrollbar.ViewPosition;
            ScrollList.Recalculate();
        }

        public void Clear()
        {
            ScrollList.RemoveAllChildren();
        }

        public void Append2List(UIElement uie)
        {
            ScrollList.AppendElement(uie);
            RefreshScrollbar();
            Recalculate();
        }

        public void RefreshScrollbar()
        {
            Scrollbar.SetView(this.Height() - this.VPadding(), ScrollList.Height());
        }
    }

    public class ScrollList : UIElement
    {
        public float HSpacing;
        public float VSpacing;
        public override bool ContainsPoint(Vector2 point) => true;

        public ScrollList(float HSpacing, float VSpacing)
        {
            SetPadding(0);
            this.HSpacing = HSpacing;
            this.VSpacing = VSpacing;
        }

        public void AppendElement(UIElement uie)
        {
            float x = 0f;
            float y = 0f;
            if (Elements.Count > 0)
            {
                UIElement end = Elements[^1];
                if (end.Left() + end.Width() + HSpacing + uie.Width() > this.WidthInside())
                {
                    x = 0;
                    y = end.Top() + end.Height() + VSpacing;
                }
                else
                {
                    x = end.Left() + end.Width() + HSpacing;
                    y = end.Top();
                }
            }
            uie.SetPos(x, y);
            Append(uie);
            Height.Pixels = uie.Top() + uie.Height() + 0.5f;
        }

        public void SetWidth(float Width)
        {
            this.Width.Pixels = Width;

            for (int i = 0; i < Elements.Count; i++)
            {
                UIElement uie = Elements[i];
                float x = 0f;
                float y = 0f;
                if (i > 0)
                {
                    UIElement end = Elements[i - 1];
                    if (end.Left() + end.Width() + HSpacing + uie.Width() > this.Width())
                    {
                        x = 0;
                        y = end.Top() + end.Height() + VSpacing;
                    }
                    else
                    {
                        x = end.Left() + end.Width() + HSpacing;
                        y = end.Top();
                    }
                }
                uie.SetPos(x, y);
                Height.Pixels = uie.Top() + VSpacing;
            }
        }
    }
}
