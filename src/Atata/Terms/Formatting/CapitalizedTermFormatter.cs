using System.Linq;

namespace Atata
{
    public class CapitalizedTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join(" ", words.Select(x => x.IsUpper() ? x : x.ToUpperFirstLetter()));
        }
    }
}
