using System;
using System.Linq;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Finds the first index at which the <paramref name="value"/> does not match the <paramref name="expected"/>
        /// string anymore, including the exact casing.
        /// </summary>
        public static int IndexOfFirstMismatch(this string value, string expected)
        {
            return IndexOfFirstMismatch(value, expected, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Finds the first index at which the <paramref name="value"/> does not match the <paramref name="expected"/>
        /// string anymore, accounting for the specified <paramref name="stringComparison"/>.
        /// </summary>
        public static int IndexOfFirstMismatch(this string value, string expected, StringComparison stringComparison)
        {
            for (int index = 0; index < value.Length; index++)
            {
                if ((index >= expected.Length) || !value[index].ToString().Equals(expected[index].ToString(), stringComparison))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the quoted three characters at the specified index of a string, including the index itself.
        /// </summary>
        public static string IndexedSegmentAt(this string value, int index)
        {
            int length = Math.Min(value.Length - index, 3);

            return String.Format("{0} (index {1})", Formatter.ToString(value.Substring(index, length)), index);
        }

        /// <summary>
        /// Replaces all characters that might conflict with formatting placeholders and newlines with their escaped counterparts.
        /// </summary>
        public static string Escape(this string value)
        {
            return value.Replace("\"", "\\\"").Replace("\n", @"\n").Replace("\r", @"\r").Replace("{", "{{").Replace(
                "}", "}}");
        }

        /// <summary>
        /// Joins a string with one or more other strings using a specified separator.
        /// </summary>
        /// <remarks>
        /// Any string that is empty (including the original string) is ignored. 
        /// </remarks>
        public static string Combine(this string @this, string other, string separator = ".")
        {
            var strings = new[] { @this }.Concat(new[] {other}).Where(s => s.Length > 0).ToArray();
            return string.Join(separator, strings);
        }
    }
}