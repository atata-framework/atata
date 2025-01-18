#nullable enable

namespace Atata;

/// <summary>
/// An interface of log consumer that requires initialization.
/// </summary>
public interface IInitializableLogConsumer : ILogConsumer, ICloneable
{
    /// <summary>
    /// Initializes this instance.
    /// </summary>
    void Initialize();
}
