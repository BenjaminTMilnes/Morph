using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph length set. A length set is a list of lengths, such as '2cm 2cm 4cm 2cm'. These are used for specifying things like margins.
    /// </summary>
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
