namespace Atata;

/// <summary>
/// An exception that is thrown when a session builder cannot be found.
/// </summary>
public class AtataSessionBuilderNotFoundException : Exception
{
    public AtataSessionBuilderNotFoundException()
    {
    }

    public AtataSessionBuilderNotFoundException(string? message)
        : base(message)
    {
    }

    public AtataSessionBuilderNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    internal static AtataSessionBuilderNotFoundException BySessionTypeAndName(Type? sessionType, string? sessionName, AtataContext? context = null)
    {
        StringBuilder messageBuilder = new("Failed to find session builder");

        if (sessionType is not null)
        {
            messageBuilder
                .Append(" for ")
                .Append(AtataSession.BuildTypedName(sessionType, sessionName));
        }
        else if (sessionName is not null)
        {
            messageBuilder
                .Append(" by name \"")
                .Append(sessionName)
                .Append('"');
        }

        if (context is not null)
            messageBuilder.Append(" in ").Append(context);

        messageBuilder.Append('.');

        return new(messageBuilder.ToString());
    }

    internal static AtataSessionBuilderNotFoundException ByBuilderType(Type sessionBuilderType, string? sessionBuilderName, AtataContext? context = null)
    {
        var messageBuilder = new StringBuilder("Failed to find ")
            .Append(sessionBuilderType.ToStringInShortForm());

        if (sessionBuilderName is not null)
            messageBuilder.Append(" { Name=").Append(sessionBuilderName).Append(" }");

        if (context is not null)
            messageBuilder.Append(" in ").Append(context);

        messageBuilder.Append('.');

        return new(messageBuilder.ToString());
    }
}
