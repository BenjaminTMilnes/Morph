using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public MProperty(string name = "", string value = "")
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name.Trim(), Value.Trim());
        }
    }
}
