#nullable enable

namespace Atata;

/// <summary>
/// Represents a builder of a session borrow request.
/// </summary>
public sealed class AtataSessionBorrowRequestBuilder : AtataSessionRequestBuilder<AtataSessionBorrowRequestBuilder>
{
    internal AtataSessionBorrowRequestBuilder(Type sessionType)
        : base(sessionType)
    {
    }

    protected override async Task StartAsync(AtataContext context, CancellationToken cancellationToken) =>
        await context.Sessions.BorrowAsync(Type, Name, cancellationToken)
            .ConfigureAwait(false);
}
