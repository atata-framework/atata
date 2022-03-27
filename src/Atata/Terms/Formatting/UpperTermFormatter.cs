using System.Globalization;

namespace Atata
{
    public class UpperTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join(" ", words).ToUpper(CultureInfo.CurrentCulture);
        }
    }
}
