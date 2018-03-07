using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class Fireball : BaseEntity
    {

        protected int animationCount;

        public Fireball(Game game, int x, int y, Vector2 speed, SosEngine.Level level)
            : base(game, "fireball", x, y, speed, level)
        {
            hasGravity = true;
        }

        public void Explode()
        {
            EntityManager.AddEntity(new FireballExplodeEffect(Game, (int)Math.Round(Position.X) - 4, (int)Math.Round(Position.Y) - 4));
            IsFinished = true;
        }

        public override void Update(GameTime gameTime)
        {

            animationCount++;
            if (animationCount > 12)
            {
                animationCount = 0;
            }

            // 0, 1, 3
            // No flip
            // 4, 5, 6
            Flipped = animationCount > 3 && animationCount < 10;
            // 7, 8, 9
            FlippedVertically = animationCount > 6;
            // 10, 11, 12


            if ((speed.X < 0 && !CanMoveLeft()) || (speed.X > 0 && !CanMoveRight()))
            {
                Explode();
            }
            if (IsGrounded())
            {
                BeginJump();
            }
            Position += speed;
            base.Update(gameTime);
        }

    }
}
