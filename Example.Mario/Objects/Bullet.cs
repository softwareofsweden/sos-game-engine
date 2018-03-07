using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class Bullet : Enemy
    {

        public Bullet(Game game, int x, int y, int playerX, SosEngine.Level level) : 
            base (game, "bullet_1", x, y, Vector2.Zero, level, 80)
        {
            AddFrame("bullet_2", 80);
            AddFrame("bullet_3", 80);
            AddFrame("bullet_2", 80);
            if (playerX < x + level.GetScrollX())
            {
                Position += new Vector2(-24, 0);
                speed = new Vector2(-1.8f, 0);
            } else
            {
                Position += new Vector2(8, 0);
                speed = new Vector2(1.8f, 0);
                Flipped = true;
            }
            IsActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            Position += speed;
            if (!IsVisibleOnScreen())
            {
                IsFinished = true;
            }
            base.Update(gameTime);
        }

    }
}
