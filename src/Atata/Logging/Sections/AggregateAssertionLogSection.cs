namespace Atata;

public class AggregateAssertionLogSection : LogSection
{
    public AggregateAssertionLogSection(string? assertionScopeName = null)
    {
        StringBuilder builder = new("Aggregate assert");

        if (assertionScopeName?.Length > 0)
            builder.Append(' ').Append(assertionScopeName);

        Message = builder.ToString();
    }
}
