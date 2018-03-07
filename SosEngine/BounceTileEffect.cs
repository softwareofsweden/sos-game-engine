using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosEngine
{
    public class BounceTileEffect : TileEffect
    {
        protected Rectangle originalTarget;
        protected Stack<int> offsets;

        public BounceTileEffect(TileData tileData, string layerName, int tileX, int tileY)
            : base(tileData, layerName, tileX, tileY)
        {
            this.offsets = new Stack<int>();
            this.offsets.Push(2);
            this.offsets.Push(1);
            this.offsets.Push(1);
            this.offsets.Push(0);
            this.offsets.Push(-1);
            this.offsets.Push(-2);
            this.offsets.Push(-1);
            this.originalTarget = tileData.Target;
        }

        public override void Update(GameTime gameTime)
        {
            this.tileData.Target.Offset(0, this.offsets.Pop());
            if (this.offsets.Count == 0)
            {
                this.tileData.Target = originalTarget;
                this.isFinished = true;
            }
        }
    }
}
