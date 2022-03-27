using System.Globalization;
using System.Linq;

namespace Atata
{
    public class HyphenKebabTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join("‐", words.Select(x => x.ToLower(CultureInfo.CurrentCulture)));
        }
    }
}
