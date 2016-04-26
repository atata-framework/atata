using System.Linq;

namespace Atata.TermFormatting
{
    public class TitleTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join(" ", words.Select(x => x.ToUpper() == x ? x : x.ToUpperFirstLetter()));
        }
    }
}
