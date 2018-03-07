using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosEngine
{
    public static class StringHelper
    {

        public static string GetScoreString(int score, int totalWidth)
        {
            return score.ToString().PadLeft(totalWidth, '0');
        }

    }
}
