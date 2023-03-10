using PointShop.Common.Animations;

namespace PointShop.Interface.SUIElements;

internal class SUIPanel : View
{
    public bool Draggable
    {
        set
        {
            DraggableX = value;
            DraggableY = value;
        }
    }

    public bool DraggableX;
    public bool DraggableY;
    private bool _draggingX;
    private bool _draggingY;
    private float _offsetX;
    private float _offsetY;

    public SUIPanel(Color backgroundColor, Color borderColor, float border = 2)
    {
        DragIgnore = true;
        Border = border;
        BorderColor = borderColor;
        BgColor = backgroundColor;

        Rounded = new Vector4(10f);
        SetPadding(16);
    }

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        // 可拖动界面
        View view = evt.Target as View;
        // 当点击的是子元素不进行移动
        if (evt.Target != this && (view is null || !view.DragIgnore) &&
            !evt.Target.GetType().IsAssignableFrom(typeof(UIElement)))
        {
            return;
        }

        if (DraggableX)
        {
            _draggingX = true;
            _offsetX = evt.MousePosition.X - Left.Pixels;
        }

        if (DraggableY)
        {
            _draggingY = true;
            _offsetY = evt.MousePosition.Y - Top.Pixels;
        }
    }

    public override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);
        _draggingX = false;
        _draggingY = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsMouseHovering)
        {
            Main.LocalPlayer.mouseInterface = true;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        bool recalculate = false;
        if (_draggingX)
        {
            recalculate = true;
            Left.Pixels = Main.mouseX - _offsetX;
        }

        if (_draggingY)
        {
            recalculate = true;
            Top.Pixels = Main.mouseY - _offsetY;
        }

        if (recalculate)
        {
            Recalculate();
        }

        base.Draw(spriteBatch);
    }
}