using Atata.TermFormatting;

namespace Atata
{
    public static class TermFormatExtensions
    {
        public static string ApplyTo(this TermFormat format, string value)
        {
            return TermFormatResolver.ApplyFormat(value, format);
        }
    }
}
