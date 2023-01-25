using PointShop.Interface.SUIElements;

namespace PointShop.Interface.BaseView
{
    // 通用滚动视图
    public class ScrollView : View
    {
        public readonly SUIScrollBar ScrollBar;
        public readonly ListView ListView;

        public ScrollView()
        {
            OverflowHidden = true;
            DragIgnore = true;

            ListView = new ListView();
            ListView.Join(this);

            ScrollBar = new SUIScrollBar();
            ScrollBar.Join(this);
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            ScrollBar.BufferViewPosition += evt.ScrollWheelValue;
        }

        public static Vector2 TotalSize(Vector2 size, Vector2 spacing, int h, int v)
        {
            return (size + spacing) * new Vector2(h, v) - spacing;
        }

        public static Vector2 TotalSize(float size, float spacing, int h, int v)
        {
            return TotalSize(new Vector2(size), new Vector2(spacing), h, v);
        }

        public static float TotalSize(float size, float spacing, int hv)
        {
            return (size + spacing) * hv - spacing;
        }
    }
}