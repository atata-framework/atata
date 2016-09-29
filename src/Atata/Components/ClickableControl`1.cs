namespace Atata
{
    /// <summary>
    /// Represents any HTML element. By default is being searched by the id attribute.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*", ComponentTypeName = "control", IgnoreNameEndings = "Button,Link")]
    [ControlFinding(FindTermBy.Id)]
    public class ClickableControl<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
