﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TauParsing;

namespace Morph
{
    public class MImporter : Parser
    {
        public static string[] LengthUnits
        {
            get
            {
                return new string[] { "mm", "cm", "dm", "m", "pt", "in", "pc" };
            }
        }

        public bool IsAlphanumeric(char character)
        {
            var n = (int)character;

            return (n >= 48 && n < 58) || (n >= 65 && n < 91) || (n >= 97 && n < 123);
        }

        public bool IsDecimal(char character)
        {
            var n = (int)character;

            return (n >= 48 && n < 58);
        }

        public bool IsHexadecimal(char character)
        {
            var n = (int)character;

            return (n >= 48 && n < 58) || (n >= 65 && n < 71) || (n >= 97 && n < 103);
        }

        public MDocument ImportDocument(string inputText)
        {
            var marker = new Marker();

            var styleRules = new List<MStyleRule>();

            while (marker.P < inputText.Length)
            {
                var styleRule = GetStyleRule(inputText, marker);

                if (styleRule != null)
                {
                    styleRules.Add(styleRule);
                }
                else
                {
                    break;
                }
            }

            var d = new MDocument();

            d.StyleRules = styleRules;

            return d;
        }

        public MStyleRule GetStyleRule(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var selectors = GetSelectors(inputText, m);
            var properties = GetProperties(inputText, m);

            if (selectors == null || properties == null)
            {
                return null;
            }

            var styleRule = new MStyleRule();

            styleRule.Selectors = selectors;
            styleRule.Properties = properties;

            marker.P = m.P;

            return styleRule;
        }

        public List<IMSelector> GetSelectors(string inputText, Marker marker)
        {
            var m = marker.Copy();

            GetWhiteSpace(inputText, m);

            var selectors = new List<IMSelector>();

            while (m.P < inputText.Length)
            {
                var idSelector = GetIdSelector(inputText, m);

                if (idSelector != null)
                {
                    selectors.Add(idSelector);
                    continue;
                }

                var classSelector = GetClassSelector(inputText, m);

                if (classSelector != null)
                {
                    selectors.Add(classSelector);
                    continue;
                }

                var elementNameSelector = GetElementNameSelector(inputText, m);

                if (elementNameSelector != null)
                {
                    selectors.Add(elementNameSelector);
                    continue;
                }

                break;
            }

            marker.P = m.P;

            return selectors;
        }

        public MIdSelector GetIdSelector(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var c = inputText.Substring(m.P, 1)[0];

            if (c != '#')
            {
                return null;
            }

            m.P++;

            var start = m.P;

            while (m.P < inputText.Length)
            {
                c = inputText.Substring(m.P, 1)[0];

                if (IsAlphanumeric(c) || c == '-' || c == '_')
                {
                    m.P++;
                }
                else
                {
                    break;
                }
            }

            var end = m.P;

            if (end == start)
            {
                return null;
            }

            var t = inputText.Substring(start, end - start);

            marker.P = m.P;

            return new MIdSelector(t);
        }

        public MClassSelector GetClassSelector(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var c = inputText.Substring(m.P, 1)[0];

            if (c != '.')
            {
                return null;
            }

            m.P++;

            var start = m.P;

            while (m.P < inputText.Length)
            {
                c = inputText.Substring(m.P, 1)[0];

                if (IsAlphanumeric(c) || c == '-' || c == '_')
                {
                    m.P++;
                }
                else
                {
                    break;
                }
            }

            var end = m.P;

            if (end == start)
            {
                return null;
            }

            var t = inputText.Substring(start, end - start);

            marker.P = m.P;

            return new MClassSelector(t);
        }

        public MElementNameSelector GetElementNameSelector(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var start = m.P;

            while (m.P < inputText.Length)
            {
                var c = inputText.Substring(m.P, 1)[0];

                if (IsAlphanumeric(c) || c == '-' || c == '_')
                {
                    m.P++;
                }
                else
                {
                    break;
                }
            }

            var end = m.P;

            if (end == start)
            {
                return null;
            }

            var t = inputText.Substring(start, end - start);

            marker.P = m.P;

            return new MElementNameSelector(t);
        }

