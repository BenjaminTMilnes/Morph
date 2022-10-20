using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents an RGBA colour in Morph.
    /// </summary>
    public class MRGBAColour : MColour
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public MRGBAColour(int r = 0, int g = 0, int b = 0, int a = 0)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override string ToString()
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", R, G, B, A);
        }
    }
}
