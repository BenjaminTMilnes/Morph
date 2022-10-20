using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a percentage in a Morph document.
    /// </summary>
    public class MPercentage
    {
        /// <summary>
        /// The value of this percentage, represented as a double between 0.0 and 1.0.
        /// </summary>
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
