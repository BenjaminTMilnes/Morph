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

            var start = m.P;

            var c = inputText.Substring(m.P, 1)[0];

            if (c != '#')
            {
                return null;
            }

            m.P++;

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
