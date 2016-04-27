using System.Linq;

namespace Atata.TermFormatting
{
    public class PascalHyphenatedTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join("‐", words.Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
        }
    }
}
