#nullable enable

namespace Atata;

/// <summary>
/// Represents the action provider class that wraps <see cref="Action"/> and has no host.
/// Recommended for static methods.
/// </summary>
public class ActionProvider : ActionProvider<NoOwner>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionProvider"/> class.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public ActionProvider(
        IObjectSource<Action> objectSource,
        string providerName,
        IAtataExecutionUnit? executionUnit = null)
        : base(NoOwner.Instance, objectSource, providerName, executionUnit)
    {
    }
}
