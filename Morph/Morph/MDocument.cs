using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Morph
{
    /// <summary>
    /// Represents a Morph document. A Morph document contains a list of style rules.
    /// </summary>
    public class MDocument
    {
        /// <summary>
        /// The style rules within the document.
        /// </summary>
        public IList<MStyleRule> StyleRules { get; set; }

        public MDocument()
        {
            StyleRules = new List<MStyleRule>();
        }

        /// <summary>
        /// Imports a Morph document from a file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static MDocument Import(string filePath)
        {
            var importer = new MImporter();

            var t = File.ReadAllText(filePath);

            return importer.ImportDocument(t);
        }

        /// <summary>
        /// Exports the Morph document as a string.
        /// </summary>
        /// <returns></returns>
        public string Export()
        {
            return MExporter.ExportDocument(this);
        }

        /// <summary>
        /// Exports the Morph document to the given file path.
        /// </summary>
        /// <param name="filePath"></param>
        public void Export(string filePath)
        {
            var t = MExporter.ExportDocument(this);

            File.WriteAllText(filePath, t, Encoding.UTF8);
        }
    }
}
