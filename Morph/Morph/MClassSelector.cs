using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph class selector. These work in the same way as CSS class selectors.
    /// </summary>
    public class MClassSelector : IMSelector
    {
        public string ClassName { get; set; }

        public MClassSelector(string className = "")
        {
            ClassName = className;
        }

        public override string ToString()
        {
            return string.Format(".{0}", ClassName.Trim());
        }
    }
}
