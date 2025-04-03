#nullable enable

namespace Atata;

internal sealed class ScopeLimitedLogConsumerConfiguration
{
    internal ScopeLimitedLogConsumerConfiguration(LogConsumerConfiguration consumerConfiguration) =>
        ConsumerConfiguration = consumerConfiguration;

    internal LogConsumerConfiguration ConsumerConfiguration { get; }

    internal AtataContextScopes Scopes { get; set; } = AtataContextScopes.All;

    internal ScopeLimitedLogConsumerConfiguration Clone() =>
        new(ConsumerConfiguration.Clone())
        {
            Scopes = Scopes
        };
}
