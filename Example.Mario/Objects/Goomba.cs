using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class Goomba : Enemy, SosEngine.IConsoleCommand
    {
        protected int animationCount;
        protected int killedCount;
        protected bool killedByStomp;
        protected bool killedByBlockBounce;

        public Goomba(Game game, int x, int y, SosEngine.Level level) :
            base(game, "goomba", x, y, new Vector2(0.6f, 0), level, 100)
        {
            this.hasGravity = true;
            this.movingDirection = MovingDirection.Left;
        }

        protected void KillByStomp()
        {
            killedCount = 0;
            IsKilled = true;
            killedByStomp = true;
            SetSpriteFrame("goomba_stomped");
        }

        protected void KillByBlockBounce()
        {
            IsKilled = true;
            killedByBlockBounce = true;
            FlippedVertically = true;
            BeginJump();
        }

        public override void KilledByFireball()
        {
            base.KilledByFireball();
            KillByBlockBounce();
        }

        protected override bool CanFall()
        {
            if (killedByBlockBounce)
            {
                return true;
            }
            return base.CanFall();
        }

        public override void CollideWithPlayer(Player player)
        {
            if (!IsActive || IsKilled)
            {
                return;
            }
            if (player.CanJumpOnEnemy())
            {
                if (GetFragileBounds().Intersects(player.GetLethalBounds()))
                {
                    SosEngine.Core.PlaySound("Bump");
                    KillByStomp();
                    player.BounceOnEnemy();
                    player.AddScore(100, true, this.TopCenter);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            if (IsKilled)
            {
                killedCount++;
                if (killedByStomp)
                {
                    if (killedCount > 20)
                    {
                        IsFinished = true;
                    }
                }
                if (killedByBlockBounce)
                {
                    if (movingDirection == MovingDirection.Left)
                    {
                        Position = Position - speed;
                    } else
                    {
                        Position = Position + speed;
                    }
                    if (Position.Y > SosEngine.Core.RenderHeight)
                    {
                        IsFinished = true;
                    }
                }
            } 
            else
            {
                if (movingDirection == MovingDirection.Left)
                {
                    if (CanMoveLeft())
                    {
                        Position = Position - speed;
                    }
                    else
                    {
                        movingDirection = MovingDirection.Right;
                    }
                }
                if (movingDirection == MovingDirection.Right)
                {
                    if (CanMoveRight())
                    {
                        Position = Position + speed;
                    }
                    else
                    {
                        movingDirection = MovingDirection.Left;
                    }
                }

                animationCount++;
                if (animationCount > 9)
                {
                    animationCount = 0;
                }
                Flipped = animationCount > 4;

                if (BlockBelowIsBounced())
                {
                    KillByBlockBounce();
                }
            }

            base.Update(gameTime);
        }


        public void ConsoleExecute(string command, params string[] args)
        {
            if (command == "bomb")
            {
                KillByBlockBounce();
            }
        }
    }
}
