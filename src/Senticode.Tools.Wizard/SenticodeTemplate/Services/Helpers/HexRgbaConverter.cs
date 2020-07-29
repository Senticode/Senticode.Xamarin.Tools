using System;
using System.Text.RegularExpressions;

namespace SenticodeTemplate.Services.Helpers
{
    internal static class HexRgbaConverter
    {
        private static readonly Regex HexColorWithAlphaRegex = new Regex("^#[a-fA-F0-9]{8}$");

        public static string ToHexWithNoAlpha(string color)
        {
            if (!HexColorWithAlphaRegex.IsMatch(color))
            {
                throw new NotSupportedException();
            }

            return $"#{color.Substring(2, color.Length - 3)}";
        }

        public static TColor ToRgbaColor<TColor>(string color)
            where TColor : struct
        {
            var colorType = typeof(TColor);
            object obj;

            if (!HexColorWithAlphaRegex.IsMatch(color))
            {
                throw new NotSupportedException();
            }

            if (colorType == typeof(System.Windows.Media.Color))
            {
                obj = System.Windows.Media.ColorConverter.ConvertFromString(color);
            }
            else if (colorType == typeof(System.Drawing.Color))
            {
                obj = System.Drawing.ColorTranslator.FromHtml(color);
            }
            else
            {
                throw new NotSupportedException();
            }

            return obj is TColor result ? result : throw new NotSupportedException();
        }
    }
}