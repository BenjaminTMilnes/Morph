using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MLength
    {
        public MNumber Number { get; set; }
        public MLengthUnit Unit { get; set; }

        public MLength(string number = "", string unit = "")
        {
            Number = new MNumber(number);
            Unit = new MLengthUnit(unit);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Number, Unit);
        }
    }
}
