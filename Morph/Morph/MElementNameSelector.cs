using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph element name selector.
    /// </summary>
    public class MElementNameSelector : IMSelector
    {
        public string ElementName { get; set; }

        public MElementNameSelector(string elementName = "")
        {
            ElementName = elementName;
        }

        public override string ToString()
        {
            return ElementName;
        }
    }
}
