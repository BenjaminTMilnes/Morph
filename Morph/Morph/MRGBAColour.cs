using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public enum RGBAColourForm
    {
        Decimal = 1,
        Hexadecimal = 2
    }

    /// <summary>
    /// Represents an RGBA colour in Morph.
    /// </summary>
    public class MRGBAColour : MColour
    {
        public IMNumeric R { get; set; }
        public IMNumeric G { get; set; }
        public IMNumeric B { get; set; }
        public IMNumeric A { get; set; }
        public RGBAColourForm Form { get; set; }

        public MRGBAColour(IMNumeric r, IMNumeric g, IMNumeric b, IMNumeric a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
            Form = RGBAColourForm.Decimal;
        }

        public override string ToString()
        {
            if (Form == RGBAColourForm.Hexadecimal && R is MNumber && G is MNumber && B is MNumber && A is MNumber)
            {
                return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (int)R.Value, (int)G.Value, (int)B.Value, (int)A.Value);
            }
            else
            {
                return string.Format("rgba({0}, {1}, {2}, {3})", R, G, B, A);
            }
        }
    }
}
