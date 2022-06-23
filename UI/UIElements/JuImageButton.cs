using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Text.Json.Nodes;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static PointShop.UI.ShopState;

namespace PointShop.UI.UIElements
{
    public class JuImageButton : UIImageButton
    {
        public Terrain terrain;
        public int i;

        public ShopState coinUI; // 记录
        public JuImageButton(Asset<Texture2D> texture, int i, ref float offsetX, ShopState coinUI) : base(texture)
        {
            // 初始化数据
            this.coinUI = coinUI;
            terrain = (Terrain)i;
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
            coinUI.LogoImage.SetImage(PointShop.IconTextures[(int)terrain]);
            coinUI.LogoImage.Recalculate();
            coinUI.terrain = terrain;
            JsonArray jsonArray = PointShop.ItemExchangeInfo[i].AsObject()["items"].AsArray();
            // 在UI里面添加 Item
            LoadItemConfig(coinUI, jsonArray, terrain);
        }

        public static void LoadItemConfig(ShopState mainUI, JsonArray itemConfig, Terrain terrain)
        {
            // 先清除数据
            mainUI.ItemPanel.RemoveAllChildren();
            // 判断有没有数据
            if (itemConfig.Count > 0)
            {
                float offsetX = 0;
                float offsetY = 0;
                for (int i = 0; i < itemConfig.Count; i++)
                {
                    JuItemSlot itemSlot = new((int)itemConfig[i]["id"], (int)itemConfig[i]["value"], terrain, (int)itemConfig[i]["mode"]);
                    itemSlot.SetPos(offsetX, offsetY);
                    mainUI.ItemPanel.Append(itemSlot);
                    offsetX += JuItemSlot.Size + 8f;
                    if (offsetX + JuItemSlot.Size > mainUI.MainPanel.Width.Pixels - 16f * 2f - 10 * 2)
                    {
                        offsetX = 0;
                        offsetY += JuItemSlot.Size + 8f;
                    }
                }
                mainUI.ItemPanel.Recalculate();
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