        public List<MProperty> GetProperties(string inputText, Marker marker)
        {
            var m = marker.Copy();

            GetWhiteSpace(inputText, m);

            if (m.P >= inputText.Length)
            {
                return null;
            }

            var c = inputText.Substring(m.P, 1)[0];

            if (c != '{')
            {
                return null;
            }

            m.P++;

            var properties = new List<MProperty>();

            while (true)
            {
                var p = GetProperty(inputText, m);

                if (p != null)
                {
                    properties.Add(p);
                }
                else
                {
                    break;
                }
            }

            GetWhiteSpace(inputText, m);

            c = inputText.Substring(m.P, 1)[0];

            if (c != '}')
            {
                return null;
            }

            m.P++;

            marker.P = m.P;

            return properties;
        }

        public MProperty GetProperty(string inputText, Marker marker)
        {
            var m = marker.Copy();

            GetWhiteSpace(inputText, m);

            var name = GetPropertyName(inputText, m);

            if (name == null)
            {
                return null;
            }

            GetWhiteSpace(inputText, m);

            var c = inputText.Substring(m.P, 1)[0];

            if (c != ':')
            {
                return null;
            }

            m.P++;

            var value = GetPropertyValue(inputText, m);

            if (value == null)
            {
                return null;
            }

            c = inputText.Substring(m.P, 1)[0];

            if (c != ';')
            {
                return null;
            }

            m.P++;

            marker.P = m.P;

            return new MProperty(name, value);
        }

        public string GetPropertyName(string inputText, Marker marker)
        {
            var start = marker.P;

            while (marker.P < inputText.Length)
            {
                var c = inputText.Substring(marker.P, 1)[0];

                if (IsAlphanumeric(c) || c == '-')
                {
                    marker.P++;
                }
                else
                {
                    break;
                }
            }

            var end = marker.P;

            if (end == start)
            {
                return null;
            }

            var t = inputText.Substring(start, end - start);

            return t;
        }

        public object GetPropertyValue(string inputText, Marker marker)
        {
            var lengthSet = GetLengthSet(inputText, marker);

            if (lengthSet != null)
            {
                return lengthSet;
            }

            var rgbaColour = GetRGBAColour(inputText, marker);

            if (rgbaColour != null)
            {
                return rgbaColour;
            }

            var hslaColour = GetHSLAColour(inputText, marker);

            if (hslaColour != null)
            {
                return hslaColour;
            }

            var start = marker.P;

            while (marker.P < inputText.Length)
            {
                var c = inputText.Substring(marker.P, 1)[0];

                if (";}{".Any(d => d == c))
                {
                    break;
                }
                else
                {
                    marker.P++;
                }
            }

            var end = marker.P;

            if (end == start)
            {
                return null;
            }

            var t = inputText.Substring(start, end - start);
            t = t.Trim();

            return t;

        }

        public MHSLAColour GetHSLAColour(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var c = inputText.Substring(m.P, 4);

            if (c == "hsla")
            {
                m.P += 4;

                var ns = GetNumberSet(inputText, m);

                if (ns == null)
                {
                    return null;
                }

                if (ns.Count != 4)
                {
                    throw new MorphSyntaxError("A HSLA colour must have four values.");
                }

                var h = ns[0];
                var s = ns[1];
                var l = ns[2];
                var a = ns[3];

                ValidateRGBAColourValue(h);
                ValidateRGBAColourValue(s);
                ValidateRGBAColourValue(l);
                ValidateRGBAColourValue(a);

                var hi = (h is MPercentage) ? (int)((h as MPercentage).Value * 360) : int.Parse((h as MNumber).Value);
                var si = (s is MPercentage) ? (int)((s as MPercentage).Value * 255) : int.Parse((s as MNumber).Value);
                var li = (l is MPercentage) ? (int)((l as MPercentage).Value * 255) : int.Parse((l as MNumber).Value);
                var ai = (a is MPercentage) ? (int)((a as MPercentage).Value * 255) : int.Parse((a as MNumber).Value);

                return new MHSLAColour(hi, si, li, ai);
            }

            c = inputText.Substring(m.P, 3);

            if (c == "hsl")
            {
                m.P += 3;

                var ns = GetNumberSet(inputText, m);

                if (ns == null)
                {
                    return null;
                }

                if (ns.Count != 3)
                {
                    throw new MorphSyntaxError("An RGB colour must have four values.");
                }

                var h = ns[0];
                var s = ns[1];
                var l = ns[2];

                ValidateRGBAColourValue(h);
                ValidateRGBAColourValue(s);
                ValidateRGBAColourValue(l);

                var hi = (h is MPercentage) ? (int)((h as MPercentage).Value * 360) : int.Parse((h as MNumber).Value);
                var si = (s is MPercentage) ? (int)((s as MPercentage).Value * 255) : int.Parse((s as MNumber).Value);
                var li = (l is MPercentage) ? (int)((l as MPercentage).Value * 255) : int.Parse((l as MNumber).Value);

                return new MHSLColour(hi, si, li);
            }

            return null;
        }

