using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine.Tasks
{

    /// <summary>
    /// Sequence of tasks to be executed
    /// </summary>
    public class TaskSequence : Task
    {
        private List<Task> tasks;
        private int currentTaskIndex;
        private bool loop;

        /// <summary>
        /// Creates a seuqence of tasks to be executed
        /// </summary>
        /// <param name="loop">True to repeat task forever</param>
        public TaskSequence(bool loop = false)
        {
            this.tasks = new List<Task>();
            this.currentTaskIndex = 0;
            this.loop = loop;
        }

        /// <summary>
        /// Add task to sequence
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public override void Update(GameTime gameTime)
        {
            if (isFinished)
            {
                return;
            }
            if (currentTaskIndex < tasks.Count)
            {
                tasks[currentTaskIndex].Update(gameTime);
                if (tasks[currentTaskIndex].IsFinished)
                {
                    if (currentTaskIndex + 1 < tasks.Count)
                    {
                        currentTaskIndex++;
                    }
                    else
                    {
                        if (loop)
                        {
                            for (int i = 0; i < tasks.Count; i++)
                            {
                                tasks[i].IsFinished = false;
                            }
                            currentTaskIndex = 0;
                        }
                        else
                        {
                            isFinished = true;
                        }
                    }
                }
            }
        }

    }

}
