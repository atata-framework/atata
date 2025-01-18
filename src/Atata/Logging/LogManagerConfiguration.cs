#nullable enable

namespace Atata;

internal sealed class LogManagerConfiguration
{
    internal LogManagerConfiguration(
        LogConsumerConfiguration[] consumerConfigurations,
        SecretStringToMask[] secretStringsToMask)
    {
        ConsumerConfigurations = consumerConfigurations;
        SecretStringsToMask = secretStringsToMask;
    }

    internal LogConsumerConfiguration[] ConsumerConfigurations { get; }

    internal SecretStringToMask[] SecretStringsToMask { get; }
}
