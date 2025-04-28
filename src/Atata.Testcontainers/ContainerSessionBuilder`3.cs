using Microsoft.Extensions.Logging.Abstractions;

namespace Atata.Testcontainers;

/// <summary>
/// Represents a builder for creating and configuring a container session.
/// </summary>
/// <typeparam name="TContainer">The type of the container.</typeparam>
/// <typeparam name="TSession">The type of the session to build, which must inherit from <see cref="ContainerSession{TContainer}"/>.</typeparam>
/// <typeparam name="TBuilder">The type of the inherited builder.</typeparam>
public abstract class ContainerSessionBuilder<TContainer, TSession, TBuilder> : AtataSessionBuilder<TSession, TBuilder>
    where TContainer : IContainer
    where TSession : ContainerSession<TContainer>, new()
    where TBuilder : ContainerSessionBuilder<TContainer, TSession, TBuilder>
{
    private Func<TContainer>? _containerCreator;

    private Func<ILogger> _containerLoggerCreator = () => NullLogger.Instance;

    /// <summary>
    /// Gets the configuration for saving container logs.
    /// </summary>
    public ContainerLogsSaveConfiguration LogsSaveConfiguration { get; private set; } =
        ContainerLogsSaveConfiguration.Default.Clone();

    /// <summary>
    /// Configures the builder to use a specific container builder.
    /// </summary>
    /// <typeparam name="TContainerBuilder">The type of the container builder.</typeparam>
    /// <param name="containerBuilderCreator">The function to create the container builder.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="containerBuilderCreator"/> is <see langword="null"/>.</exception>
    public TBuilder Use<TContainerBuilder>(Func<TContainerBuilder> containerBuilderCreator)
        where TContainerBuilder : IContainerBuilder<TContainerBuilder, TContainer>
    {
        Guard.ThrowIfNull(containerBuilderCreator);

        _containerCreator = () =>
        {
            TContainerBuilder containerBuilder = containerBuilderCreator.Invoke();

            ILogger? containerLogger = _containerLoggerCreator?.Invoke();

            if (containerLogger is not null)
                containerBuilder = containerBuilder.WithLogger(containerLogger);

            return containerBuilder.Build();
        };
        return (TBuilder)this;
    }

    /// <summary>
    /// Configures the builder to use a specific logger for the container.
    /// </summary>
    /// <param name="containerLoggerCreator">The function to create the logger.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseContainerLogger(Func<ILogger> containerLoggerCreator)
    {
        _containerLoggerCreator = containerLoggerCreator;

        return (TBuilder)this;
    }

    /// <summary>
    /// Configures the builder to use a specific configuration for saving container logs.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="ContainerLogsSaveConfiguration"/>.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <see langword="null"/>.</exception>
    public TBuilder UseLogsSaveConfiguration(Action<ContainerLogsSaveConfiguration> configure)
    {
        Guard.ThrowIfNull(configure);

        configure.Invoke(LogsSaveConfiguration);

        return (TBuilder)this;
    }

    /// <summary>
    /// Configures the builder to use a specific instance of <see cref="ContainerLogsSaveConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The configuration instance to use.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is <see langword="null"/>.</exception>
    public TBuilder UseLogsSaveConfiguration(ContainerLogsSaveConfiguration configuration)
    {
        Guard.ThrowIfNull(configuration);

        LogsSaveConfiguration = configuration;

        return (TBuilder)this;
    }

    protected override void ValidateConfiguration()
    {
        base.ValidateConfiguration();

        if (_containerCreator is null)
            throw new AtataSessionBuilderValidationException("Container is not set. Use 'Use' method to set it.");
    }

    protected override void ConfigureSession(TSession session)
    {
        base.ConfigureSession(session);

        TContainer container = default!;

        session.Log.ExecuteSection(
            new LogSection("Build container", LogLevel.Trace),
            () =>
            {
                container = _containerCreator!.Invoke();
                return container.Image.FullName;
            });

        session.Container = container;
        session.LogsSaveConfiguration = LogsSaveConfiguration;

        session.Variables["container-image-fullname"] = container.Image.FullName;
        session.Variables["container-image-repository"] = container.Image.Repository;
        session.Variables["container-image-registry"] = container.Image.Registry;
        session.Variables["container-image-tag"] = container.Image.Tag;
        session.Variables["container-image-digest"] = container.Image.Digest;
    }

    protected override void OnClone(TBuilder copy)
    {
        base.OnClone(copy);

        copy.LogsSaveConfiguration = LogsSaveConfiguration.Clone();
    }
}
