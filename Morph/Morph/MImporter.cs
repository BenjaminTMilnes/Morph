using System;
using System.Collections.Generic;
using System.Text;
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
