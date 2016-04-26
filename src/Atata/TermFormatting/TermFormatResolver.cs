using System.Collections.Generic;
using System.Linq;

namespace Atata.TermFormatting
{
    public static class TermFormatResolver
    {
        private static readonly Dictionary<TermFormat, ITermFormatter> Formatters;

        static TermFormatResolver()
        {
            Formatters = new Dictionary<TermFormat, ITermFormatter>
            {
                { TermFormat.Title, new TitleTermFormatter() },
                { TermFormat.Camel, new CamelTermFormatter() },
                { TermFormat.Dashed, new DashedTermFormatter() },
                { TermFormat.XDashed, new XDashedTermFormatter() },
                { TermFormat.Underscored, new UnderscoredTermFormatter() }
            };
        }

        public static string ApplyFormat(string value, TermFormat format)
        {
            ITermFormatter formatter = Formatters[format];
            string[] words = value.SplitIntoWords();

            if (!words.Any())
                return string.Empty;

            return formatter.Format(words);
        }
    }
}
