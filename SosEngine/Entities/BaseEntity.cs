using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SosEngine.Entities
{


    public abstract class BaseEntity : SosEngine.Sprite
    {

        public SosEngine.Animation IdleAnimation
        {
            get;
            set;
        }

        protected enum MovingDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        protected enum EntityState
        {
            Walk,
            Jump,
            Crouch,
            Idle,
            Climb,
            Hurt,
            Fire
        }

        /// <summary>
        /// The speed.
        /// </summary>
        protected Vector2 speed;

        /// <summary>
        /// The moving direction.
        /// </summary>
        protected MovingDirection movingDirection;

        protected EntityState entityState;
        protected EntityState oldEntityState;

        protected Animation currentAnimation;

        protected SosEngine.Level level;

        BaseEntity(Game game) : base(game, "", 0, 0, null)
        {
            entityState = EntityState.Idle;
        }


        public override void Update(GameTime gameTime)
        {
            // Animation
            if (entityState == EntityState.Idle) {
                currentAnimation = IdleAnimation;
            }

        }

    }
}
