using System;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
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
