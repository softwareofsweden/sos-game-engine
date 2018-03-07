using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class BulletSpawner : GameComponent
    {
        protected SosEngine.Level level;
        public int X { get; internal set; }
        public int Y { get; internal set; }
        protected int cooldown;
        

        public BulletSpawner(Game game, SosEngine.Level level, int x, int y) :
            base(game)
        {
            this.level = level;
            this.X = x; // Center
            this.Y = y - 8; // Top
        }

        protected bool IsVisibleOnScreen()
        {
            var screenX = X + level.GetScrollX();
            if (screenX > 0 && screenX < SosEngine.Core.RenderWidth)
            {
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime, EntityManager entityManager)
        {
            if (cooldown > 0)
            {
                cooldown--;
            }

            if (!IsVisibleOnScreen())
            {
                return;
            }

            if (cooldown <= 0)
            {
                if (Math.Abs(entityManager.PlayerX + 16 - X - level.GetScrollX()) > 32)
                {
                    cooldown = 100;
                    var bullet = new Bullet(Game, X, Y, entityManager.PlayerX, level);
                    entityManager.AddEntity(bullet);
                    entityManager.AddEntity(new SmokeEffect(Game, (int)bullet.Position.X, (int)bullet.Position.Y));
                }
            }
            base.Update(gameTime);
        }
    }
}
