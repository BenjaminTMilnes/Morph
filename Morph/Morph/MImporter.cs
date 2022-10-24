using System;
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
                return new string[] { "mm", "cm", "dm", "m", "pt", "in", "pw", "ph", "em", "en", "rem", "ren" };
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

            if (Expect(inputText, m, "#") == false)
            {
                return null;
            }

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

            return new MIdSelector(t);
        }

        public MClassSelector GetClassSelector(string inputText, Marker marker)
        {
            var m = marker.Copy();

            if (Expect(inputText, m, ".") == false)
            {
                return null;
            }

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

        /// <summary>
        /// Gets a list of properties at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public List<MProperty> GetProperties(string inputText, Marker marker)
        {
            var m = marker.Copy();

            GetWhiteSpace(inputText, m);

            if (Expect(inputText, m, "{") == false)
            {
                return null;
            }

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

            if (Expect(inputText, m, "}") == false)
            {
                return null;
            }

            marker.P = m.P;

            return properties;
        }

        /// <summary>
        /// Gets a property at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
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

            if (Expect(inputText, m, ":") == false)
            {
                return null;
            }

            GetWhiteSpace(inputText, m);

            var value = GetPropertyValue(inputText, m);

            if (value == null)
            {
                return null;
            }

            GetWhiteSpace(inputText, m);

            if (Expect(inputText, m, ";") == false)
            {
                return null;
            }

            marker.P = m.P;

            return new MProperty(name, value);
        }

        /// <summary>
        /// Gets a property name at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets a property value at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
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

            var cmykColour = GetCMYKColour(inputText, marker);

            if (cmykColour != null)
            {
                return cmykColour;
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

        /// <summary>
        /// Gets a CMYK colour at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public MCMYKColour GetCMYKColour(string inputText, Marker marker)
        {
            var m = marker.Copy();

            if (m.P > inputText.Length - 4)
            {
                return null;
            }

            var c = inputText.Substring(m.P, 4);

            if (c == "cmyk")
            {
                m.P += 4;

                var ns = GetNumberSet(inputText, m);

                if (ns == null)
                {
                    return null;
                }

                if (ns.Count != 4)
                {
                    throw new MorphSyntaxError("A CMYK colour must have four values.");
                }

                var _c = ns[0];
                var _m = ns[1];
                var _y = ns[2];
                var _k = ns[3];

                ValidateCMYKColourValue(_c);
                ValidateCMYKColourValue(_m);
                ValidateCMYKColourValue(_y);
                ValidateCMYKColourValue(_k);

                marker.P = m.P;

                return new MCMYKColour(_c, _m, _y, _k);
            }

            return null;
        }

        /// <summary>
        /// Gets a HSLA colour at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public MHSLAColour GetHSLAColour(string inputText, Marker marker)
        {
            var m = marker.Copy();

            if (m.P > inputText.Length - 4)
            {
                return null;
            }

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

                ValidateHSLAColourValue(s);
                ValidateHSLAColourValue(l);
                ValidateHSLAColourValue(a);

                marker.P = m.P;

                return new MHSLAColour(h, s, l, a);
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

                ValidateHSLAColourValue(s);
                ValidateHSLAColourValue(l);

                marker.P = m.P;

                return new MHSLColour(h, s, l);
            }

            return null;
        }

        /// <summary>
        /// Gets an RGBA colour at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
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

                var r = new MNumber(Convert.ToInt32(hn.Substring(0, 2), 16).ToString());
                var g = new MNumber(Convert.ToInt32(hn.Substring(2, 2), 16).ToString());
                var b = new MNumber(Convert.ToInt32(hn.Substring(4, 2), 16).ToString());

                if (hn.Length == 8)
                {
                    var a = new MNumber(Convert.ToInt32(hn.Substring(6, 2), 16).ToString());

                    marker.P = m.P;

                    var colour = new MRGBAColour(r, g, b, a);

                    colour.Form = RGBAColourForm.Hexadecimal;

                    return colour;
                }
                else
                {
                    marker.P = m.P;

                    var colour = new MRGBColour(r, g, b);

                    colour.Form = RGBAColourForm.Hexadecimal;

                    return colour;
                }
            }

            if (m.P > inputText.Length - 4)
            {
                return null;
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

                marker.P = m.P;

                return new MRGBAColour(r, g, b, a);
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

                marker.P = m.P;

                return new MRGBColour(r, g, b);
            }

            return null;
        }

        /// <summary>
        /// Checks that a number is valid as a CMYK colour value.
        /// </summary>
        /// <param name="value"></param>
        public void ValidateCMYKColourValue(object value)
        {
            if (value is MNumber)
            {
                var v = (value as MNumber).Value;

                if (v < 0 || v > 1)
                {
                    throw new MorphSyntaxError("CMYK colour values must be between 0 and 1.");
                }
            }
            if (value is MPercentage)
            {
                var v = (value as MPercentage).Value * 100;

                if (v < 0 || v > 100)
                {
                    throw new MorphSyntaxError("CMYK colour values must be between 0% and 100%.");
                }
            }
        }

        /// <summary>
        /// Checks that a number is valid as a HSLA colour value.
        /// </summary>
        /// <param name="value"></param>
        public void ValidateHSLAColourValue(object value)
        {
            if (value is MNumber)
            {
                var v = (value as MNumber).Value;

                if (v < 0 || v > 1)
                {
                    throw new MorphSyntaxError("HSLA colour values must be between 0 and 1.");
                }
            }
            if (value is MPercentage)
            {
                var v = (value as MPercentage).Value * 100;

                if (v < 0 || v > 100)
                {
                    throw new MorphSyntaxError("HSLA colour values must be between 0% and 100%.");
                }
            }
        }

        /// <summary>
        /// Checks that a number is valid as an RGBA colour value.
        /// </summary>
        /// <param name="value"></param>
        public void ValidateRGBAColourValue(object value)
        {
            if (value is MNumber)
            {
                var v = (value as MNumber).Value;

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

        /// <summary>
        /// Gets a hexadecimal number at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public string GetHexadecimalNumber(string inputText, Marker marker)
        {
            var m = marker.Copy();

            if (Expect(inputText, m, "#") == false)
            {
                return null;
            }

            var start = m.P;

            while (m.P < inputText.Length)
            {
                var c = inputText.Substring(m.P, 1)[0];

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

        /// <summary>
        /// Gets a number set at the current position and returns it. A number set is simply a list of numbers and percentages.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public List<IMNumeric> GetNumberSet(string inputText, Marker marker)
        {
            var m = marker.Copy();

            if (Expect(inputText, m, "(") == false)
            {
                return null;
            }

            var numbers = new List<IMNumeric>();

            while (m.P < inputText.Length)
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

                if (m.P >= inputText.Length)
                {
                    break;
                }

                var c = inputText.Substring(m.P, 1);

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

            marker.P = m.P;

            return numbers;
        }

        /// <summary>
        /// Gets a percentage at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public MPercentage GetPercentage(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var number = GetNumber(inputText, m);

            if (number == null)
            {
                return null;
            }

            if (Expect(inputText, m, "%") == false)
            {
                return null;
            }

            marker.P = m.P;

            return new MPercentage(number.Text);
        }

        /// <summary>
        /// Gets a length set at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public MLengthSet GetLengthSet(string inputText, Marker marker)
        {
            var m = marker.Copy();

            var lengths = new List<MLength>();

            while (m.P < inputText.Length)
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

        /// <summary>
        /// Gets a length at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets a number at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public MNumber GetNumber(string inputText, Marker marker)
        {
            // This is a very basic function for finding a decimal number. It doesn't account for things like thousands separators or standard form. This can be upgraded later if necessary.

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

        /// <summary>
        /// Gets a length unit at the current position and returns it.
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
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
