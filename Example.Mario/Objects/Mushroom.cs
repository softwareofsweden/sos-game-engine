using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class Mushroom : BaseEntity
    {

        public enum MushroomTypes
        {
            Mushroom,
            OneUp,
            Deadly
        }

        public MushroomTypes MushroomType { get; set; }

        protected bool isAppearing;
        protected int moveCount;

        public Mushroom(Game game, string spriteFrameName, MushroomTypes mushroomType, int x, int y, SosEngine.Level level) :
            base(game, spriteFrameName, x, y, new Vector2(1, 0), level)
        {
            SosEngine.Core.PlaySound("ItemSprout");
            this.MushroomType = mushroomType;
            this.isAppearing = true;
            this.movingDirection = MovingDirection.Right;
            this.hasGravity = true;
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

        protected override bool CanFall()
        {
            return !IsGrounded() && !isAppearing;
        }

        public override void Update(GameTime gameTime)
        {
            if (isAppearing)
            {
                Position = new Vector2(Position.X, Position.Y - 1);
                ClipBottomAmount = (int)Position.Y % 16;
                if (Position.Y % 16 == 0)
                {
                    ClipBottomAmount = 0;
                    isAppearing = false;
                }
            }
            else
            {
                if (moveCount < 100)
                {
                    moveCount++;
                }
                if (movingDirection == MovingDirection.Left)
                {
                    if (CanMoveLeft())
                    {
                        Position = Position - speed;
                    } else
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

                if (IsGrounded() && BlockBelowIsBounced())
                {
                    movingDirection = (Position.X % 16 < 8) ? MovingDirection.Right : MovingDirection.Left;
                    BeginJump();
                }

            }
            base.Update(gameTime);
        }

    }
}
