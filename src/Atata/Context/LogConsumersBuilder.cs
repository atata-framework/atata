﻿namespace Atata;

/// <summary>
/// Represents the builder of log consumers.
/// </summary>
public class LogConsumersBuilder
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
    /// Adds the log consumer.
    /// </summary>
    /// <typeparam name="TLogConsumer">
    /// The type of the log consumer.
    /// Should have default constructor.
    /// </typeparam>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<TLogConsumer> Add<TLogConsumer>()
        where TLogConsumer : ILogConsumer, new()
        =>
        Add(new TLogConsumer());

    /// <summary>
    /// Adds the log consumer by its type name or alias.
    /// Predefined aliases are defined in <see cref="LogConsumerAliases"/> static class.
    /// </summary>
    /// <param name="typeNameOrAlias">The type name or alias of the log consumer.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<ILogConsumer> Add(string typeNameOrAlias)
    {
        ILogConsumer consumer = LogConsumerAliases.Resolve(typeNameOrAlias);
        return Add(consumer);
    }

    /// <summary>
    /// Adds the log consumer.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="consumer">The log consumer.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<TLogConsumer> Add<TLogConsumer>(TLogConsumer consumer)
        where TLogConsumer : ILogConsumer
    {
        consumer.CheckNotNull(nameof(consumer));

        var consumerConfiguration = new LogConsumerConfiguration(consumer);
        _items.Add(consumerConfiguration);
        return new LogConsumerBuilder<TLogConsumer>(consumerConfiguration);
    }

    /// <summary>
    /// Returns a log consumer builder for existing <typeparamref name="TLogConsumer"/> log consumer or adds a new one.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<TLogConsumer> Configure<TLogConsumer>()
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
            return new LogConsumerBuilder<TLogConsumer>(consumerConfiguration);
        }
    }

    /// <summary>
    /// Adds the <see cref="TraceLogConsumer"/> instance that uses <see cref="Trace"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<TraceLogConsumer> AddTrace() =>
        Add(new TraceLogConsumer());

    /// <summary>
    /// Adds the <see cref="DebugLogConsumer"/> instance that uses <see cref="Debug"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<DebugLogConsumer> AddDebug() =>
        Add(new DebugLogConsumer());

    /// <summary>
    /// Adds the <see cref="ConsoleLogConsumer"/> instance that uses <see cref="Console"/> class for logging.
    /// </summary>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public LogConsumerBuilder<ConsoleLogConsumer> AddConsole() =>
        Add(new ConsoleLogConsumer());
}
