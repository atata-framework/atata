using System;
using System.Globalization;

namespace Atata
{
    public class AtataTemplateStringFormatter : IFormatProvider, ICustomFormatter
    {
        private const string InnerFormatValueIndicator = "*";

        public static AtataTemplateStringFormatter Default { get; } = new AtataTemplateStringFormatter();

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

            if (!string.IsNullOrEmpty(format))
            {
                if (arg is IFormattable argFormattable)
                    return argFormattable.ToString(format, CultureInfo.CurrentCulture);

                string argumentAsString = arg as string ?? arg.ToString();
                return FormatInnerString(format, argumentAsString);
            }
            else
            {
                return arg.ToString();
            }
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
