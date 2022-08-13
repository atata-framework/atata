using System.Globalization;
using System.Linq;

namespace Atata
{
    public class PascalKebabTermFormatter : ITermFormatter
    {
        public string Format(string[] words) =>
            string.Join("-", words.Select(x => char.ToUpper(x[0], CultureInfo.CurrentCulture) + x.Substring(1).ToLower(CultureInfo.CurrentCulture)));
    }
}
