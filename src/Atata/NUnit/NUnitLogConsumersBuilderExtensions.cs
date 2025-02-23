#nullable enable

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
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <see cref="NUnitTestContextLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder AddNUnitTestContext(
        this LogConsumersBuilder builder,
        Action<LogConsumerBuilder<NUnitTestContextLogConsumer>>? configure = null)
        =>
        builder.Add(configure);
}
