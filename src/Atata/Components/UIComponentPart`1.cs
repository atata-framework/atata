namespace Atata
{
    public class UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected UIComponent<TOwner> Component { get; private set; }
    }
}
