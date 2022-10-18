using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MLengthUnit
    {
        public string Value { get; set; }

        public MLengthUnit(string value = "")
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.Trim();
        }
    }
}
