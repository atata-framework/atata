#nullable enable

namespace Atata;

/// <summary>
/// Indicates that component log messages should be written with <see cref="LogLevel.Trace"/>
/// log level instead of <see cref="LogLevel.Info"/>.
/// Attribute is useful for sub-controls of complex controls to keep informational log
/// cleaner by tracing sub-control informational log messages.
/// </summary>
public sealed class TraceLogAttribute : MulticastAttribute
{
}
