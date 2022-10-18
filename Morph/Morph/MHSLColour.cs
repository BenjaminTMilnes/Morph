using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MHSLColour : MHSLAColour
    {
        public MHSLColour(int h = 0, int s = 0, int l = 0) : base(h, s, l, 0) { }

        public override string ToString()
        {
            return string.Format("hsla({0}, {1}, {2})", H, S, L);
        }
    }
}
