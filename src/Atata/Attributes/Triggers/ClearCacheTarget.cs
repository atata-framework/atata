namespace Atata;

/// <summary>
/// Specifies the target component whose cache should be cleared.
/// </summary>
public enum ClearCacheTarget
{
    /// <summary>
    /// Targets self component.
    /// </summary>
    Self,

    /// <summary>
    /// Targets parent component.
    /// </summary>
    Parent,

    /// <summary>
    /// Targets grandparent component.
    /// </summary>
    Grandparent,

    /// <summary>
    /// Targets great grandparent component.
    /// </summary>
    GreatGrandparent,

    /// <summary>
    /// Targets the page object.
    /// </summary>
    PageObject
}
