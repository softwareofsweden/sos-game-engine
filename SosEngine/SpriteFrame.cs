using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine
{

    /// <summary>
    /// Sprite frame with name, asset name and source rectangle.
    /// </summary>
    public class SpriteFrame
    {

        /// <summary>
        /// Name of sprite frame.
        /// </summary>
        public string FrameName { get; set; }

        /// <summary>
        /// Name of texture asset.
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Source rectangle.
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// Creates a new sprite.
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="frameName"></param>
        /// <param name="rectangle"></param>
        public SpriteFrame(string assetName, string frameName, Rectangle rectangle)
        {
            this.AssetName = assetName;
            this.FrameName = frameName;
            this.Rectangle = rectangle;
        }

    }
}
