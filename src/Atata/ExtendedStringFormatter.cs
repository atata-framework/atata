using System;
using System.Globalization;

namespace Atata
{
    public class ExtendedStringFormatter : IFormatProvider, ICustomFormatter
    {
        private const string InnerFormatValueIndicator = "*";

        public static ExtendedStringFormatter Default { get; } = new ExtendedStringFormatter();

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!Equals(formatProvider))
                return null;

            if (arg == null)
                return string.Empty;

            if (arg is string argString && !string.IsNullOrWhiteSpace(format))
                return FormatInnerString(format, argString);
            else if (arg is IFormattable argFormattable)
                return argFormattable.ToString(format, CultureInfo.CurrentCulture);
            else
                return arg.ToString();
        }

        private static string FormatInnerString(string format, string argument)
        {
            string normalizedFormat = format.
                Replace(InnerFormatValueIndicator, "{0}").
                Replace("{0}{0}", InnerFormatValueIndicator);

            return string.Format(normalizedFormat, argument);
        }
    }
}
