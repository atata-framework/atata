#nullable enable

namespace Atata;

/// <summary>
/// Provides log4net extension methods for <see cref="LogConsumerBuilder{TLogConsumer}"/>.
/// </summary>
public static class Log4NetLogConsumerBuilderExtensions
{
    /// <summary>
    /// Specifies the name of the logger repository.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="repositoryName">The name of the logger repository.</param>
    /// <returns>The same builder instance.</returns>
    [Obsolete("Use functionality from Atata.NLog package instead.")] // Obsolete since v4.0.0.
    public static LogConsumerBuilder<Log4NetConsumer> WithRepositoryName(
        this LogConsumerBuilder<Log4NetConsumer> builder,
        string repositoryName)
    {
        builder.Consumer.RepositoryName = repositoryName;
        return builder;
    }

    /// <summary>
    /// Specifies the assembly to use to lookup the logger repository.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="repositoryAssembly">The name of the assembly to use to lookup the repository.</param>
    /// <returns>The same builder instance.</returns>
    [Obsolete("Use functionality from Atata.NLog package instead.")] // Obsolete since v4.0.0.
    public static LogConsumerBuilder<Log4NetConsumer> WithRepositoryAssembly(
        this LogConsumerBuilder<Log4NetConsumer> builder,
        Assembly repositoryAssembly)
    {
        builder.Consumer.RepositoryAssembly = repositoryAssembly;
        return builder;
    }
}
