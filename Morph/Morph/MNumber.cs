using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a decimal number in a Morph document. This class just acts as a container for a number written as a string.
    /// </summary>
    public class MNumber
    {
        public string Value { get; set; }

        public MNumber(string value = "")
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.Trim();
        }
    }
}
