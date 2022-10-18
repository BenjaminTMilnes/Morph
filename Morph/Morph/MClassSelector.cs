using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
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
