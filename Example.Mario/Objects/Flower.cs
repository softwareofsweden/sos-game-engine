using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class Flower : BaseEntity
    {

        protected bool isAppearing;
        protected int moveCount;

        public Flower(Game game, int x, int y, SosEngine.Level level) :
            base(game, "flower_0", x, y, new Vector2(1, 0), level, 20)
        {
            SosEngine.Core.PlaySound("ItemSprout");
            AddFrame("flower_1", 20);
            AddFrame("flower_2", 20);
            AddFrame("flower_3", 20);
            this.isAppearing = true;
            this.hasGravity = false;
            this.Position = new Vector2(Position.X, Position.Y - 4);
        }

        protected override Rectangle GetBoundingBox()
        {
            if (isAppearing)
            {
                return Rectangle.Empty;
            }
            else
            {
                return base.GetBoundingBox();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (isAppearing)
            {
                Position = new Vector2(Position.X, Position.Y - 1);
                if (Position.Y % 16 == 0)
                {
                    isAppearing = false;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            if (!Visible)
            {
                return;
            }
            if (spriteFrame != null)
            {
                SpriteEffects spriteEffects = Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                Rectangle sourceRect = spriteFrame.Rectangle;
                if (isAppearing)
                {
                    sourceRect.Height = 16 - (int)Position.Y % 16;
                }

                // texture, position, sourceRectangle, color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth
                Color color = alpha < 1f ? Color.White * alpha : Color.White;
                SosEngine.Core.SpriteBatch.Draw(SosEngine.Core.GetTexture(spriteFrame.AssetName), this.Position + new Vector2(DrawOffsetX, DrawOffsetY), sourceRect, color,
                    0, this.origin, new Vector2(this.ScaleX, this.ScaleY), spriteEffects, 0);

                if (SosEngine.Core.DebugSpriteBorders && !BoundingBox.IsEmpty)
                {
                    Rectangle rectangle = new Rectangle(BoundingBox.X + DrawOffsetX, BoundingBox.Y + DrawOffsetY, BoundingBox.Width, BoundingBox.Height);
                    SosEngine.Core.DrawRectangle(rectangle, Color.Red);
                }

            }

            
        }

    }
}
