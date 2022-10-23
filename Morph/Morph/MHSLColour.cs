using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a HSL colour in Morph. A HSL colour is just a HSLA colour where a = 0.
    /// </summary>
    public class MHSLColour : MHSLAColour
    {
        public MHSLColour(IMNumeric h, IMNumeric s, IMNumeric l) : base(h, s, l, new MNumber("0")) { }

        public override string ToString()
        {
            return string.Format("hsl({0}, {1}, {2})", H, S, L);
        }
    }
}
