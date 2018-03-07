using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class BreakBlockEffect : BaseEntity
    {

        public enum Placement
        {
            UpperLeft,
            UpperRight,
            LowerLeft,
            LowerRight
        }

        protected int animationCount;

        public BreakBlockEffect(Game game, string spriteFrameName, int x, int y, Placement placement) :
            base(game, spriteFrameName, x, y, new Vector2(1, 0), null, 32)
        {
            this.hasGravity = true;
            switch (placement)
            {
                case Placement.UpperLeft:
                    this.Position = new Vector2(Position.X, Position.Y);
                    this.movingDirection = MovingDirection.Left;
                    this.initialJumpSpeed = 4f;
                    this.maxJumpCount = 10;
                    break;
                case Placement.UpperRight:
                    this.Position = new Vector2(Position.X + 8, Position.Y);
                    this.movingDirection = MovingDirection.Right;
                    this.initialJumpSpeed = 4f;
                    this.maxJumpCount = 10;
                    break;
                case Placement.LowerLeft:
                    this.Position = new Vector2(Position.X, Position.Y + 8);
                    this.movingDirection = MovingDirection.Left;
                    this.initialJumpSpeed = 2f;
                    this.maxJumpCount = 8;
                    break;
                case Placement.LowerRight:
                    this.Position = new Vector2(Position.X + 8, Position.Y + 8);
                    this.movingDirection = MovingDirection.Right;
                    this.initialJumpSpeed = 2f;
                    this.maxJumpCount = 8;
                    break;
            }
            BeginJump();
        }

        protected override bool CanFall()
        {
            return !isJumping;
        }

        protected override Rectangle GetBoundingBox()
        {
            return Rectangle.Empty;
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

            if (movingDirection == MovingDirection.Left)
            {
                Position = Position - speed;
            }
            else
            {
                Position = Position + speed;
            }
            if (Position.Y > SosEngine.Core.RenderHeight)
            {
                IsFinished = true;
            }            
            base.Update(gameTime);
        }

    }
}
