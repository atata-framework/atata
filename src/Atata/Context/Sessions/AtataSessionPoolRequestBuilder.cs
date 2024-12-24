#nullable enable

namespace Atata;

/// <summary>
/// Represents a builder of a session taking from pool request.
/// </summary>
public sealed class AtataSessionPoolRequestBuilder : AtataSessionRequestBuilder
{
    internal AtataSessionPoolRequestBuilder(Type sessionType)
        : base(sessionType)
    {
    }

    protected override async Task StartAsync(AtataContext context, CancellationToken cancellationToken) =>
        await context.Sessions.TakeFromPoolAsync(Type, Name, cancellationToken)
            .ConfigureAwait(false);
}
