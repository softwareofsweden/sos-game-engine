using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{

    /// <summary>
    /// Base class for entities such as enemies, powerups, effects etc.
    /// </summary>
    public abstract class BaseEntity : SosEngine.AnimatedSprite
    {
        protected Vector2 speed;
        protected SosEngine.Level level;
        protected MovingDirection movingDirection;
        protected float fallSpeed;
        protected bool hasGravity;

        protected float initialJumpSpeed;
        protected int maxJumpCount;
        protected bool isJumping;
        protected int jumpCount;
        protected float jumpSpeed;

        public EntityManager EntityManager { get; set; }

        public bool IsFinished { get; set; }

        protected enum MovingDirection
        {
            Left,
            Right,
        }

        public Vector2 TopCenter 
        { 
            get
            {
                return new Vector2(Position.X + (GetBoundingBox().Width / 2), Position.Y);
            }
        }

        public BaseEntity(Game game, string spriteFrameName, int x, int y, Vector2 speed, SosEngine.Level level, int delay = 0)
            : base(game, spriteFrameName, delay)
        {
            this.Position = new Vector2(x, y);
            this.speed = speed;
            this.level = level;
            this.fallSpeed = 1f;
            this.maxJumpCount = 8;
            this.initialJumpSpeed = 3f;
        }

        protected bool IsVisibleOnScreen()
        {
            var screenX = Position.X + level.GetScrollX();
            if (screenX + BoundingBox.Width > 0 && screenX < SosEngine.Core.RenderWidth)
            {
                return true;
            }
            return false;
        }

        protected bool BlockBelowIsBounced()
        {
            int x = (int)Position.X + spriteFrame.Rectangle.Width / 2;
            int y = (int)Position.Y + spriteFrame.Rectangle.Height;
            int bx;
            int by;
            if (level.GetBlockAtPixel("Block", x + level.GetScrollX(), y + 1, out bx, out by) != 0)
            {
                if (level.IsBlockBouncing("Block", bx, by))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual bool CanMoveLeft()
        {
            int x = GetBoundingBox().Left + 1;
            int y = GetBoundingBox().Bottom - 1;
            if (Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x + level.GetScrollX(), y)))
            {
                return false;
            }
            return true;
        }

        protected virtual bool CanMoveRight()
        {
            int x = GetBoundingBox().Right - 1;
            int y = GetBoundingBox().Bottom - 1;
            if (Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x + level.GetScrollX(), y)))
            {
                return false;
            }
            return true;
        }

        protected virtual bool IsGrounded()
        {
            if (isJumping)
            {
                return false;
            }

            int x = GetBoundingBox().Left + (GetBoundingBox().Width / 2);
            int y = GetBoundingBox().Bottom;

            if (y % 16 != 0)
            {
                return false;
            }

            if (level != null)
            {
                int block1 = level.GetBlockAtPixel("Block", x - 5 + level.GetScrollX(), y + 1);
                int block2 = level.GetBlockAtPixel("Block", x + 5 + level.GetScrollX(), y + 1);
                return Helpers.LevelHelper.IsWall(block1) || Helpers.LevelHelper.IsWall(block2);
            }

            return true;
        }

        protected virtual bool CanFall()
        {
            return !IsGrounded();
        }

        protected void ApplyGravity()
        {
            int fallCount = (int)Math.Ceiling(fallSpeed);
            if (fallCount > 3)
            {
                fallCount = 3;
            }
            for (int i = 0; i < fallCount; i++)
            {
                if (CanFall())
                {
                    Position = new Vector2(Position.X, Position.Y + (fallSpeed / (float)fallCount));
                    fallSpeed = fallSpeed * 1.07f;
                    if (fallSpeed > 3f)
                    {
                        fallSpeed = 3f;
                    }
                }
                else
                {
                    Position = new Vector2(Position.X, (float)Math.Round(Position.Y));
                    fallSpeed = 1f;
                    break;
                }
            }
        }

        public void BeginJump(bool fromAirplane = false)
        {
            jumpSpeed = initialJumpSpeed;
            isJumping = true;
            jumpCount = 0;
        }

        protected void Jump()
        {
            Position = new Vector2(Position.X, Position.Y - jumpSpeed);
            jumpSpeed = jumpSpeed * 0.963f;
            jumpCount++;
            if (jumpCount > maxJumpCount)
            {
                Position = new Vector2(Position.X, (float)Math.Round(Position.Y));
                isJumping = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (hasGravity && !isJumping)
            {
                ApplyGravity();
            }
            if (isJumping)
            {
                Jump();
            }
            base.Update(gameTime);
        }

    }
}
