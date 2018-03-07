using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosEngine
{
    public class LevelObject
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Rectangle Bounds { get; set; }

        public LevelObject(string name, int x, int y, Rectangle bounds)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Bounds = bounds;
        }
    }
}
