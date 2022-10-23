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
        [InlineData("123")]
        [InlineData("123123123")]
        [InlineData("123.")]
        [InlineData("123.0")]
        [InlineData("123.000")]
        [InlineData("123.123")]
        [InlineData("1.123")]
        [InlineData("0.123")]
        [InlineData(".123")]
        [InlineData("000.123")]
        [InlineData("000.123000")]
        public void ImportNumberTest1(string t)
        {
            var number = Importer.GetNumber(t, new Marker());

            Assert.True(number is MNumber);
            Assert.Equal(t, number.Text);
        }

        [Theory]
        [InlineData("123abc", "123")]
        [InlineData("123123123abc", "123123123")]
        [InlineData("123.abc", "123.")]
        [InlineData("123.0abc", "123.0")]
        [InlineData("123.000abc", "123.000")]
        [InlineData("123.123abc", "123.123")]
        [InlineData("1.123abc", "1.123")]
        [InlineData("0.123abc", "0.123")]
        [InlineData(".123abc", ".123")]
        [InlineData("000.123abc", "000.123")]
        [InlineData("000.123000abc", "000.123000")]
        public void ImportNumberTest2(string t, string nt)
        {
            var number = Importer.GetNumber(t, new Marker());

            Assert.True(number is MNumber);
            Assert.Equal(nt, number.Text);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("abc")]
        [InlineData("abc123")]
        [InlineData(" 123")]
        [InlineData("   123")]
        public void ImportNumberTest3(string t)
        {
            var number = Importer.GetNumber(t, new Marker());

            Assert.True(number == null);
        }

        [Theory]
        [InlineData("123..123")]
        [InlineData("123...123")]
        [InlineData("..123")]
        [InlineData("...123")]
        [InlineData("123..")]
        [InlineData("123...")]
        [InlineData("12.0.1")]
        public void ImportNumberTest4(string t)
        {
            Assert.Throws<MorphSyntaxError>(() => Importer.GetNumber(t, new Marker()));
        }

        [Theory]
        [InlineData("123mm", "123", "mm")]
        [InlineData("123123cm", "123123", "cm")]
        [InlineData("123.dm", "123.", "dm")]
        [InlineData("123.0m", "123.0", "m")]
        [InlineData("123.000in", "123.000", "in")]
        [InlineData("123.123pt", "123.123", "pt")]
        [InlineData("0.123 mm", "0.123", "mm")]
        [InlineData(".123   cm", ".123", "cm")]
        [InlineData("000.123      dm", "000.123", "dm")]
        [InlineData("10mm", "10", "mm")]
        [InlineData("1cm", "1", "cm")]
        [InlineData("0.1dm", "0.1", "dm")]
        [InlineData("0.01m", "0.01", "m")]
        [InlineData("1in", "1", "in")]
        [InlineData("12pt", "12", "pt")]
        [InlineData("0.1pw", "0.1", "pw")]
        [InlineData("0.1ph", "0.1", "ph")]
        [InlineData("1.5em", "1.5", "em")]
        [InlineData("3en", "3", "en")]
        [InlineData("1.5rem", "1.5", "rem")]
        [InlineData("3ren", "3", "ren")]
        public void ImportLengthTest1(string t, string nt, string lut)
        {
            var length = Importer.GetLength(t, new Marker());

            Assert.True(length is MLength);
            Assert.Equal(nt, length.Number.Text);
            Assert.Equal(lut, length.Unit.Text);
        }

        [Theory]
        [InlineData("123mm123mm", "123", "mm")]
        [InlineData("123mmmm", "123", "mm")]
        [InlineData("123mmabc", "123", "mm")]
        [InlineData("123123cm123mm", "123123", "cm")]
        [InlineData("123.dm123mm", "123.", "dm")]
        [InlineData("123.0m123mm", "123.0", "m")]
        [InlineData("123.000in123mm", "123.000", "in")]
        [InlineData("123.123pt123mm", "123.123", "pt")]
        [InlineData("0.123 mm123mm", "0.123", "mm")]
        [InlineData(".123   cm123mm", ".123", "cm")]
        [InlineData("000.123      dm123mm", "000.123", "dm")]
        public void ImportLengthTest2(string t, string nt, string lut)
        {
            var length = Importer.GetLength(t, new Marker());

            Assert.True(length is MLength);
            Assert.Equal(nt, length.Number.Text);
            Assert.Equal(lut, length.Unit.Text);
        }

        [Theory]
        [InlineData("123km")]
        [InlineData("123nm")]
        [InlineData("123pc")]
        [InlineData("123yd")]
        [InlineData("123abc")]
        [InlineData("mm")]
        [InlineData("km")]
        [InlineData("abc")]
        public void ImportLengthTest3(string t)
        {
            var length = Importer.GetLength(t, new Marker());

            Assert.True(length == null);
        }

        [Theory]
        [InlineData("123..dm")]
        [InlineData("123..0m")]
        [InlineData("123..000in")]
        [InlineData("123..123pt")]
        [InlineData("1..123pc")]
        [InlineData("0..123 mm")]
        [InlineData("..123   cm")]
        [InlineData("000..123      dm")]
        public void ImportLengthTest4(string t)
        {
            Assert.Throws<MorphSyntaxError>(() => Importer.GetLength(t, new Marker()));
        }

        [Theory]
        [InlineData("2cm", 1)]
        [InlineData("2cm 2cm", 2)]
        [InlineData("2cm 2cm 2cm 2cm", 4)]
        [InlineData("2cm 2cm 2", 2)]
        [InlineData("2cm 2cm cm", 2)]
        [InlineData("2cm 2cm abc", 2)]
        [InlineData("2cm 2cm, 2cm", 2)]
        [InlineData("2cm 2cm, 2cm 2cm", 2)]
        [InlineData("2cm, 2cm, 2cm, 2cm", 1)]
        public void ImportLengthSetTest1(string ls, int n)
        {
            var lengthSet = Importer.GetLengthSet(ls, new Marker());

            Assert.True(lengthSet is MLengthSet);
            Assert.Equal(n, lengthSet.Lengths.Count);
        }

        [Theory]
        [InlineData("123%", "123")]
        [InlineData("123123123%", "123123123")]
        [InlineData("123.%", "123.")]
        [InlineData("123.0%", "123.0")]
        [InlineData("123.000%", "123.000")]
        [InlineData("123.123%", "123.123")]
        [InlineData("1.123%", "1.123")]
        [InlineData("0.123%", "0.123")]
        [InlineData(".123%", ".123")]
        [InlineData("000.123%", "000.123")]
        [InlineData("000.123000%", "000.123000")]
        [InlineData("12.3%", "12.3")]
        public void ImportPercentageTest1(string t, string nt)
        {
            var percentage = Importer.GetPercentage(t, new Marker());

            Assert.True(percentage is MPercentage);
            Assert.Equal(double.Parse(nt) / 100.0, percentage.Value);
        }

        [Theory]
        [InlineData("123 %")]
        [InlineData("123123123 %")]
        [InlineData("123. %")]
        [InlineData("123.0 %")]
        [InlineData("123.000 %")]
        [InlineData("123.123 %")]
        [InlineData("1.123 %")]
        [InlineData("0.123 %")]
        [InlineData(".123 %")]
        [InlineData("000.123 %")]
        [InlineData("000.123000 %")]
        [InlineData("12.3 %")]
        [InlineData("%")]
        [InlineData("a%")]
        public void ImportPercentageTest2(string t)
        {
            var percentage = Importer.GetPercentage(t, new Marker());

            Assert.True(percentage == null);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("123abc")]
        [InlineData("abcdef")]
        [InlineData("ffffff")]
        [InlineData("f0f0f0")]
        [InlineData("d4d4d4")]
        [InlineData("123ABC")]
        [InlineData("ABCDEF")]
        [InlineData("FFFFFF")]
        [InlineData("F0F0F0")]
        [InlineData("D4D4D4")]
        public void ImportHexadecimalNumberTest1(string n)
        {
            var number = Importer.GetHexadecimalNumber("#" + n, new Marker());

            Assert.Equal(n, number);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("123abc")]
        [InlineData("abcdef")]
        [InlineData("ffffff")]
        [InlineData("f0f0f0")]
        [InlineData("d4d4d4")]
        [InlineData("123ABC")]
        [InlineData("ABCDEF")]
        [InlineData("FFFFFF")]
        [InlineData("F0F0F0")]
        [InlineData("D4D4D4")]
        public void ImportHexadecimalNumberTest2(string n)
        {
            var number = Importer.GetHexadecimalNumber(n, new Marker());

            Assert.True(number == null);
        }

        [Theory]
        [InlineData("ghi")]
        [InlineData("GHI")]
        [InlineData("gggggg")]
        [InlineData("g0g0g0")]
        [InlineData("GGGGGG")]
        [InlineData("G0G0G0")]
        [InlineData(".123")]
        [InlineData(" 123123")]
        [InlineData(" FFFFFF")]
        [InlineData("-FFFFFF")]
        public void ImportHexadecimalNumberTest3(string n)
        {
            var number = Importer.GetHexadecimalNumber("#" + n, new Marker());

            Assert.True(number == null);
        }

        [Theory]
        [InlineData("#00000000", 0, 0, 0, 0)]
        [InlineData("#04050607", 4, 5, 6, 7)]
        [InlineData("#10101010", 16, 16, 16, 16)]
        [InlineData("#20304050", 32, 48, 64, 80)]
        [InlineData("#ffffffff", 255, 255, 255, 255)]
        [InlineData("#FFFFFFFF", 255, 255, 255, 255)]
        public void ImportRGBAColourTest1(string t, int r, int g, int b, int a)
        {
            var colour = Importer.GetRGBAColour(t, new Marker());

            Assert.Equal(r, colour.R.Value);
            Assert.Equal(g, colour.G.Value);
            Assert.Equal(b, colour.B.Value);
            Assert.Equal(a, colour.A.Value);
        }

        [Theory]
        [InlineData("#000000", 0, 0, 0)]
        [InlineData("#040506", 4, 5, 6)]
        [InlineData("#101010", 16, 16, 16)]
        [InlineData("#203040", 32, 48, 64)]
        [InlineData("#ffffff", 255, 255, 255)]
        [InlineData("#FFFFFF", 255, 255, 255)]
        public void ImportRGBAColourTest2(string t, int r, int g, int b)
        {
            var colour = Importer.GetRGBAColour(t, new Marker());

            Assert.Equal(r, colour.R.Value);
            Assert.Equal(g, colour.G.Value);
            Assert.Equal(b, colour.B.Value);
            Assert.Equal(0, colour.A.Value);
        }

        [Theory]
        [InlineData("#00")]
        [InlineData("#000")]
        [InlineData("#0000")]
        [InlineData("#00000")]
        [InlineData("#0000000")]
        [InlineData("#000000000")]
        public void ImportRGBAColourTest3(string t)
        {
            Assert.Throws<MorphSyntaxError>(() => Importer.GetRGBAColour(t, new Marker()));
        }

        [Theory]
        [InlineData("rgba(0, 0, 0, 0%)", 0, 0, 0, 0)]
        [InlineData("rgba(100, 0, 0, 0%)", 100, 0, 0, 0)]
        [InlineData("rgba(100, 120, 200, 0%)", 100, 120, 200, 0)]
        [InlineData("rgba(100, 120, 200, 50%)", 100, 120, 200, 0.5)]
        [InlineData("rgba(225, 225, 225, 100%)", 225, 225, 225, 1)]
        public void ImportRGBAColourTest4(string t, double r, double g, double b, double a)
        {
            var colour = Importer.GetRGBAColour(t, new Marker());

            Assert.Equal(r, colour.R.Value);
            Assert.Equal(g, colour.G.Value);
            Assert.Equal(b, colour.B.Value);
            Assert.Equal(a, colour.A.Value);
        }

        [Theory]
        [InlineData("rgba(512, 0, 0, 0%)")]
        [InlineData("rgba(0, 1024, 0, 0%)")]
        [InlineData("rgba(512, 512, 512, 0%)")]
        [InlineData("rgba(0, 0, 0, 150%)")]
        [InlineData("rgba(225, 225, 225, 1000%)")]
        [InlineData("rgba(0, 0, 0, 0, 0)")]
        [InlineData("rgba(0, 0, 0)")]
        [InlineData("rgba(0, 0)")]
        [InlineData("rgba(0)")]
        public void ImportRGBAColourTest5(string t)
        {
            Assert.Throws<MorphSyntaxError>(() => Importer.GetRGBAColour(t, new Marker()));
        }

        [Theory]
        [InlineData("rgb(0, 0, 0)", 0, 0, 0)]
        [InlineData("rgb(100, 0, 0)", 100, 0, 0)]
        [InlineData("rgb(100, 120, 200)", 100, 120, 200)]
        [InlineData("rgb(225, 225, 225)", 225, 225, 225)]
        public void ImportRGBAColourTest6(string t, double r, double g, double b)
        {
            var colour = Importer.GetRGBAColour(t, new Marker());

            Assert.Equal(r, colour.R.Value);
            Assert.Equal(g, colour.G.Value);
            Assert.Equal(b, colour.B.Value);
        }

        [Theory]
        [InlineData("rgb(512, 0, 0)")]
        [InlineData("rgb(0, 1024, 0)")]
        [InlineData("rgb(512, 512, 512)")]
        [InlineData("rgb(0, 0, 0, 0, 0)")]
        [InlineData("rgb(0, 0, 0, 0)")]
        [InlineData("rgb(0, 0)")]
        [InlineData("rgb(0)")]
        public void ImportRGBAColourTest7(string t)
        {
            Assert.Throws<MorphSyntaxError>(() => Importer.GetRGBAColour(t, new Marker()));
        }

        [Theory]
        [InlineData("hsla(0, 0%, 0%, 0%)", 0, 0, 0, 0)]
        [InlineData("hsla(60, 100%, 100%, 0%)", 60, 1, 1, 0)]
        [InlineData("hsla(220, 100%, 100%, 0%)", 220, 1, 1, 0)]
        [InlineData("hsla(359, 100%, 100%, 0%)", 359, 1, 1, 0)]
        [InlineData("hsla(120, 50%, 50%, 50%)", 120, 0.5, 0.5, 0.5)]
        [InlineData("hsla(0, 0, 0, 0)", 0, 0, 0, 0)]
        [InlineData("hsla(60, 1, 1, 0)", 60, 1, 1, 0)]
        [InlineData("hsla(220, 1, 1, 0)", 220, 1, 1, 0)]
        [InlineData("hsla(359, 1, 1, 0)", 359, 1, 1, 0)]
        [InlineData("hsla(120, 0.5, 0.5, 0.5)", 120, 0.5, 0.5, 0.5)]
        public void ImportHSLAColourTest1(string t, double h, double s, double l, double a)
        {
            var colour = Importer.GetHSLAColour(t, new Marker());

            Assert.Equal(h, colour.H.Value);
            Assert.Equal(s, colour.S.Value);
            Assert.Equal(l, colour.L.Value);
            Assert.Equal(a, colour.A.Value);
        }

        [Theory]
        [InlineData("hsla(0, 200%, 0%, 0%)")]
        [InlineData("hsla(0, 0%, 200%, 0%)")]
        [InlineData("hsla(0, 0%, 0%, 200%)")]
        [InlineData("hsla(0, 200%, 200%, 200%)")]
        [InlineData("hsla(0, 2, 0, 0)")]
        [InlineData("hsla(0, 0, 2, 0)")]
        [InlineData("hsla(0, 0, 0, 2)")]
        [InlineData("hsla(0, 2, 2, 2)")]
        [InlineData("hsla(0, 0%, 0%, 0%, 0%)")]
        [InlineData("hsla(0, 0%, 0%)")]
        [InlineData("hsla(0, 0%)")]
        [InlineData("hsla(0)")]
        public void ImportHSLAColourTest2(string t)
        {
            Assert.Throws<MorphSyntaxError>(() => Importer.GetHSLAColour(t, new Marker()));
        }

        [Theory]
        [InlineData("hsl(0, 0%, 0%)", 0, 0, 0)]
        [InlineData("hsl(60, 100%, 100%)", 60, 1, 1)]
        [InlineData("hsl(220, 100%, 100%)", 220, 1, 1)]
        [InlineData("hsl(359, 100%, 100%)", 359, 1, 1)]
        [InlineData("hsl(120, 50%, 50%)", 120, 0.5, 0.5)]
        public void ImportHSLAColourTest3(string t, double h, double s, double l)
        {
            var colour = Importer.GetHSLAColour(t, new Marker());

            Assert.Equal(h, colour.H.Value);
            Assert.Equal(s, colour.S.Value);
            Assert.Equal(l, colour.L.Value);
        }

        [Theory]
        [InlineData("page-size: a4;", "page-size", "a4")]
        [InlineData("page-margin: 2cm 2cm 2cm 2cm;", "page-margin", "2cm 2cm 2cm 2cm")]
        [InlineData("font-name: 'Open Sans', sans-serif;", "font-name", "'Open Sans', sans-serif")]
        [InlineData("font-colour: hsl(350, 60%, 60%);", "font-colour", "hsl(350, 60%, 60%)")]
        [InlineData("font-colour: #dd0000;", "font-colour", "#DD0000")]
        [InlineData("font-colour: #dd000088;", "font-colour", "#DD000088")]
        [InlineData("font-colour: black;", "font-colour", "black")]
        public void ImportPropertyTest1(string text, string name, string value)
        {
            var property = Importer.GetProperty(text, new Marker());

            Assert.Equal(name, property.Name);
            Assert.Equal(value, property.Value.ToString());
        }

        [Fact]
        public void ImportDocumentTest1()
        {
            var text = @"
                            p {
                                font-size: 12pt;
                                font-colour   :   black   ;
                                margin   :12pt 16pt   ;
                            }

                            .red { font-colour: red; }

                            #infobox {
                                border: 1px solid blue;
                            }
                        ";

            var document = Importer.ImportDocument(text);

            Assert.Equal(3, document.StyleRules.Count);

            var sr1 = document.StyleRules[0];
            var sr2 = document.StyleRules[1];
            var sr3 = document.StyleRules[2];

            Assert.Equal(1, sr1.Selectors.Count);
            Assert.Equal(3, sr1.Properties.Count);

            var p1 = sr1.Properties[0];
            var p2 = sr1.Properties[1];
            var p3 = sr1.Properties[2];

            Assert.Equal("font-size", p1.Name);

            var ls1 = new MLengthSet();

            ls1.Lengths.Add(new MLength("12", "pt"));

            Assert.True(p1.Value is MLengthSet);
            Assert.Equal(1, (p1.Value as MLengthSet).Lengths.Count);
            Assert.Equal(ls1.ToString(), p1.Value.ToString());

            Assert.Equal("font-colour", p2.Name);
            Assert.Equal("black", p2.Value.ToString());

            Assert.Equal("margin", p3.Name);

            var ls2 = new MLengthSet();

            ls2.Lengths.Add(new MLength("12", "pt"));
            ls2.Lengths.Add(new MLength("16", "pt"));

            Assert.True(p3.Value is MLengthSet);
            Assert.Equal(2, (p3.Value as MLengthSet).Lengths.Count);
            Assert.Equal(ls2.ToString(), p3.Value.ToString());

            Assert.Equal(1, sr2.Selectors.Count);
            Assert.Equal(1, sr2.Properties.Count);

            var p4 = sr2.Properties[0];

            Assert.Equal("font-colour", p4.Name);
            Assert.Equal("red", p4.Value.ToString());

            Assert.Equal(1, sr3.Selectors.Count);
            Assert.Equal(1, sr3.Properties.Count);

            var s3 = sr3.Selectors[0] as MIdSelector;

            Assert.Equal("infobox", s3.Id);
        }
    }
}
