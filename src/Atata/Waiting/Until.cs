namespace Atata;

/// <summary>
/// Specifies the waiting condition of the component/element.
/// </summary>
public enum Until
{
    /// <summary>
    /// Waits until the component/element will be missing.
    /// </summary>
    Missing,

    /// <summary>
    /// Waits until the component/element will be hidden.
    /// </summary>
    Hidden,

    /// <summary>
    /// Waits until the component/element will be missing or hidden.
    /// </summary>
    MissingOrHidden,

    /// <summary>
    /// Waits until the component/element will be visible.
    /// </summary>
    Visible,

    /// <summary>
    /// Waits until the component/element will be visible or hidden.
    /// </summary>
    VisibleOrHidden,

    /// <summary>
    /// Waits until the component/element will be visible and then until it will be hidden.
    /// </summary>
    VisibleThenHidden,

    /// <summary>
    /// Waits until the component/element will be visible and then until it will be missing.
    /// </summary>
    VisibleThenMissing,

    /// <summary>
    /// Waits until the component/element will be visible and then until it will be missing or hidden.
    /// </summary>
    VisibleThenMissingOrHidden,

    /// <summary>
    /// Waits until the component/element will be missing and then until it will be visible.
    /// </summary>
    MissingThenVisible,

    /// <summary>
    /// Waits until the component/element will be hidden and then until it will be visible.
    /// </summary>
    HiddenThenVisible,

    /// <summary>
    /// Waits until the component/element will be missing or hidden and then until it will be visible.
    /// </summary>
    MissingOrHiddenThenVisible
}
