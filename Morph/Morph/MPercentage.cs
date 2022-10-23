using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a percentage in a Morph document.
    /// </summary>
    public class MPercentage : IMNumeric
    {
        public string Text { get; set; }

        /// <summary>
        /// The value of this percentage, represented as a double between 0.0 and 1.0.
        /// </summary>
        public double Value
        {
            get
            {
                return double.Parse(Text) / 100.0;
            }
        }

        public MPercentage(string text = "")
        {
            Text = text;
        }

        public override string ToString()
        {
            return string.Format("{0}%", Text);
        }
    }
}
