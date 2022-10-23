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
    /// 'pw' - multiples of the page width
    /// 'ph' - multiples of the page height 
    /// 'em' - multiples of the current font height in points
    /// 'en' - multiples of one half of 1em
    /// 'rem' - multiples of the root element font height in points - same as CSS rem
    /// 'ren' - multiples of one half of the root element font height in points - 1ren = 0.5rem
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
