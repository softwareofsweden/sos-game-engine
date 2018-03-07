using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine.Tasks
{

    /// <summary>
    /// Generic task for executing code
    /// </summary>
    public class ActionTask : Task
    {
        private Action action;

        public ActionTask(Action action)
        {
            this.action = action;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isFinished)
            {
                action.Invoke();
                isFinished = true;
            }
        }

    }

}
