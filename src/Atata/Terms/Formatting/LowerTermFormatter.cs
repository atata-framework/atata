using System.Globalization;

namespace Atata
{
    public class LowerTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join(" ", words).ToLower(CultureInfo.CurrentCulture);
        }
    }
}
