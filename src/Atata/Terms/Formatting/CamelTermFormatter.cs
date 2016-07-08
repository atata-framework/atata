using System.Linq;

namespace Atata.TermFormatting
{
    public class CamelTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Concat(
                new[] { words.First().ToLower() }.
                    Concat(words.Skip(1).Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower())));
        }
    }
}
