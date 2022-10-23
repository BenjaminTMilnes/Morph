using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a decimal number in a Morph document. This class just acts as a container for a number written as a string.
    /// </summary>
    public class MNumber : IMNumeric
    {
        public string Text { get; set; }

        public double Value
        {
            get
            {
                return double.Parse(Text);
            }
        }

        public MNumber(string text = "")
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text.Trim();
        }
    }
}
