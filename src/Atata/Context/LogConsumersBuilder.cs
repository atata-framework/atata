#nullable enable

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
    /// Configures log consumer builder for existing <typeparamref name="TLogConsumer"/> log consumer or adds a new one.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <typeparamref name="TLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure<TLogConsumer>(
        Action<LogConsumerBuilder<TLogConsumer>>? configure = null)
       where TLogConsumer : ILogConsumer
    {
        var consumerConfiguration = Items.LastOrDefault(x => x.Consumer is TLogConsumer);

        if (consumerConfiguration is null)
        {
            var consumer = ActivatorEx.CreateInstance<TLogConsumer>();
            return Add(consumer);
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
}
