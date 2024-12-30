#nullable enable

namespace Atata;

/// <summary>
/// Represents a builder of a session taking from pool request.
/// </summary>
public sealed class AtataSessionPoolRequestBuilder : AtataSessionRequestBuilder<AtataSessionPoolRequestBuilder>
{
    internal AtataSessionPoolRequestBuilder(Type sessionType)
        : base(sessionType)
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether to use a shared session mode.
    /// Shared session can be borrowed by child contexts.
    /// The default value is <see langword="false"/>.
    /// </summary>
    public bool SharedMode { get; set; }

    /// <summary>
    /// Sets a value indicating whether to use a shared session mode.
    /// Shared session can be borrowed by child contexts.
    /// The default value is <see langword="false"/>.
    /// </summary>
    /// <param name="enable">Whether to enable shared mode.</param>
    /// <returns>The same <see cref="AtataSessionPoolRequestBuilder"/> instance.</returns>
    public AtataSessionPoolRequestBuilder UseSharedMode(bool enable)
    {
        SharedMode = enable;
        return this;
    }

    protected override async Task StartAsync(AtataContext context, CancellationToken cancellationToken)
    {
        AtataSession session = await context.Sessions.TakeFromPoolAsync(Type, Name, cancellationToken)
            .ConfigureAwait(false);

        session.IsShareable = SharedMode;
    }
}
