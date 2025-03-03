// using PointShop.Interface.SUIElements;
//
// namespace PointShop.Interface.BaseView
// {
//     // 通用滚动视图
//     public class ScrollView : View
//     {
//         public readonly SUIScrollBar ScrollBar;
//         public readonly ListView ListView;
//
//         public ScrollView()
//         {
//             OverflowHidden = true;
//             DragIgnore = true;
//
//             ListView = new ListView();
//             ListView.Join(this);
//
//             ScrollBar = new SUIScrollBar
//             {
//                 HAlign = 1f,
//                 Height = new StyleDimension(-2f, 1f)
//             };
//             ScrollBar.Left.Pixels = -2;
//             ScrollBar.Join(this);
//         }
//
//         protected override void DrawSelf(SpriteBatch spriteBatch)
//         {
//             base.DrawSelf(spriteBatch);
//
//             if (!(Math.Abs(-ScrollBar.ViewPosition - ListView.Top.Pixels) > 0.000000001f))
//             {
//                 return;
//             }
//
//             ListView.Top.Pixels = -ScrollBar.ViewPosition;
//             ListView.Recalculate();
//         }
//
//         public override void ScrollWheel(UIScrollWheelEvent evt)
//         {
//             base.ScrollWheel(evt);
//             ScrollBar.BufferViewPosition += evt.ScrollWheelValue;
//         }
//
//         public static Vector2 TotalSize(Vector2 size, Vector2 spacing, int h, int v)
//         {
//             return (size + spacing) * new Vector2(h, v) - spacing;
//         }
//     }
// }