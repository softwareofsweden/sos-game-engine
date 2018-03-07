using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{

    /// <summary>
    /// Base class for enemies.
    /// </summary>
    public class Enemy : BaseEntity
    {

        public bool IsActive { get; set; }
        public bool IsKilled { get; set; }
        public bool CanBeKilledByFireball { get; set; }
        public bool CanCollideWithOtherEnemy { get; set; }

        public Enemy(Game game, string spriteFrameName, int x, int y, Vector2 speed, SosEngine.Level level, int delay = 0) :
            base(game, spriteFrameName, x, y, speed, level, delay)
        {
            IsActive = false;
            IsKilled = false;
            CanBeKilledByFireball = true;
            CanCollideWithOtherEnemy = true;
        }

        public virtual Rectangle GetFragileBounds()
        {
            Rectangle rect = GetBoundingBox();
            rect.Offset(level.GetScrollX(), 0);
            return rect;
        }

        public virtual void CollideWithPlayer(Player player)
        {
        }

        public virtual void KilledByFireball()
        {
        }

        public bool CollideWithFireball(Fireball fireball)
        {
            if (IsActive && !IsKilled && CanBeKilledByFireball)
            {
                if (fireball.BoundingBox.Intersects(this.BoundingBox))
                {
                    IsKilled = true;
                    KilledByFireball();
                    return true;
                }
            }
            return false;
        }

        public virtual void CollideWithEnemy(Enemy other)
        {
            if (Position.Y == other.Position.Y && movingDirection != other.movingDirection)
            {
                if (movingDirection == MovingDirection.Left)
                {
                    if (other.BoundingBox.Contains(Position.X - 1, Position.Y))
                    {
                        movingDirection = MovingDirection.Right;
                        other.movingDirection = MovingDirection.Left;
                    }
                } 
                else if (movingDirection == MovingDirection.Right)
                {
                    if (other.BoundingBox.Contains(Position.X + BoundingBox.Width + 1, Position.Y))
                    {
                        movingDirection = MovingDirection.Left;
                        other.movingDirection = MovingDirection.Right;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsActive)
            {
                base.Draw(gameTime);
            }
        }

    }

}
