// using PointShop.Common.Animations;
// using PointShop.Helpers.Extensions;
// using Terraria.GameInput;
//
// namespace PointShop.Interface.SUIElements
// {
//     public class SUIScrollBar : View
//     {
//         private float viewPosition; // 滚动条当前位置
//         private float MaxViewPoisition => maxViewSize - viewSize;
//
//         private float viewSize = 1f; // 显示出来的高度
//         private float maxViewSize = 20f; // 控制元素的高度
//         private float ViewScale => viewSize / maxViewSize;
//         private bool innerHovered;
//
//         // 用于拖动内滚动条
//         private float offsetY;
//         public bool dragging;
//
//         public bool Visible;
//
//         public AnimationTimer HoverTimer = new(3);
//
//         public float ViewPosition
//         {
//             get => viewPosition;
//             set => viewPosition = MathHelper.Clamp(value, 0f, MaxViewPoisition);
//         }
//
//         private float _bufferViewPosition;
//
//         /// <summary>
//         /// 缓冲距离, 不想使用动画就直接设置 <see cref="ViewPosition"/>
//         /// </summary>
//         public float BufferViewPosition
//         {
//             get => _bufferViewPosition;
//             set => _bufferViewPosition = value;
//         }
//
//         public SUIScrollBar()
//         {
//             Visible = true;
//             Width.Pixels = 20f;
//
//             BgColor = UIColor.ScrollBarBg;
//             Border = 2f;
//             BorderColor = UIColor.ScrollBarBorder;
//
//             SetPadding(5);
//         }
//
//         public override void Update(GameTime gameTime)
//         {
//             if (!Visible)
//                 return;
//
//             CalculatedStyle InnerDimensions = GetInnerDimensions();
//             CalculatedStyle InnerRectangle = InnerDimensions;
//             InnerRectangle.Y += (ViewPosition / MaxViewPoisition) * (InnerDimensions.Height * (1 - ViewScale));
//             InnerRectangle.Height = InnerDimensions.Height * ViewScale;
//             if (IsMouseHovering)
//             {
//                 if (!innerHovered)
//                 {
//                     innerHovered = true;
//                     InnerMouseOver();
//                 }
//             }
//             else
//             {
//                 if (innerHovered)
//                 {
//                     InnerMouseOut();
//                 }
//
//                 innerHovered = false;
//             }
//
//             HoverTimer.Update();
//             base.Update(gameTime);
//
//             if (dragging)
//             {
//                 if (Math.Abs(ViewScale - 1) > 0.000000001f)
//                     ViewPosition = (Main.MouseScreen.Y - InnerDimensions.Y - offsetY) /
//                         (InnerDimensions.Height * (1 - ViewScale)) * MaxViewPoisition;
//             }
//
//             if (BufferViewPosition == 0)
//             {
//                 return;
//             }
//
//             ViewPosition -= BufferViewPosition * 0.2f;
//             BufferViewPosition *= 0.8f;
//             if (!(MathF.Abs(BufferViewPosition) < 0.1f))
//             {
//                 return;
//             }
//
//             ViewPosition = MathF.Round(ViewPosition, 1);
//             BufferViewPosition = 0;
//         }
//
//         public virtual void InnerMouseOver()
//         {
//             HoverTimer.Open();
//             SoundEngine.PlaySound(SoundID.MenuTick);
//         }
//
//         public virtual void InnerMouseOut()
//         {
//             HoverTimer.Close();
//         }
//
//         public override void LeftMouseDown(UIMouseEvent evt)
//         {
//             base.LeftMouseDown(evt);
//
//             if (!Visible)
//                 return;
//
//             if (evt.Target != this)
//             {
//                 return;
//             }
//
//             CalculatedStyle innerDimensions = GetInnerDimensions();
//
//             if (IsMouseHovering)
//             {
//                 dragging = true;
//                 offsetY = evt.MousePosition.Y - innerDimensions.Y -
//                           (innerDimensions.Height * (1 - ViewScale) * (viewPosition / MaxViewPoisition));
//             }
//
//             BufferViewPosition = 0;
//         }
//
//         public override void LeftMouseUp(UIMouseEvent evt)
//         {
//             base.LeftMouseUp(evt);
//             if (!Visible)
//                 return;
//             dragging = false;
//         }
//
//         public override void MouseOver(UIMouseEvent evt)
//         {
//             base.MouseOver(evt);
//             if (!Visible)
//                 return;
//             PlayerInput.LockVanillaMouseScroll("ModLoader/UIScrollbar");
//         }
//
//         protected override void DrawSelf(SpriteBatch spriteBatch)
//         {
//             Vector2 size = GetDimensions().Size();
//             Rounded = new Vector4(MathF.Min(size.X, size.Y) / 2f);
//             base.DrawSelf(spriteBatch);
//             if (!Visible)
//                 return;
//
//             CalculatedStyle innerDimensions = GetInnerDimensions();
//             Vector2 innerPos = innerDimensions.Position();
//             Vector2 innerSize = innerDimensions.Size();
//             if (MaxViewPoisition != 0)
//                 innerPos.Y += innerDimensions.Height * (1 - ViewScale) * (ViewPosition / MaxViewPoisition);
//             innerSize.Y *= ViewScale;
//
//             Color hoverColor = Color.Lerp(UIColor.ScrollBarInnerBgHover, UIColor.ScrollBarInnerBg,
//                 dragging ? 1 : HoverTimer.Schedule);
//
//             // 滚动条拖动块
//             SDFRectangle.NoBorder(innerPos, innerSize, new Vector4(innerSize.X / 2), hoverColor);
//         }
//
//         /// <summary>
//         /// 设置范围
//         /// </summary>
//         /// <param name="viewSize">绑定 UI 的显示大小</param>
//         /// <param name="maxViewSize">绑定 UI 的真实大小</param>
//         public void SetView(float viewSize, float maxViewSize)
//         {
//             viewSize = MathHelper.Clamp(viewSize, 0f, maxViewSize);
//             viewPosition = MathHelper.Clamp(viewPosition, 0f, maxViewSize - viewSize);
//
//             this.viewSize = viewSize;
//             this.maxViewSize = maxViewSize;
//         }
//     }
// }