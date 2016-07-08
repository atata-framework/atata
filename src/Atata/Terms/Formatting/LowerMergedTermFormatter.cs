namespace Atata.TermFormatting
{
    public class LowerMergedTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Concat(words).ToLower();
        }
    }
}
