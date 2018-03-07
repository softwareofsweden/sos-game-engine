using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine.Tasks
{
    public class ScaleSpriteTask : Task
    {
        private Sprite sprite; 
        private Vector2 targetScale;
        private Vector2 initialScale;
        private int duration;
        private int durationLeft;
        private bool checkInitialScale;

        public ScaleSpriteTask(Sprite sprite, Vector2 scale, int duration)
        {
            this.sprite = sprite;
            this.targetScale = scale;
            this.duration = duration;
            this.durationLeft = duration;
            this.checkInitialScale = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (isFinished)
            {
                return;
            }
            if (checkInitialScale)
            {
                checkInitialScale = false;
                initialScale = new Vector2(sprite.ScaleX, sprite.ScaleY);
            }
            durationLeft -= gameTime.ElapsedGameTime.Milliseconds;
            if (durationLeft < 0)
            {
                durationLeft = 0;
            }
            if (duration > 0)
            {
                int elapsed = duration - durationLeft;
                float progress = elapsed / (float)duration;
                float scaleX = MathHelper.SmoothStep(initialScale.X, targetScale.X, progress);
                float scaleY = MathHelper.SmoothStep(initialScale.Y, targetScale.Y, progress);
                sprite.ScaleX = scaleX;
                sprite.ScaleY = scaleY;
            }
            if (durationLeft <= 0)
            {
                sprite.ScaleX = targetScale.X;
                sprite.ScaleY = targetScale.Y;
                durationLeft = duration;
                checkInitialScale = true;
                isFinished = true;
            }
        }
    }
}
