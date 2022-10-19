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
    }
}
