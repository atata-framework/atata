using System.Linq;

namespace Atata.TermFormatting
{
    public class PascalTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Concat(words.Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
        }
    }
}
