using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosEngine
{

    /// <summary>
    /// Handles sprite frame cache.
    /// </summary>
    public class SpriteFrameCache
    {

        /// <summary>
        /// Cache for sprite frames.
        /// </summary>
        protected List<SpriteFrame> cache;

        public SpriteFrameCache()
        {
            cache = new List<SpriteFrame>();
        }

        /// <summary>
        /// Add sprite frame to cache.
        /// </summary>
        /// <param name="spriteFrame"></param>
        public void AddSpriteFrame(SpriteFrame spriteFrame)
        {
            cache.Add(spriteFrame);
        }

        /// <summary>
        /// Get sprite frame by specified name.
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        public SpriteFrame GetSpriteFrame(string frameName)
        {
            SpriteFrame spriteFrame = cache.Find(x => x.FrameName == frameName);
            if (spriteFrame == null)
            {
                throw new Exception(string.Format("Invalid sprite frame name: {0}", frameName));
            }
            return spriteFrame;
        }

        /// <summary>
        /// Check if a sprite frame with the specified name already exists.
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        public bool SpriteFrameExists(string frameName)
        {
            return cache.Exists(x => x.FrameName == frameName);
        }

    }
}
