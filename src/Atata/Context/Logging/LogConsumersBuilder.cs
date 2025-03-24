#nullable enable

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
        consumer.CheckNotNull(nameof(consumer));

        ScopeLimitedLogConsumerConfiguration consumerConfiguration = new(new(consumer));
        _configurations.Add(consumerConfiguration);

        configure?.Invoke(new(consumerConfiguration));

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures log consumer builder for existing <typeparamref name="TLogConsumer"/> log consumer;
    /// or throws <see cref="LogConsumerNotFoundException"/> if consumer of such type is not found.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <typeparamref name="TLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure<TLogConsumer>(
        Action<LogConsumerBuilder<TLogConsumer>> configure)
        where TLogConsumer : ILogConsumer
    {
        configure.CheckNotNull(nameof(configure));

        var consumerConfiguration = GetConfigurationOrNull<TLogConsumer>()
            ?? throw LogConsumerNotFoundException.ByBuilderType(typeof(TLogConsumer));

        configure.Invoke(new(consumerConfiguration));

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures log consumer builder for <typeparamref name="TLogConsumer"/> log consumer if it exists.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <typeparamref name="TLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder ConfigureIfExists<TLogConsumer>(
        Action<LogConsumerBuilder<TLogConsumer>> configure)
        where TLogConsumer : ILogConsumer
    {
        configure.CheckNotNull(nameof(configure));

        var consumerConfiguration = GetConfigurationOrNull<TLogConsumer>();

        if (consumerConfiguration is not null)
            configure.Invoke(new(consumerConfiguration));

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures log consumer builder for existing <typeparamref name="TLogConsumer"/> log consumer;
    /// or adds a new one if such consumer doesn't exist.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <typeparamref name="TLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder ConfigureOrAdd<TLogConsumer>(
        Action<LogConsumerBuilder<TLogConsumer>>? configure = null)
        where TLogConsumer : ILogConsumer, new()
    {
        var consumerConfiguration = GetConfigurationOrNull<TLogConsumer>();

        if (consumerConfiguration is null)
        {
            TLogConsumer consumer = new();
            return Add(consumer, configure);
        }
        else
        {
            configure?.Invoke(new(consumerConfiguration));
            return _atataContextBuilder;
        }
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
        logConsumer.CheckNotNull(nameof(logConsumer));

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
        logConsumerConfiguration.CheckNotNull(nameof(logConsumerConfiguration));

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

    private ScopeLimitedLogConsumerConfiguration GetConfigurationOrNull<TLogConsumer>()
        where TLogConsumer : ILogConsumer
        =>
        _configurations.LastOrDefault(x => x.ConsumerConfiguration.Consumer is TLogConsumer);
}
