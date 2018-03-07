using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine.Tasks
{

    /// <summary>
    /// Positions Sprite at specified position
    /// </summary>
    public class PositionSpriteTask : Task
    {
        private Sprite sprite; 
        private Vector2 position;

        public PositionSpriteTask(Sprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        public PositionSpriteTask(Sprite sprite, int x, int y)
        {
            this.sprite = sprite;
            this.position = new Vector2(x, y);
        }

        public override void Update(GameTime gameTime)
        {
            if (isFinished)
            {
                return;
            }
            sprite.Position = position;
            isFinished = true;
        }

    }

}
