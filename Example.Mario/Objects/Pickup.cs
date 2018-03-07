using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mario.Objects
{

    /// <summary>
    /// Item that can be picked up by player
    /// </summary>
    public class Pickup : SosEngine.AnimatedSprite
    {

        /// <summary>
        /// How much to increment player score when picking
        /// up this item
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// The block number for this item
        /// </summary>
        public int Block { get; set; }

        public Pickup(Game game, string spriteFrameName, int block, int x, int y, int delay, int score, SosEngine.SpriteFrameCache spriteFrameCache)
            : base(game, spriteFrameName, delay, spriteFrameCache)
        {
            this.Position = new Vector2(x, y);
            this.Block = block; 
            this.Score = score;
        }

        protected override Rectangle GetBoundingBox()
        {
            if (Visible)
            {
                return new Rectangle((int)Position.X + 6, (int)Position.Y + 2, 16 - 12, 16 - 4);
            }
            else
            {
                return Rectangle.Empty;
            }
        }

    }

}
