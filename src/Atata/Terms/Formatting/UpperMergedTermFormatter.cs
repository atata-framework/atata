using System.Globalization;

namespace Atata.TermFormatting
{
    public class UpperMergedTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Concat(words).ToUpper(CultureInfo.CurrentCulture);
        }
    }
}
