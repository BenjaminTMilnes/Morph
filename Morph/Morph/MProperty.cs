using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph style property. 
    /// </summary>
    public class MProperty
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public MProperty(string name = "", object value = null)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1};", Name.Trim(), Value.ToString().Trim());
        }
    }
}
