using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
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
