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
        public double H { get; set; }
        public double S { get; set; }
        public double L { get; set; }
        public double A { get; set; }

        public MHSLAColour(double h = 0, double s = 0, double l = 0, double a = 0)
        {
            H = h;
            S = s;
            L = l;
            A = a;
        }

        public override string ToString()
        {
            return string.Format("hsla({0}, {1:F3}, {2:F3}, {3:F3})", H, S, L, A);
        }
    }
}
