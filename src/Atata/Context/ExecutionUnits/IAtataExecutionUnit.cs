namespace Atata;

/// <summary>
/// Represents an execution unit, the unit which can hold either
/// <see cref="AtataContext"/> or <see cref="AtataSession"/>.
/// </summary>
public interface IAtataExecutionUnit
{
    /// <summary>
    /// Gets the context.
    /// </summary>
    AtataContext Context { get; }

    /// <summary>
    /// Gets the log.
    /// </summary>
    ILogManager Log { get; }

    /// <summary>
    /// Gets a value indicating whether the unit is active.
    /// </summary>
    bool IsActive { get; }
}
