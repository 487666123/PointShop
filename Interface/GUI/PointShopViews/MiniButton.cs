// using PointShop.Common.Animations;
// using PointShop.Interface.SUIElements;
//
// namespace PointShop.Interface.GUI.PointShopViews;
//
// public class MiniButton : SUIText
// {
//     private readonly AnimationTimer _hoverTimer;
//
//     public MiniButton(string text, float textScale = 0.8f) : base(text, textScale)
//     {
//         _hoverTimer = new AnimationTimer(3);
//         this.SetSize(TextSize);
//
//         Height.Pixels = 36f * textScale;
//         Width.Pixels = (MyUtils.TextSize(text).X + 30f) * textScale;
//
//         Border = 2f;
//         Rounded = new Vector4(10f);
//     }
//
//     public override void Update(GameTime gameTime)
//     {
//         _hoverTimer.Update();
//         base.Update(gameTime);
//     }
//
//     public override void MouseOver(UIMouseEvent evt)
//     {
//         _hoverTimer.Open();
//         base.MouseOver(evt);
//         SoundEngine.PlaySound(SoundID.MenuTick);
//     }
//
//     public override void MouseOut(UIMouseEvent evt)
//     {
//         _hoverTimer.Close();
//         base.MouseOut(evt);
//     }
//
//     protected override void DrawSelf(SpriteBatch sb)
//     {
//         BgColor = Color.Lerp(UIColor.ButtonBg, UIColor.ButtonBgHover, _hoverTimer.Schedule);
//         BorderColor = Color.Lerp(UIColor.ButtonBorder, UIColor.ButtonBorderHover, _hoverTimer.Schedule);
//         base.DrawSelf(sb);
//     }
// }