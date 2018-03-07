using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosEngine
{
    public abstract class TileEffect
    {
        /// <summary>
        /// True if effect is finished
        /// </summary>
        public bool IsFinished
        {
            get { return isFinished; }
            set { isFinished = value; }
        }
        protected bool isFinished;

        protected TileData tileData;

        public int TileX { get; internal set; }
        public int TileY { get; internal set; }
        public string LayerName { get; internal set; }

        public abstract void Update(GameTime gameTime);

        public TileEffect(TileData tileData, string layerName, int tileX, int tileY)
        {
            this.LayerName = layerName;
            this.TileX = tileX;
            this.TileY = tileY;
            this.tileData = tileData;
        }
    }
}
