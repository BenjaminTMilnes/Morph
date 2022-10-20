using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph style rule. A style rule consists of a list of selectors and a list of style properties.
    /// 
    /// When applied to a Graph document, the elements of the document will be filtered based on the selectors of the style rule, and the style properties will be applied to each matching element.
    /// </summary>
    public class MStyleRule
    {
        public IList<IMSelector> Selectors { get; set; }
        public IList<MProperty> Properties { get; set; }

        public MStyleRule()
        {
            Selectors = new List<IMSelector>();
            Properties = new List<MProperty>();
        }
    }
}
