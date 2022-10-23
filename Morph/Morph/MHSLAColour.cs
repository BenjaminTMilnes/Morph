using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a HSLA colour in Morph.
    /// </summary>
    public class MHSLAColour : MColour
    {
        public IMNumeric H { get; set; }
        public IMNumeric S { get; set; }
        public IMNumeric L { get; set; }
        public IMNumeric A { get; set; }

        public MHSLAColour(IMNumeric h, IMNumeric s, IMNumeric l, IMNumeric a)
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
