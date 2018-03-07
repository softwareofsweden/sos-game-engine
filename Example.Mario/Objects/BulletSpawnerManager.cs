using Microsoft.Xna.Framework;
using SosEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class BulletSpawnerManager : GameComponent
    {
        EntityManager entityManager;
        List<BulletSpawner> bulletSpawners;

        public BulletSpawnerManager(Game game, Level level, EntityManager entityManager) :
            base (game)
        {
            this.entityManager = entityManager;
            this.bulletSpawners = new List<BulletSpawner>();
            var spawnlocations = level.GetTilePositionsForBlock("Block", 165);
            foreach(var spawnLocation in spawnlocations)
            {
                this.bulletSpawners.Add(new BulletSpawner(game, level, spawnLocation.X, spawnLocation.Y));
            }
        }

        public void Update(GameTime gameTime, EntityManager entityManager)
        {
            for (int i = 0; i < bulletSpawners.Count; i++)
            {
                bulletSpawners[i].Update(gameTime, entityManager);
            }
            base.Update(gameTime);
        }

    }
}

