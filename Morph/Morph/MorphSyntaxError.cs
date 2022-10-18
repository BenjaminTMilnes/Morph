using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MorphSyntaxError : Exception
    {
        public MorphSyntaxError(string message) : base(message) { }
    }
}
