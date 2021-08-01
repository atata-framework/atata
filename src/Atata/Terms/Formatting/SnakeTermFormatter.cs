using System.Globalization;

namespace Atata.TermFormatting
{
    public class SnakeTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join("_", words).ToLower(CultureInfo.CurrentCulture);
        }
    }
}
