using System.Globalization;

namespace Atata
{
    public class UpperMergedTermFormatter : ITermFormatter
    {
        public string Format(string[] words) =>
            string.Concat(words)
                .ToUpper(CultureInfo.CurrentCulture);
    }
}
