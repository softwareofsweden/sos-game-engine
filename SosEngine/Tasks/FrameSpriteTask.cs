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
    public class FrameSpriteTask : Task
    {
        private Sprite sprite; 
        private string frameName;

        public FrameSpriteTask(Sprite sprite, string frameName)
        {
            this.sprite = sprite;
            this.frameName = frameName;
        }

        public override void Update(GameTime gameTime)
        {
            if (isFinished)
            {
                return;
            }
            sprite.SetSpriteFrame(frameName);
            isFinished = true;
        }

    }

}
