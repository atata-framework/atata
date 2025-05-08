using Atata.Testcontainers;

namespace Atata;

/// <summary>
/// A set of extension methods for <see cref="AtataSessionsBuilder"/> to add and configure <see cref="ContainerSessionBuilder"/> session builders.
/// </summary>
public static class ContainerSessionAtataSessionsBuilderExtensions
{
    /// <summary>
    /// Adds a new instance of <see cref="ContainerSessionBuilder"/> builder.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="ContainerSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder AddContainer(
        this AtataSessionsBuilder builder,
        Action<ContainerSessionBuilder>? configure = null)
        =>
        builder.Add(configure);

    /// <summary>
    /// Adds a new instance of <see cref="ContainerSessionBuilder{TContainer}"/> builder.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="ContainerSessionBuilder{TContainer}"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder AddContainer<TContainer>(
        this AtataSessionsBuilder builder,
        Action<ContainerSessionBuilder<TContainer>>? configure = null)
        where TContainer : IContainer
        =>
        builder.Add(configure);

    /// <summary>
    /// Configures existing nameless <see cref="ContainerSessionBuilder"/> session builder.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="ContainerSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureContainer(
        this AtataSessionsBuilder builder,
        Action<ContainerSessionBuilder> configure,
        ConfigurationMode mode = default)
        =>
        builder.Configure(configure, mode);

    /// <summary>
    /// Configures existing nameless <see cref="ContainerSessionBuilder{TContainer}"/> session builder.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="ContainerSessionBuilder{TContainer}"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureContainer<TContainer>(
        this AtataSessionsBuilder builder,
        Action<ContainerSessionBuilder<TContainer>> configure,
        ConfigurationMode mode = default)
        where TContainer : IContainer
        =>
        builder.Configure(configure, mode);

    /// <summary>
    /// Configures existing <see cref="ContainerSessionBuilder"/> session builder that has the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="ContainerSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureContainer(
        this AtataSessionsBuilder builder,
        string? name,
        Action<ContainerSessionBuilder> configure,
        ConfigurationMode mode = default)
        =>
        builder.Configure(name, configure, mode);

    /// <summary>
    /// Configures existing <see cref="ContainerSessionBuilder{TContainer}"/> session builder that has the specified <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <param name="builder">The sessions builder.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="ContainerSessionBuilder{TContainer}"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ConfigureContainer<TContainer>(
        this AtataSessionsBuilder builder,
        string? name,
        Action<ContainerSessionBuilder<TContainer>> configure,
        ConfigurationMode mode = default)
        where TContainer : IContainer
        =>
        builder.Configure(name, configure, mode);
}
