using System.Linq;

namespace Atata.TermFormatting
{
    public class UpperCaseTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Join(" ", words.Select(x => x.ToUpper()));
        }
    }
}
