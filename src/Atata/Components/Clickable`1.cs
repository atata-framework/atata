namespace Atata
{
    /// <summary>
    /// Represents any HTML element. Default search finds the first occurring element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(ComponentTypeName = "control", IgnoreNameEndings = "Button,Link")]
    public class Clickable<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
