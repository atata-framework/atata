using System.Globalization;

namespace Atata.TermFormatting
{
    public class LowerTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join(" ", words).ToLower(CultureInfo.CurrentCulture);
        }
    }
}
