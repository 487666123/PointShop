using System.Collections.Generic;
using PointShop.Common.Animations;

namespace PointShop.Interface
{
    /// <summary>
    /// 排列模式，横向排列或者纵向排列。
    /// </summary>
    public enum RelativeMode
    {
        Disabled,
        Horizontal,
        Vertical
    };

    /// <summary>
    /// 相对定位，用于可变大小的 UI 更方便计算位置。
    /// </summary>
    public class View : UIElement
    {
        /// <summary>
        /// 相对的模式，横向填充或者纵向填充
        /// </summary>
        public RelativeMode Relative;

        /// <summary>
        /// 间距
        /// </summary>
        public Vector2 Spacing;

        /// <summary>
        /// 越界换行
        /// </summary>
        public bool Wrap;

        /// <summary>
        /// 拖动忽略，需要自己在 Panel 加判定
        /// </summary>
        public bool DragIgnore;

        public float Border;
        public Color BgColor, BorderColor;
        public Vector4 Rounded;

        public View()
        {
            Border = -1;
        }

        public override void Recalculate()
        {
            if (Relative != RelativeMode.Disabled && Parent is View { Children: List<UIElement> views } parent)
            {
                int index = views!.IndexOf(this);
                // 判断前面有没有元素
                if (index > 0 && views[index - 1] is View before)
                {
                    Vector2 parentSize = parent.GetInnerDimensions().Size();

                    switch (Relative)
                    {
                        case RelativeMode.Horizontal:
                            Left.Pixels = before.RightPixels() + Spacing.X;

                            if (Wrap && RightPixels() > parentSize.X)
                            {
                                SetPosPixels(0, before.BottomPixels() + Spacing.Y);
                            }

                            break;
                        case RelativeMode.Vertical:
                            Top.Pixels = before.BottomPixels() + Spacing.Y;

                            if (Wrap && BottomPixels() > parentSize.Y)
                            {
                                SetPosPixels(before.RightPixels() + Spacing.X, 0f);
                            }

                            break;
                    }
                }
            }

            base.Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 pos = GetDimensions().Position();
            Vector2 size = GetDimensions().Size();

            if (Border > 0 && (BgColor != Color.Transparent || BorderColor != Color.Transparent))
            {
                PixelShader.RoundedRectangle(pos, size, Rounded, BgColor, Border, BorderColor);
            }
            else if (BgColor != Color.Transparent)
            {
                PixelShader.RoundedRectangle(pos, size, Rounded, BgColor);
            }

            base.DrawSelf(spriteBatch);
        }

        /// <summary>
        /// 加入我们吧！！！
        /// </summary>
        public void Join(UIElement parent)
        {
            parent.Append(this);
        }

        public View SetPosPixels(float left, float top)
        {
            Left.Pixels = left;
            Top.Pixels = top;
            return this;
        }

        public View SetPosPixels(Vector2 size)
        {
            Left.Pixels = size.X;
            Top.Pixels = size.Y;
            return this;
        }

        public View SetInnerPixels(float width, float height)
        {
            Width.Pixels = width + HPadding;
            Height.Pixels = height + VPadding;
            return this;
        }

        public View SetInnerPixels(Vector2 size)
        {
            Width.Pixels = size.X + HPadding;
            Height.Pixels = size.Y + VPadding;
            return this;
        }

        public View SetInnerPixels(float size)
        {
            Width.Pixels = size + HPadding;
            Height.Pixels = size + VPadding;
            return this;
        }

        public View SetSizePixels(float width, float height)
        {
            Width.Pixels = width;
            Height.Pixels = height;
            return this;
        }

        public View SetSizePixels(Vector2 size)
        {
            Width.Pixels = size.X;
            Height.Pixels = size.Y;
            return this;
        }

        public View SetPadding(float left, float top, float right, float bottom)
        {
            PaddingLeft = left;
            PaddingTop = top;
            PaddingRight = right;
            PaddingBottom = bottom;
            return this;
        }

        public View SetPadding(float h, float v)
        {
            PaddingLeft = PaddingRight = h;
            PaddingTop = PaddingBottom = v;
            return this;
        }

        public float HPadding => PaddingLeft + PaddingRight;
        public float VPadding => PaddingTop + PaddingBottom;

        // 获取
        public float RightPixels()
        {
            return Left.Pixels + Width.Pixels;
        }

        public float BottomPixels()
        {
            return Top.Pixels + Height.Pixels;
        }

        public Vector2 GetPosPixel()
        {
            return new Vector2(Left.Pixels, Top.Pixels);
        }

        public Vector2 GetSizePixel()
        {
            return new Vector2(Width.Pixels, Height.Pixels);
        }

        public Vector2 GetInnerPixel()
        {
            return new Vector2(Width.Pixels - this.HPadding, Height.Pixels - this.VPadding);
        }
    }
}