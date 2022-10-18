using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
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
