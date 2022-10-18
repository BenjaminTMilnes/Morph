using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MLengthSet
    {
        public IList<MLength> Lengths { get; set; }

        public MLengthSet()
        {
            Lengths = new List<MLength>();
        }

        public override string ToString()
        {
            return string.Join(" ", Lengths);
        }
    }
}
