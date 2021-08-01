using System.Globalization;

namespace Atata.TermFormatting
{
    public class UpperTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join(" ", words).ToUpper(CultureInfo.CurrentCulture);
        }
    }
}
