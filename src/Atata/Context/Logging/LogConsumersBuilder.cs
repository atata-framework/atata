namespace Atata;

/// <summary>
/// A builder of log consumers.
/// </summary>
public sealed class LogConsumersBuilder
{
    private readonly AtataContextBuilder _atataContextBuilder;

    private readonly List<ScopeLimitedLogConsumerConfiguration> _configurations;

    internal LogConsumersBuilder(AtataContextBuilder atataContextBuilder)
        : this(atataContextBuilder, [])
    {
    }

    private LogConsumersBuilder(
        AtataContextBuilder atataContextBuilder,
        List<ScopeLimitedLogConsumerConfiguration> items)
    {
        _atataContextBuilder = atataContextBuilder;
        _configurations = items;
    }

    /// <summary>
    /// Gets the list of log consumer configurations.
    /// </summary>
    public IEnumerable<LogConsumerConfiguration> Items =>
        _configurations.Select(x => x.ConsumerConfiguration);

    /// <summary>
    /// Adds a log consumer of the specified type.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer. Should have public default constructor.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <typeparamref name="TLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Add<TLogConsumer>(
        Action<LogConsumerBuilder<TLogConsumer>>? configure = null)
        where TLogConsumer : ILogConsumer, new()
        =>
        Add(new TLogConsumer(), configure);

    /// <summary>
    /// Adds a log consumer by its type name or alias.
    /// Predefined aliases are defined in <see cref="LogConsumerAliases"/> static class.
    /// </summary>
    /// <param name="typeNameOrAlias">The type name or alias of the log consumer.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <see cref="ILogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Add(
        string typeNameOrAlias,
        Action<LogConsumerBuilder<ILogConsumer>>? configure = null)
    {
        ILogConsumer consumer = LogConsumerAliases.Resolve(typeNameOrAlias);
        return Add(consumer, configure);
    }

    /// <summary>
    /// Adds the log consumer.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="consumer">The log consumer.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <typeparamref name="TLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Add<TLogConsumer>(
        TLogConsumer consumer,
        Action<LogConsumerBuilder<TLogConsumer>>? configure = null)
        where TLogConsumer : ILogConsumer
    {
        Guard.ThrowIfNull(consumer);

        ScopeLimitedLogConsumerConfiguration consumerConfiguration = new(new(consumer));
        _configurations.Add(consumerConfiguration);

        configure?.Invoke(new(consumerConfiguration));

        return _atataContextBuilder;
    }

    /// <summary>
    /// <para>
    /// Configures a log consumer builder for existing <typeparamref name="TLogConsumer"/> log consumer.
    /// </para>
    /// <para>
    /// The <paramref name="mode"/> (<see cref="ConfigurationMode.ConfigureOrThrow"/> by default)
    /// parameter specifies the behavior of the fallback logic when the log consumer builder is not found:
    /// <list type="bullet">
    /// <item><see cref="ConfigurationMode.ConfigureOrThrow"/> - configures the builder or throws the <see cref="LogConsumerNotFoundException"/> if it is not found.</item>
    /// <item><see cref="ConfigurationMode.ConfigureIfExists"/> - configures the builder only if it exists; otherwise, no action is taken.</item>
    /// <item><see cref="ConfigurationMode.ConfigureOrAdd"/> - configures the builder if it exists, or adds a new builder if it does not exist.</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <typeparamref name="TLogConsumer"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure<TLogConsumer>(
        Action<LogConsumerBuilder<TLogConsumer>> configure,
        ConfigurationMode mode = default)
        where TLogConsumer : ILogConsumer
    {
        Guard.ThrowIfNull(configure);

        var consumerConfiguration = GetConfigurationOrNull<TLogConsumer>();

        if (consumerConfiguration is not null)
        {
            configure.Invoke(new(consumerConfiguration));
        }
        else
        {
            switch (mode)
            {
                case ConfigurationMode.ConfigureOrThrow:
                    throw LogConsumerNotFoundException.ByBuilderType(typeof(TLogConsumer));
                case ConfigurationMode.ConfigureIfExists:
                    break;
                case ConfigurationMode.ConfigureOrAdd:
                    TLogConsumer consumer = ActivatorEx.CreateInstance<TLogConsumer>();
                    Add(consumer, configure);
                    break;
                default:
                    throw Guard.CreateArgumentExceptionForUnsupportedValue(mode);
            }
        }

        return _atataContextBuilder;
    }

    /// <summary>
    /// Adds the <see cref="TraceLogConsumer"/> instance that uses <see cref="Trace"/> class for logging.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <see cref="TraceLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder AddTrace(
        Action<LogConsumerBuilder<TraceLogConsumer>>? configure = null)
        =>
        Add(configure);

    /// <summary>
    /// Adds the <see cref="DebugLogConsumer"/> instance that uses <see cref="Debug"/> class for logging.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <see cref="DebugLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder AddDebug(
        Action<LogConsumerBuilder<DebugLogConsumer>>? configure = null)
        =>
        Add(configure);

    /// <summary>
    /// Adds the <see cref="ConsoleLogConsumer"/> instance that uses <see cref="Console"/> class for logging.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <see cref="ConsoleLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder AddConsole(
        Action<LogConsumerBuilder<ConsoleLogConsumer>>? configure = null)
        =>
        Add(configure);

    /// <summary>
    /// Removes a log consumer configuration by the specified log consumer.
    /// </summary>
    /// <param name="logConsumer">The log consumer.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Remove(ILogConsumer logConsumer)
    {
        Guard.ThrowIfNull(logConsumer);

        var configuration = _configurations.FirstOrDefault(x => x.ConsumerConfiguration.Consumer == logConsumer);

        if (configuration is not null)
            _configurations.Remove(configuration);

        return _atataContextBuilder;
    }

    /// <summary>
    /// Removes the specified log consumer configuration.
    /// </summary>
    /// <param name="logConsumerConfiguration">The log consumer configuration.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Remove(LogConsumerConfiguration logConsumerConfiguration)
    {
        Guard.ThrowIfNull(logConsumerConfiguration);

        var configuration = _configurations.FirstOrDefault(x => x.ConsumerConfiguration == logConsumerConfiguration);

        if (configuration is not null)
            _configurations.Remove(configuration);

        return _atataContextBuilder;
    }

    /// <summary>
    /// Removes all log consumer configurations with log consumers of the specified <typeparamref name="TLogConsumer"/> type.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder RemoveAll<TLogConsumer>()
        where TLogConsumer : ILogConsumer
    {
        _configurations.RemoveAll(x => x.ConsumerConfiguration.Consumer is TLogConsumer);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Clears all log consumer configurations.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Clear()
    {
        _configurations.Clear();
        return _atataContextBuilder;
    }

    internal LogConsumersBuilder CloneFor(AtataContextBuilder atataContextBuilder) =>
        new(
            atataContextBuilder,
            [.. _configurations.Select(x => x.Clone())]);

    internal IEnumerable<LogConsumerConfiguration> GetItemsForScope(AtataContextScope? scope)
    {
        foreach (var item in _configurations)
        {
            if (item.Scopes.Satisfies(scope))
                yield return item.ConsumerConfiguration;
        }
    }

    private ScopeLimitedLogConsumerConfiguration? GetConfigurationOrNull<TLogConsumer>()
        where TLogConsumer : ILogConsumer
        =>
        _configurations.LastOrDefault(x => x.ConsumerConfiguration.Consumer is TLogConsumer);
}
