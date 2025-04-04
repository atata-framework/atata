namespace Atata;

/// <summary>
/// Represents a base class for events associated with <see cref="PageObject{TOwner}"/>.
/// </summary>
public abstract class PageObjectEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageObjectEvent"/> class.
    /// </summary>
    /// <param name="pageObject">The page object.</param>
    protected PageObjectEvent(UIComponent pageObject) =>
        PageObject = pageObject;

    /// <summary>
    /// Gets the page object.
    /// </summary>
    public UIComponent PageObject { get; }
}
