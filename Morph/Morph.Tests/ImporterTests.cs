﻿using System;
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
        public void ImportLengthTest1(string l)
        {
            var length = Importer.GetLength(l, new Marker());

            Assert.True(length is MLength);
        }

        [Theory]
        [InlineData("2cm", 1)]
        [InlineData("2cm 2cm", 2)]
        [InlineData("2cm 2cm 2cm 2cm", 4)]
        public void ImportLengthSetTest1(string ls, int n)
        {
            var lengthSet = Importer.GetLengthSet(ls, new Marker());

            Assert.True(lengthSet is MLengthSet);
            Assert.Equal(n, lengthSet.Lengths.Count);
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
        [InlineData("#00000000", 0, 0, 0, 0)]
        [InlineData("#04050607", 4, 5, 6, 7)]
        [InlineData("#10101010", 16, 16, 16, 16)]
        [InlineData("#20304050", 32, 48, 64, 80)]
        [InlineData("#ffffffff", 255, 255, 255, 255)]
        [InlineData("#FFFFFFFF", 255, 255, 255, 255)]
        public void ImportRGBAColourTest1(string t, int r, int g, int b, int a)
        {
            var colour = Importer.GetRGBAColour(t, new Marker());

            Assert.Equal(r, colour.R);
            Assert.Equal(g, colour.G);
            Assert.Equal(b, colour.B);
            Assert.Equal(a, colour.A);
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

            Assert.Equal(r, colour.R);
            Assert.Equal(g, colour.G);
            Assert.Equal(b, colour.B);
            Assert.Equal(0, colour.A);
        }

        [Theory]
        [InlineData("rgba(0, 0, 0, 0%)", 0, 0, 0, 0)]
        [InlineData("rgba(100, 0, 0, 0%)", 100, 0, 0, 0)]
        [InlineData("rgba(100, 120, 200, 0%)", 100, 120, 200, 0)]
        [InlineData("rgba(100, 120, 200, 50%)", 100, 120, 200, 127)]
        [InlineData("rgba(225, 225, 225, 100%)", 225, 225, 225, 255)]
        public void ImportRGBAColourTest3(string t, int r, int g, int b, int a)
        {
            var colour = Importer.GetRGBAColour(t, new Marker());

            Assert.Equal(r, colour.R);
            Assert.Equal(g, colour.G);
            Assert.Equal(b, colour.B);
            Assert.Equal(a, colour.A);
        }

        [Theory]
        [InlineData("rgb(0, 0, 0)", 0, 0, 0)]
        [InlineData("rgb(100, 0, 0)", 100, 0, 0)]
        [InlineData("rgb(100, 120, 200)", 100, 120, 200)]
        [InlineData("rgb(225, 225, 225)", 225, 225, 225)]
        public void ImportRGBAColourTest4(string t, int r, int g, int b)
        {
            var colour = Importer.GetRGBAColour(t, new Marker());

            Assert.Equal(r, colour.R);
            Assert.Equal(g, colour.G);
            Assert.Equal(b, colour.B);
        }

        [Theory]
        [InlineData("hsla(0, 0%, 0%, 0%)", 0, 0, 0, 0)]
        [InlineData("hsla(60, 100%, 100%, 0%)", 60, 1, 1, 0)]
        [InlineData("hsla(220, 100%, 100%, 0%)", 220, 1, 1, 0)]
        [InlineData("hsla(359, 100%, 100%, 0%)", 359, 1, 1, 0)]
        [InlineData("hsla(120, 50%, 50%, 50%)", 120, 0.5, 0.5, 0.5)]
        public void ImportHSLAColourTest1(string t, double h, double s, double l, double a)
        {
            var colour = Importer.GetHSLAColour(t, new Marker());

            Assert.Equal(h, colour.H);
            Assert.Equal(s, colour.S);
            Assert.Equal(l, colour.L);
            Assert.Equal(a, colour.A);
        }

        [Theory]
        [InlineData("hsl(0, 0%, 0%)", 0, 0, 0)]
        [InlineData("hsl(60, 100%, 100%)", 60, 1, 1)]
        [InlineData("hsl(220, 100%, 100%)", 220, 1, 1)]
        [InlineData("hsl(359, 100%, 100%)", 359, 1, 1)]
        [InlineData("hsl(120, 50%, 50%)", 120, 0.5, 0.5)]
        public void ImportHSLAColourTest2(string t, double h, double s, double l)
        {
            var colour = Importer.GetHSLAColour(t, new Marker());

            Assert.Equal(h, colour.H);
            Assert.Equal(s, colour.S);
            Assert.Equal(l, colour.L);
        }

        [Theory]
        [InlineData("page-size: a4;", "page-size", "a4")]
        [InlineData("page-margin: 2cm 2cm 2cm 2cm;", "page-margin", "2cm 2cm 2cm 2cm")]
        [InlineData("font-name: 'Open Sans', sans-serif;", "font-name", "'Open Sans', sans-serif")]
        [InlineData("font-colour: hsl(350, 60%, 60%);", "font-colour", "hsl(350, 60%, 60%)")]
        [InlineData("font-colour: #dd0000;", "font-colour", "#dd0000")]
        [InlineData("font-colour: #dd000088;", "font-colour", "#dd000088")]
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
