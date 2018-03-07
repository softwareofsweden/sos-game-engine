using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine.Tasks
{

    /// <summary>
    /// Base class for Tasks
    /// </summary>
    public abstract class Task
    {
        /// <summary>
        /// True if task is finished
        /// </summary>
        public bool IsFinished
        {
            get { return isFinished; }
            set { isFinished = value; }
        }
        protected bool isFinished;

        public abstract void Update(GameTime gameTime);

    }

}
