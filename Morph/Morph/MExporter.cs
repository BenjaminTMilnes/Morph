using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Morph
{
    public class MExporter
    {
        public static string ExportDocument(MDocument document)
        {
            return string.Join("", document.StyleRules.Select(sr => ExportStyleRule(sr)));
        }

        public static string ExportStyleRule(MStyleRule styleRule)
        {
            var ss = string.Join("", styleRule.Selectors);
            var pp = ExportProperties(styleRule.Properties);
            var t = string.Format("{0} {\n {1}}\n\n", ss, pp);

            return t;
        }

        public static string ExportProperties(IList<MProperty> properties, bool inline = false)
        {
            if (inline)
            {
                return string.Join(" ", properties);
            }
            else
            {
                return string.Join("", properties.Select(p => string.Format("\t{0}\n", p)));
            }
        }
    }
}
