using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
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
