using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a CMYK colour in Morph.
    /// </summary>
    public class MCMYKColour : MColour
    {
        public IMNumeric C { get; set; }
        public IMNumeric M { get; set; }
        public IMNumeric Y { get; set; }
        public IMNumeric K { get; set; }

        public MCMYKColour(IMNumeric c, IMNumeric m, IMNumeric y, IMNumeric k)
        {
            C = c;
            M = m;
            Y = y;
            K = k;
        }

        public override string ToString()
        {
            return string.Format("cmyk({0}, {1}, {2}, {3})", C, M, Y, K);
        }
    }
}
