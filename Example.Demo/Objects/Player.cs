using System;
using Microsoft.Xna.Framework;

namespace Example_Demo.MacOS.Objects
{
    public class Player : SosEngine.Sprite
    {

        protected bool controlLeft;
        protected bool controlRight;
        protected bool controlUp;
        protected bool controlDown;

        protected string direction;

        protected int animFrameIndex;
        protected int animMilliseconds;

        public int Speed
        {
            get;
            set;
        }

        public Player(Game game, string spriteFrameName) :
            base(game, spriteFrameName)
        {
            direction = "down";
            animFrameIndex = 0;
            animMilliseconds = 0;
            Speed = 1;
        }

        /// <summary>
        /// Set states of inputs controlling the player.
        /// </summary>
        /// <param name="controlLeft"></param>
        /// <param name="controlRight"></param>
        /// <param name="controlUp"></param>
        /// <param name="controlDown"></param>
        public void SetControls(bool controlLeft, bool controlRight, bool controlUp, bool controlDown)
        {
            this.controlLeft = controlLeft;
            this.controlRight = controlRight;
            this.controlUp = controlUp;
            this.controlDown = controlDown;
        }

        /// <summary>
        /// Update player
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var moving = false;
            if (controlLeft)
            {
                Position = new Vector2(Position.X - Speed, Position.Y);
                direction = "left";
                moving = true;
            }
            if (controlRight)
            {
                Position = new Vector2(Position.X + Speed, Position.Y);
                direction = "right";
                moving = true;
            }
            if (controlUp)
            {
                Position = new Vector2(Position.X, Position.Y - Speed);
                direction = "up";
                moving = true;
            }
            if (controlDown)
            {
                Position = new Vector2(Position.X, Position.Y + Speed);
                direction = "down";
                moving = true;
            }

            // Animate
            if (moving)
            {
                animMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (animMilliseconds >= 100)
                {
                    animMilliseconds = 0;
                    animFrameIndex++;
                    if (animFrameIndex > 3)
                    {
                        animFrameIndex = 0;
                    }
                }
            }
            else
            {
                animFrameIndex = 0;
            }

            SetSpriteFrame(direction + "_" + animFrameIndex.ToString());

        }

    }
}
