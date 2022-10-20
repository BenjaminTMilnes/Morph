using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph length unit. Morph length units are a subset of physical length units that are useful for things on the scale of printed documents.
    /// 
    /// The allowed values are:
    /// 
    /// 'mm' - millimetres
    /// 'cm' - centimetres
    /// 'dm' - decimetres
    /// 'm' - metres
    /// 'in' - inches
    /// 'pt' - points
    /// </summary>
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
