using System.Globalization;
using System.Text;

namespace System
{
    internal static class StringExtensions
    {
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var ti = CultureInfo.InvariantCulture.TextInfo;
            return ti.ToTitleCase(ti.ToLower(value));
        }

        public static string UppercaseFirst(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            char[] a = value.ToCharArray();
            a[0] = char.ToUpper(value[0]);
            return new string(a);
        }

        public static string Repeat(this string value, int times)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            for (int i = 0; i < times; i++)
                sb.Append(value);
            
            return sb.ToString();
        }
    }
}
