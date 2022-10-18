using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MPercentage
    {
        public double Value { get; set; }

        public MPercentage(double value = 0.0)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}%", Value * 100);
        }
    }
}
