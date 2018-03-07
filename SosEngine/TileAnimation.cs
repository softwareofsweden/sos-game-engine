using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosEngine
{
    public class TileAnimation
    {
        public string Layer { get; set; }
        public int[] Sequence { get; set; }
        public int Index { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
