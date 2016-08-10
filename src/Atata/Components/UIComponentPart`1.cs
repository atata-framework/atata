namespace Atata
{
    public class UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected internal UIComponent<TOwner> Component { get; internal set; }
    }
}
