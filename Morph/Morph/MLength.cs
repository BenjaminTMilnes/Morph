using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph length. A length consists of a magnitude and a length unit.
    /// </summary>
    public class MLength
    {
        public MNumber Number { get; set; }
        public MLengthUnit Unit { get; set; }

        public MLength(MNumber number, MLengthUnit unit)
        {
            Number = number;
            Unit = unit;
        }

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
