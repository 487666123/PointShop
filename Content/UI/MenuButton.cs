using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using static PointShop.Content.UI.CoinUI;

namespace PointShop.Content.UI
{
    public class MenuButton : UIImageButton
    {
        public HuanJing huanJing;
        public int i;

        public CoinUI coinUI; // 记录
        public MenuButton(Asset<Texture2D> texture, int i, ref float offsetX, CoinUI coinUI) : base(texture)
        {
            // 初始化数据
            this.coinUI = coinUI;
            huanJing = (HuanJing)i;
            this.i = i;

            // 以下只有设置位置和大小的代码
            Vector2 size = texture.Value.Size();
            Width.Set(size.X, 0f);
            Height.Set(size.Y, 0f);
            Left.Set(offsetX, 0f);
            Top.Set(-size.Y / 2f, 0.5f);
            offsetX += size.X + 16f;
        }

        // 点击事件
        public override void Click(UIMouseEvent evt)
        {
            coinUI.LogoImage.SetImage(PointShop.IconTextures[((int)huanJing)]);
            coinUI.huanJing = huanJing;
            JsonArray jsonArray = PointShop.ItemExchangeInfo[i].AsObject()["items"].AsArray();
            // 在UI里面添加 Item
            AddItemInCoinUI(coinUI, jsonArray, huanJing);
        }

        public static void AddItemInCoinUI(CoinUI coinUI, JsonArray jsonArray, HuanJing huanJing)
        {
            // 先清除数据
            coinUI.ItemListPanel.RemoveAllChildren();
            // 判断有没有数据
            if (jsonArray.Count > 0)
            {
                float offsetX = 0;
                float offsetY = 0;
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    ItemDisplaySlot itemDisplaySlot = new(((int)jsonArray[i]["id"]), ((int)jsonArray[i]["value"]), huanJing, ((int)jsonArray[i]["mode"]));
                    itemDisplaySlot.Left.Set(offsetX, 0f);
                    itemDisplaySlot.Top.Set(offsetY, 0f);
                    coinUI.ItemListPanel.Append(itemDisplaySlot);
                    offsetX += 55f + 14f;
                    if (offsetX + 55f > coinUI.panel.Width.Pixels - 16f * 4f)
                    {
                        offsetX = 0;
                        offsetY += 55 + 14f;
                    }
                }
                coinUI.ItemListPanel.Recalculate();
            }
        }

        // 绘制自己
        protected override void DrawSelf(SpriteBatch sb)
        {
            JsonObject keyValuePairs = PointShop.ItemExchangeInfo[i].AsObject();
            base.DrawSelf(sb);
            // 在鼠标位置绘制文字
            if (ContainsPoint(Main.MouseScreen))
            {
                Utils.DrawBorderString(sb, keyValuePairs["name"].ToString(), Main.MouseScreen + new Vector2(20f), Color.White);
            }
        }
    }
}
