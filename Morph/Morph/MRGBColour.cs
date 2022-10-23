using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents an RGB colour in Morph. An RGB colour is just an RGBA colour where a = 0.
    /// </summary>
    public class MRGBColour : MRGBAColour
    {
        public MRGBColour(IMNumeric r, IMNumeric g, IMNumeric b) : base(r, g, b, new MNumber("0")) { }

        public override string ToString()
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", R, G, B);
        }
    }
}
