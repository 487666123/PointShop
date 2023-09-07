using PointShop.Common.Animations;
using PointShop.Helpers.Extensions;

namespace PointShop.Interface.SUIElements
{
    public class ColorAnimation
    {
        public readonly AnimationTimer Timer;
        public Color Begin;
        public Color End;
        public Color Get => Color.Lerp(Begin, End, Timer.Schedule);

        public ColorAnimation(Color begin, Color end, AnimationTimer timer2)
        {
            Begin = begin;
            End = end;
            Timer = timer2;
        }
    }

    public class SUICross : View
    {
        private readonly float
            _forkSize, _crossRound, _crossBorder;

        private Color _crossBorderColor;
        private readonly Color _crossBeginColor;
        private readonly Color _crossEndColor;

        private readonly AnimationTimer _hoverTimer;

        public SUICross(float forkSize)
        {
            _hoverTimer = new AnimationTimer();
            Rounded = new Vector4(10f);
            _crossRound = 4.5f;
            _crossBorder = 2;
            _forkSize = forkSize;
            Width.Pixels = 50;
            Height.Pixels = 50;
            _crossBeginColor = UIColor.Cross * 0.5f;
            _crossEndColor = UIColor.Cross;

            Border = 2;
            BorderColor = UIColor.PanelBorder;
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            _hoverTimer.Open();
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            _hoverTimer.Close();
            base.MouseOut(evt);
        }

        public override void Update(GameTime gameTime)
        {
            _hoverTimer.Update();
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            base.DrawSelf(sb);
            _crossBorderColor = Color.Lerp(UIColor.PanelBorder, UIColor.ItemSlotBorderFav, _hoverTimer.Schedule);
            Vector2 pos = GetDimensions().Position();
            Vector2 size = GetDimensions().Size();

            Vector2 forkPos = pos + size / 2 - new Vector2(_forkSize / 2);
            Color cross = Color.Lerp(_crossBeginColor, _crossEndColor, _hoverTimer.Schedule);
            SDFGraphic.HasBorderCross(forkPos, _forkSize, _crossRound, cross, _crossBorder, _crossBorderColor);
        }
    }
}