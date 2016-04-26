using System.Linq;

namespace Atata.TermFormatting
{
    public class UnderscoredTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join("_", words.Select(x => x.ToLower()));
        }
    }
}
