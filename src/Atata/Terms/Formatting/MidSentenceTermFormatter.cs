using System.Linq;

namespace Atata.TermFormatting
{
    public class MidSentenceTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join(" ",
                words.Select(x => x.Length >= 2 && x.IsUpper() ? x : x.ToLower()));
        }
    }
}
