﻿#nullable enable

namespace Atata;

/// <summary>
/// A builder of log consumers.
/// </summary>
public sealed class LogConsumersBuilder
{
    private readonly AtataContextBuilder _atataContextBuilder;

    private readonly List<LogConsumerConfiguration> _items;

    internal LogConsumersBuilder(
        AtataContextBuilder atataContextBuilder,
        List<LogConsumerConfiguration> items)
    {
        _atataContextBuilder = atataContextBuilder;
        _items = items;
    }

    /// <summary>
    /// Gets the list of log consumer configurations.
    /// </summary>
    public IReadOnlyList<LogConsumerConfiguration> Items =>
        _items;

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

        LogConsumerConfiguration consumerConfiguration = new(consumer);
        _items.Add(consumerConfiguration);

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

        var logConsumerConfiguration = Items.FirstOrDefault(x => x.Consumer == logConsumer);

        if (logConsumerConfiguration is not null)
            _items.Remove(logConsumerConfiguration);

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

        _items.Remove(logConsumerConfiguration);

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
        _items.RemoveAll(x => x.Consumer is TLogConsumer);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Clears all log consumer configurations.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Clear()
    {
        _items.Clear();
        return _atataContextBuilder;
    }

    private LogConsumerConfiguration GetConfigurationOrNull<TLogConsumer>()
        where TLogConsumer : ILogConsumer
        =>
        Items.LastOrDefault(x => x.Consumer is TLogConsumer);
}
