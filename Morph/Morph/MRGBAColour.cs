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
        public IMNumeric R { get; set; }
        public IMNumeric G { get; set; }
        public IMNumeric B { get; set; }
        public IMNumeric A { get; set; }

        public MRGBAColour(IMNumeric r, IMNumeric g, IMNumeric b, IMNumeric a)
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
