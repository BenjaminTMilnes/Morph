using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MHSLAColour : MColour
    {
        public int H { get; set; }
        public int S { get; set; }
        public int L { get; set; }
        public int A { get; set; }

        public MHSLAColour(int h = 0, int s = 0, int l = 0, int a = 0)
        {
            H = h;
            S = s;
            L = l;
            A = a;
        }

        public override string ToString()
        {
            return string.Format("hsla({0}, {1}, {2}, {3})", H, S, L, A);
        }
    }
}
