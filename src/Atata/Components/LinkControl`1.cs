namespace Atata
{
    [ControlDefinition("a", ComponentTypeName = "link", IgnoreNameEndings = "Button,Link")]
    [ControlFinding(FindTermBy.Content)]
    public class LinkControl<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