        public MRGBAColour GetRGBAColour(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var hn = GetHexadecimalNumber(inputText, m);

            if (hn != null)
            {
                if (hn.Length != 6 && hn.Length != 8)
                {
                    throw new MorphSyntaxError(string.Format("'#{0}' is not a valid hexadecimal colour code.", hn));
                }

                var r = Convert.ToInt32(hn.Substring(0, 2), 16);
                var g = Convert.ToInt32(hn.Substring(2, 2), 16);
                var b = Convert.ToInt32(hn.Substring(4, 2), 16);

                if (hn.Length == 8)
                {
                    var a = Convert.ToInt32(hn.Substring(6, 2), 16);

                    return new MRGBAColour(r, g, b, a);
                }
                else
                {
                    return new MRGBColour(r, g, b);
                }
            }

            var c = inputText.Substring(m.P, 4);

            if (c == "rgba")
            {
                m.P += 4;

                var ns = GetNumberSet(inputText, m);

                if (ns == null)
                {
                    return null;
                }

                if (ns.Count != 4)
                {
                    throw new MorphSyntaxError("An RGBA colour must have four values.");
                }

                var r = ns[0];
                var g = ns[1];
                var b = ns[2];
                var a = ns[3];

                ValidateRGBAColourValue(r);
                ValidateRGBAColourValue(g);
                ValidateRGBAColourValue(b);
                ValidateRGBAColourValue(a);

                var ri = (r is MPercentage) ? (int)((r as MPercentage).Value * 255) : int.Parse((r as MNumber).Value);
                var gi = (g is MPercentage) ? (int)((g as MPercentage).Value * 255) : int.Parse((g as MNumber).Value);
                var bi = (b is MPercentage) ? (int)((b as MPercentage).Value * 255) : int.Parse((b as MNumber).Value);
                var ai = (a is MPercentage) ? (int)((a as MPercentage).Value * 255) : int.Parse((a as MNumber).Value);

                return new MRGBAColour(ri, gi, bi, ai);
            }

            c = inputText.Substring(m.P, 3);

            if (c == "rgb")
            {
                m.P += 3;

                var ns = GetNumberSet(inputText, m);

                if (ns == null)
                {
                    return null;
                }

                if (ns.Count != 3)
                {
                    throw new MorphSyntaxError("An RGB colour must have four values.");
                }

                var r = ns[0];
                var g = ns[1];
                var b = ns[2];

                ValidateRGBAColourValue(r);
                ValidateRGBAColourValue(g);
                ValidateRGBAColourValue(b);

                var ri = (r is MPercentage) ? (int)((r as MPercentage).Value * 255) : int.Parse((r as MNumber).Value);
                var gi = (g is MPercentage) ? (int)((g as MPercentage).Value * 255) : int.Parse((g as MNumber).Value);
                var bi = (b is MPercentage) ? (int)((b as MPercentage).Value * 255) : int.Parse((b as MNumber).Value);

                return new MRGBColour(ri, gi, bi);
            }

            return null;
        }

