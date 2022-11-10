using System;

namespace Atata
{
    internal sealed class AtataPathTemplateStringFormatter : IFormatProvider, ICustomFormatter
    {
        private readonly AtataTemplateStringFormatter _templateFormatter = AtataTemplateStringFormatter.Default;

        private AtataPathTemplateStringFormatter()
        {
        }

        public static AtataPathTemplateStringFormatter Default { get; } = new AtataPathTemplateStringFormatter();

        public object GetFormat(Type formatType) =>
            formatType == typeof(ICustomFormatter) ? this : null;

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is null)
                return string.Empty;

            string origin = _templateFormatter.Format(format, arg, formatProvider);

            char[] result = new char[origin.Length];

            for (int i = 0; i < origin.Length; i++)
            {
                switch (origin[i])
                {
                    case '"':
                    case '<':
                    case '>':
                    case '|':
                    case '\u0000':
                    case '\u0001':
                    case '\u0002':
                    case '\u0003':
                    case '\u0004':
                    case '\u0005':
                    case '\u0006':
                    case '\u0007':
                    case '\u0008':
                    case '\u0009':
                    case '\u000a':
                    case '\u000b':
                    case '\u000c':
                    case '\u000d':
                    case '\u000e':
                    case '\u000f':
                    case '\u0010':
                    case '\u0011':
                    case '\u0012':
                    case '\u0013':
                    case '\u0014':
                    case '\u0015':
                    case '\u0016':
                    case '\u0017':
                    case '\u0018':
                    case '\u0019':
                    case '\u001a':
                    case '\u001b':
                    case '\u001c':
                    case '\u001d':
                    case '\u001e':
                    case '\u001f':
                    case ':':
                    case '*':
                    case '?':
                    case '\\':
                    case '/':
                        result[i] = '_';
                        break;
                    default:
                        result[i] = origin[i];
                        break;
                }
            }

            return new string(result);
        }
    }
}
