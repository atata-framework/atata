namespace Atata
{
    [ControlDefinition("a", ComponentTypeName = "link", IgnoreNameEndings = "Button,Link")]
    [ControlFinding(FindTermBy.Content)]
    public class LinkControl<TNavigateTo, TOwner> : ClickableControl<TNavigateTo, TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
