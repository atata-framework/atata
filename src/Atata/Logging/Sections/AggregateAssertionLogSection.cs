using System.Text;

namespace Atata
{
    public class AggregateAssertionLogSection : LogSection
    {
        public AggregateAssertionLogSection(string assertionScopeName)
        {
            StringBuilder builder = new StringBuilder("Aggregate assert");

            if (!string.IsNullOrEmpty(assertionScopeName))
                builder.Append(" ").Append(assertionScopeName);

            Message = builder.ToString();
        }
    }
}
