using System.Linq;

namespace Atata.TermFormatting
{
    public class XDashedTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return "x-" + string.Join("-", words.Select(x => x.ToLower()));
        }
    }
}
