namespace Atata;

/// <summary>
/// Contains optional properties bag.
/// </summary>
public interface IHasOptionalProperties
{
    /// <summary>
    /// Gets the optional properties bag.
    /// </summary>
    PropertyBag OptionalProperties { get; }
}
