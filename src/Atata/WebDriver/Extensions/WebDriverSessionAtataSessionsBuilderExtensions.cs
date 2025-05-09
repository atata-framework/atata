namespace Atata;

/// <summary>
/// A set of extension methods for <see cref="AtataSessionsBuilder"/> to add and configure <see cref="WebDriverSessionBuilder"/> session builders.
/// </summary>
public static class WebDriverSessionAtataSessionsBuilderExtensions
{
    /// <summary>
    /// Adds a new instance of <see cref="WebDriverSessionBuilder"/> builder.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebDriverSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder AddWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder>? configure = null) =>
        builder.Add(configure);

    /// <summary>
    /// Configures existing nameless <see cref="WebDriverSessionBuilder"/> session builder.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebDriverSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder> configure, ConfigurationMode mode = default) =>
        builder.Configure(configure, mode);

    /// <summary>
    /// Configures existing <see cref="WebDriverSessionBuilder"/> session builder that has the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="WebDriverSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureWebDriver(this AtataSessionsBuilder builder, string? name, Action<WebDriverSessionBuilder> configure, ConfigurationMode mode = default) =>
        builder.Configure(name, configure, mode);
}
