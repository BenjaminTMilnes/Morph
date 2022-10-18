﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MRGBColour : MRGBAColour
    {
        public MRGBColour(int r = 0, int g = 0, int b = 0) : base(r, g, b, 0) { }

        public override string ToString()
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", R, G, B);
        }
    }
}
