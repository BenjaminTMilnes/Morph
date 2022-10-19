using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Morph;
using TauParsing;

namespace Morph.Tests
{
    public class ImporterTests
    {
        private MImporter Importer;

        public ImporterTests()
        {
            Importer = new MImporter();
        }

        [Theory]
        [InlineData("123.")]
        [InlineData("123123")]
        [InlineData("123.0")]
        [InlineData("123.000")]
        [InlineData("123.123")]
        [InlineData("1.123")]
        [InlineData("0.123")]
        [InlineData(".123")]
        [InlineData("000.123")]
        public void ImportNumberTest1(string n)
        {
            var number = Importer.GetNumber(n, new Marker());

            Assert.True(number is MNumber);
            Assert.Equal(n, number.Value);
        }

        [Theory]
        [InlineData("123mm")]
        [InlineData("123123cm")]
        [InlineData("123.dm")]
        [InlineData("123.0m")]
        [InlineData("123.000in")]
        [InlineData("123.123pt")]
        [InlineData("1.123pc")]
        [InlineData("0.123 mm")]
        [InlineData(".123   cm")]
        [InlineData("000.123      dm")]
        public void ImportLengthTest1(string n)
        {
            var length = Importer.GetLength(n, new Marker());

            Assert.True(length is MLength);
        }
    }
}
