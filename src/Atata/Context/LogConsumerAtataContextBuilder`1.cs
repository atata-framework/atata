namespace Atata
{
    public class LogConsumerAtataContextBuilder<TLogConsumer> : LogConsumersAtataContextBuilder, IHasContext<TLogConsumer>
        where TLogConsumer : ILogConsumer
    {
        private readonly LogConsumerConfiguration _logConsumerConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogConsumerAtataContextBuilder{TLogConsumer}" /> class.
        /// </summary>
        /// <param name="logConsumer">The log consumer.</param>
        /// <param name="logConsumerConfiguration">The log consumer configuration.</param>
        /// <param name="buildingContext">The building context.</param>
        public LogConsumerAtataContextBuilder(TLogConsumer logConsumer, LogConsumerConfiguration logConsumerConfiguration, AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            Context = logConsumer;
            _logConsumerConfiguration = logConsumerConfiguration;
        }

        /// <inheritdoc/>
        public TLogConsumer Context { get; }

        /// <summary>
        /// Defines that the logging should not use section-like pair messages (not <c>"Starting: {action}"</c> and <c>"Finished: {action}"</c>, but just <c>"{action}"</c>).
        /// </summary>
        /// <returns>The same builder instance.</returns>
        public LogConsumerAtataContextBuilder<TLogConsumer> WithoutSectionFinish()
        {
            _logConsumerConfiguration.LogSectionFinish = false;
            return this;
        }

        /// <summary>
        /// Specifies the minimum level of the log event to write to the log.
        /// The default value is <see cref="LogLevel.Trace"/>.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <returns>The same builder instance.</returns>
        public LogConsumerAtataContextBuilder<TLogConsumer> WithMinLevel(LogLevel level)
        {
            _logConsumerConfiguration.MinLevel = level;
            return this;
        }

        /// <summary>
        /// Specifies the nesting level indent of the log message.
        /// The default value is <c>"- "</c>.
        /// </summary>
        /// <param name="messageNestingLevelIndent">The message nesting level indent.</param>
        /// <returns>The same builder instance.</returns>
        public LogConsumerAtataContextBuilder<TLogConsumer> WithMessageNestingLevelIndent(string messageNestingLevelIndent)
        {
            _logConsumerConfiguration.MessageNestingLevelIndent = messageNestingLevelIndent;
            return this;
        }

        /// <summary>
        /// Specifies the start section prefix of the log message.
        /// The default value is <c>"&gt; "</c>.
        /// </summary>
        /// <param name="messageStartSectionPrefix">The message start section prefix.</param>
        /// <returns>The same builder instance.</returns>
        public LogConsumerAtataContextBuilder<TLogConsumer> WithMessageStartSectionPrefix(string messageStartSectionPrefix)
        {
            _logConsumerConfiguration.MessageStartSectionPrefix = messageStartSectionPrefix;
            return this;
        }

        /// <summary>
        /// Specifies the end section prefix of the log message.
        /// The default value is <c>"&lt; "</c>.
        /// </summary>
        /// <param name="messageEndSectionPrefix">The message end section prefix.</param>
        /// <returns>The same builder instance.</returns>
        public LogConsumerAtataContextBuilder<TLogConsumer> WithMessageEndSectionPrefix(string messageEndSectionPrefix)
        {
            _logConsumerConfiguration.MessageEndSectionPrefix = messageEndSectionPrefix;
            return this;
        }
    }
}
