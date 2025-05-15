using Atata.AspNetCore;

namespace Atata;

/// <summary>
/// A set of extension methods for <see cref="AtataSessionCollection"/> to add <see cref="WebApplicationSessionBuilder"/> and <see cref="WebApplicationSessionBuilder{TSession}"/> session builders.
/// </summary>
public static class WebApplicationSessionAtataSessionCollectionExtensions
{
    /// <summary>
    /// Creates a new <see cref="WebApplicationSessionBuilder"/> and adds it to the collection.
    /// </summary>
    /// <param name="collection">The session collection.</param>
    /// <param name="configure">An action delegate to configure the <see cref="WebApplicationSessionBuilder"/>.</param>
    /// <returns>The created <see cref="WebApplicationSessionBuilder"/> instance.</returns>
    public static WebApplicationSessionBuilder AddWebApplication(
        this AtataSessionCollection collection,
        Action<WebApplicationSessionBuilder>? configure = null)
        =>
        collection.Add(configure);

    /// <summary>
    /// Creates a new <see cref="WebApplicationSessionBuilder{TSession}"/> and adds it to the collection.
    /// </summary>
    /// <typeparam name="TSession">The type of the web application session, which should inherit <see cref="WebApplicationSession"/>.</typeparam>
    /// <param name="collection">The session collection.</param>
    /// <param name="configure">An action delegate to configure the <see cref="WebApplicationSessionBuilder{TSession}"/>.</param>
    /// <returns>The created <see cref="WebApplicationSessionBuilder{TSession}"/> instance.</returns>
    public static WebApplicationSessionBuilder<TSession> AddWebApplication<TSession>(
        this AtataSessionCollection collection,
        Action<WebApplicationSessionBuilder<TSession>>? configure = null)
        where TSession : WebApplicationSession, new()
        =>
        collection.Add(configure);
}
