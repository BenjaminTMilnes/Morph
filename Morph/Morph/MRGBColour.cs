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
            if (Form == RGBAColourForm.Hexadecimal && R is MNumber && G is MNumber && B is MNumber && A is MNumber)
            {
                return string.Format("#{0:X2}{1:X2}{2:X2}", (int)R.Value, (int)G.Value, (int)B.Value);
            }
            else
            {
                return string.Format("rgb({0}, {1}, {2})", R, G, B);
            }
        }
    }
}
