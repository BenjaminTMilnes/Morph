using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph id selector. These work in the same way as CSS id selectors.
    /// </summary>
    public class MIdSelector : IMSelector
    {
        public string Id { get; set; }

        public MIdSelector(string id = "")
        {
            Id = id;
        }

        public override string ToString()
        {
            return string.Format("#{0}", Id.Trim());
        }
    }
}
