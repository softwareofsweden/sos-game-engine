using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class EntityManager : SosEngine.SpriteGroup, SosEngine.IConsoleCommand
    {

        private static int GoombaBlock = 33;

        protected Game game;
        protected SosEngine.Level level;
        protected GameComponentCollection gameComponents;
        protected BulletSpawnerManager bulletSpawnerManager;

        protected List<BaseEntity> newEntitiesQueue;

        protected bool isLocked;

        public int PlayerX { get; set; }

        public EntityManager(Game game, SosEngine.Level level) : base(game)
        {
            this.newEntitiesQueue = new List<BaseEntity>();
            this.game = game;
            this.level = level;

            this.bulletSpawnerManager = new BulletSpawnerManager(game, level, this);

            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    int block = level.GetBlock("Items", x, y);
                    if (block == EntityManager.GoombaBlock)
                    {
                        level.RemoveBlock("Items", x, y);
                        AddEntity(new Goomba(game, x * 16, y * 16, level));
                    }
                }
            }

        }

        public void AddEntity(BaseEntity entity)
        {
            if (isLocked)
            {
                newEntitiesQueue.Add(entity);
            }
            else
            {
                entity.EntityManager = this;
                sprites.Add(entity);
            }
        }

        public void SetDrawOffset(int offset)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].DrawOffsetX = offset;
            }
        }

        public override void Update(GameTime gameTime)
        {
            isLocked = true;

            // Activate enemies within range
            foreach(var enemy in sprites.OfType<Enemy>().Where(x => !x.IsActive))
            {
                if (enemy.Position.X + level.GetScrollX() - SosEngine.Core.RenderWidth - (16 * 4) < 0)
                {
                    enemy.IsActive = true;
                }
            }

            var activeEnemies = sprites.OfType<Enemy>().Where(x => x.IsActive && x.CanCollideWithOtherEnemy);

            // Check if enemies collide with other enemies
            foreach (var enemy1 in activeEnemies)
            {
                foreach (var enemy2 in activeEnemies)
                {
                    if (enemy1 != enemy2)
                    {
                        enemy1.CollideWithEnemy(enemy2);
                    }
                }
            }

            // Check if enemies collide with fireballs
            var fireballs = sprites.OfType<Fireball>();
            foreach (var fireball in fireballs)
            {
                foreach (var enemy in activeEnemies)
                {
                    if (enemy.CollideWithFireball(fireball))
                    {
                        fireball.Explode();
                    }
                }
            }

            foreach (var sprite in sprites)
            {
                sprite.Update(gameTime);
            }
            sprites.RemoveAll(x => ((BaseEntity)x).IsFinished);

            bulletSpawnerManager.Update(gameTime, this);

            isLocked = false;

            foreach (var entity in newEntitiesQueue)
            {
                AddEntity(entity);
            }
            newEntitiesQueue.Clear();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(var sprite in sprites)
            {
                sprite.Draw(gameTime);
            }
            base.Draw(gameTime);
        }


        public void ConsoleExecute(string command, params string[] args)
        {
            if (command == "bullet")
            {
                AddEntity(new Bullet(game, 160, 100, 200, level));
            }
            foreach (var sprite in sprites)
            {
                if (sprite is SosEngine.IConsoleCommand)
                {
                    ((SosEngine.IConsoleCommand)sprite).ConsoleExecute(command, args);
                }
            }
        }
    }
}
