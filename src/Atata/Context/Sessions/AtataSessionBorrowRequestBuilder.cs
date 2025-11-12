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

    /// <inheritdoc/>
    protected override async Task RequestSessionAsync(AtataContext context, CancellationToken cancellationToken) =>
        await context.Sessions.BorrowAsync(Type, Name, cancellationToken)
            .ConfigureAwait(false);
}
