namespace Atata;

/// <summary>
/// Provides NUnit extension methods for <see cref="LogConsumersBuilder"/>.
/// </summary>
public static class NUnitLogConsumersBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="NUnitTestContextLogConsumer"/> instance
    /// that uses <c>NUnit.Framework.TestContext</c> class for logging.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public static LogConsumerBuilder<NUnitTestContextLogConsumer> AddNUnitTestContext(
        this LogConsumersBuilder builder)
        =>
        builder.Add(new NUnitTestContextLogConsumer());
}
