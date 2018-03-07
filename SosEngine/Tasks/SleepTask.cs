using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine.Tasks
{

    /// <summary>
    /// Sleep for specified time in milliseconds
    /// </summary>
    public class SleepTask : Task
    {
        private int originalSleepTime;
        private int sleepTime;

        public SleepTask(int milliseconds)
        {
            this.originalSleepTime = milliseconds;
            this.sleepTime = milliseconds;
        }

        public override void Update(GameTime gameTime)
        {
            if (isFinished)
            {
                return;
            }
            sleepTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (sleepTime <= 0)
            {
                sleepTime = originalSleepTime;
                isFinished = true;
            }
        }

    }

}
