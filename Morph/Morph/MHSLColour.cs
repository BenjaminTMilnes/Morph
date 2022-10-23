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
        public MHSLColour(double h = 0, double s = 0, double l = 0) : base(h, s, l, 0) { }

        public override string ToString()
        {
            return string.Format("hsl({0}, {1:F3}, {2:F3})", H, S, L);
        }
    }
}
