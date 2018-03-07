using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class CoinEffect : BaseEntity
    {

        public CoinEffect(Game game, int x, int y) :
            base(game, "coin_0", x, y, new Vector2(0, -1), null, 32)
        {
            SosEngine.Core.PlaySound("Coin");
            this.IsRepeatable = false;
            this.Position = new Vector2(Position.X - 2, Position.Y);
            for (int i = 0; i < 8; i++)
            {
                AddFrame(string.Format("coin_{0}", i + 1), 32);
            }
        }

        protected override Rectangle GetBoundingBox()
        {
            return Rectangle.Empty;
        }

        public override void Update(GameTime gameTime)
        {
            Position = Position + speed;
            if (CompletedAFullCycle)
            {
                IsFinished = true;
            }
            base.Update(gameTime);
        }

    }
}
