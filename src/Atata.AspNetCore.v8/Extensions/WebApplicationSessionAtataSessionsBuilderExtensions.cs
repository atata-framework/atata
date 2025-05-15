using Atata.AspNetCore;

namespace Atata;

/// <summary>
/// A set of extension methods for <see cref="AtataSessionsBuilder"/> to add and configure <see cref="WebApplicationSessionBuilder"/> session builders.
/// </summary>
public static class WebApplicationSessionAtataSessionsBuilderExtensions
{
    /// <summary>
    /// Adds a new instance of <see cref="WebApplicationSessionBuilder"/> builder.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebApplicationSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder AddWebApplication(
        this AtataSessionsBuilder builder,
        Action<WebApplicationSessionBuilder>? configure = null)
        =>
        builder.Add(configure);

    /// <summary>
    /// Adds a new instance of <see cref="WebApplicationSessionBuilder{TSession}"/> builder.
    /// </summary>
    /// <typeparam name="TSession">The type of the web application session, which should inherit <see cref="WebApplicationSession"/>.</typeparam>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebApplicationSessionBuilder{TSession}"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder AddWebApplication<TSession>(
        this AtataSessionsBuilder builder,
        Action<WebApplicationSessionBuilder<TSession>>? configure = null)
        where TSession : WebApplicationSession, new()
        =>
        builder.Add(configure);

    /// <summary>
    /// Configures existing nameless <see cref="WebApplicationSessionBuilder"/> session builder.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebApplicationSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureWebApplication(
        this AtataSessionsBuilder builder,
        Action<WebApplicationSessionBuilder> configure,
        ConfigurationMode mode = default)
        =>
        builder.Configure(configure, mode);

    /// <summary>
    /// Configures existing nameless <see cref="WebApplicationSessionBuilder{TSession}"/> session builder.
    /// </summary>
    /// <typeparam name="TSession">The type of the web application session, which should inherit <see cref="WebApplicationSession"/>.</typeparam>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebApplicationSessionBuilder{TSession}"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureWebApplication<TSession>(
        this AtataSessionsBuilder builder,
        Action<WebApplicationSessionBuilder<TSession>> configure,
        ConfigurationMode mode = default)
        where TSession : WebApplicationSession, new()
        =>
        builder.Configure(configure, mode);

    /// <summary>
    /// Configures existing <see cref="WebApplicationSessionBuilder"/> session builder that has the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebApplicationSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureWebApplication(
        this AtataSessionsBuilder builder,
        string? name,
        Action<WebApplicationSessionBuilder> configure,
        ConfigurationMode mode = default)
        =>
        builder.Configure(name, configure, mode);

    /// <summary>
    /// Configures existing <see cref="WebApplicationSessionBuilder{TSession}"/> session builder that has the specified <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="TSession">The type of the web application session, which should inherit <see cref="WebApplicationSession"/>.</typeparam>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebApplicationSessionBuilder{TSession}"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureWebApplication<TSession>(
        this AtataSessionsBuilder builder,
        string? name,
        Action<WebApplicationSessionBuilder<TSession>> configure,
        ConfigurationMode mode = default)
        where TSession : WebApplicationSession, new()
        =>
        builder.Configure(name, configure, mode);
}