        public void ValidateRGBAColourValue(object value)
        {
            if (value is MNumber)
            {
                var v = int.Parse((value as MNumber).Value);

                if (v < 0 || v > 255)
                {
                    throw new MorphSyntaxError("RGBA colour values must be between 0 and 255.");
                }
            }
            if (value is MPercentage)
            {
                var v = (value as MPercentage).Value * 100;

                if (v < 0 || v > 100)
                {
                    throw new MorphSyntaxError("RGBA colour values must be between 0% and 100%.");
                }
            }
        }

        public string GetHexadecimalNumber(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var c = inputText.Substring(m.P, 1)[0];

            if (c != '#')
            {
                return null;
            }

            m.P++;

            var start = m.P;

            while (m.P < inputText.Length)
            {
                c = inputText.Substring(m.P, 1)[0];

                if (IsHexadecimal(c))
                {
                    m.P++;
                }
                else
                {
                    break;
                }
            }

            var end = m.P;

            if (end - start < 2)
            {
                return null;
            }

            var t = inputText.Substring(start, end - start);

            marker.P = m.P;

            return t;
        }

        public List<object> GetNumberSet(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var c = inputText.Substring(m.P, 1);

            if (c != "(")
            {
                return null;
            }

            m.P++;

            var numbers = new List<object>();

            while (true)
            {
                GetWhiteSpace(inputText, m);

                var p = GetPercentage(inputText, m);

                if (p != null)
                {
                    numbers.Add(p);
                }
                else
                {
                    var n = GetNumber(inputText, m);

                    if (n != null)
                    {
                        numbers.Add(n);
                    }
                }

                GetWhiteSpace(inputText, m);

                c = inputText.Substring(m.P, 1);

                if (c == ",")
                {
                    m.P++;
                    continue;
                }
                else if (c == ")")
                {
                    m.P++;
                    break;
                }
                else
                {
                    throw new MorphSyntaxError("Expected a comma or a closing bracket.");
                }
            }

            return numbers;
        }

        public MPercentage GetPercentage(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var number = GetNumber(inputText, m);

            if (number == null)
            {
                return null;
            }

            var c = inputText.Substring(m.P, 1);

            if (c != "%")
            {
                return null;
            }

            m.P++;
            marker.P = m.P;

            return new MPercentage(double.Parse(number.Value) / 100);
        }

        public MLengthSet GetLengthSet(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var lengths = new List<MLength>();

            while (true)
            {
                GetWhiteSpace(inputText, m);

                var length = GetLength(inputText, m);

                if (length != null)
                {
                    lengths.Add(length);
                }
                else
                {
                    break;
                }
            }

            if (!lengths.Any())
            {
                return null;
            }

            marker.P = m.P;

            var lengthSet = new MLengthSet();

            lengthSet.Lengths = lengths;

            return lengthSet;
        }

        public MLength GetLength(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var number = GetNumber(inputText, m);

            if (number == null)
            {
                return null;
            }

            GetWhiteSpace(inputText, m);

            var unit = GetLengthUnit(inputText, m);

            if (unit == null)
            {
                return null;
            }

            marker.P = m.P;

            var length = new MLength();

            length.Number = number;
            length.Unit = unit;

            return length;
        }

        public MNumber GetNumber(string inputText, Marker marker)
        {
            var start = marker.P;
            var q = 0;

            while (marker.P < inputText.Length)
            {
                var c = inputText.Substring(marker.P, 1)[0];

                if (IsDecimal(c))
                {
                    marker.P++;
                }
                else if (c == '.')
                {
                    q++;
                    marker.P++;
                }
                else
                {
                    break;
                }
            }

            var end = marker.P;

            if (end == start)
            {
                return null;
            }

            var t = inputText.Substring(start, end - start);

            if (t == "." || q > 1)
            {
                throw new MorphSyntaxError(string.Format("'{0}' is not a valid number.", t));
            }

            return new MNumber(t);
        }

        public MLengthUnit GetLengthUnit(string inputText, Marker marker)
        {
            foreach (var lengthUnit in LengthUnits)
            {
                var l = lengthUnit.Length;

                if (marker.P <= inputText.Length - l)
                {
                    var t = inputText.Substring(marker.P, l);

                    if (t == lengthUnit)
                    {
                        marker.P += l;

                        return new MLengthUnit(lengthUnit);
                    }
                }
            }

            return null;
        }
    }
}
