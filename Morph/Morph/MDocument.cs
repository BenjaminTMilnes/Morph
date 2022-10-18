using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    public class MDocument
    {
        public IList<MStyleRule> StyleRules { get; set; }

        public MDocument()
        {
            StyleRules = new List<MStyleRule>();
        }

        public string Export()
        {
            return MExporter.ExportDocument(this);
        }

        public void Export(string filePath)
        {
            var t = MExporter.ExportDocument(this);

            File.WriteAllText(filePath, t, Encoding.UTF8);
        }
    }
}
