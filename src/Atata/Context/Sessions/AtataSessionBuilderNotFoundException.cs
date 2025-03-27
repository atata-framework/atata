#nullable enable

namespace Atata;

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

    internal static AtataSessionBuilderNotFoundException BySessionType(Type sessionType, string? sessionName, AtataContext? context = null)
    {
        var messageBuilder = new StringBuilder("Failed to find session builder for ")
            .Append(AtataSession.BuildTypedName(sessionType, sessionName));

        if (context is not null)
            messageBuilder.Append(" in ").Append(context);

        messageBuilder.Append('.');

        return new(messageBuilder.ToString());
    }

    internal static AtataSessionBuilderNotFoundException ByBuilderType(Type sessionBuilderType, string? sessionBuilderName, AtataContext? context = null)
    {
        var messageBuilder = new StringBuilder("Failed to find ")
            .Append(sessionBuilderType.Name);

        if (sessionBuilderName is not null)
            messageBuilder.Append(" { Name=").Append(sessionBuilderName).Append(" }");

        if (context is not null)
            messageBuilder.Append(" in ").Append(context);

        messageBuilder.Append('.');

        return new(messageBuilder.ToString());
    }
}
