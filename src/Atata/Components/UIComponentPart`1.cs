namespace Atata
{
    public class UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected internal IUIComponent<TOwner> Component { get; internal set; }
    }
}
