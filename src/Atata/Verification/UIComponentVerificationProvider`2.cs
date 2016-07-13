namespace Atata
{
    public class UIComponentVerificationProvider<TComponent, TOwner> :
        UIComponentVerificationProvider<TComponent, UIComponentVerificationProvider<TComponent, TOwner>, TOwner>
        where TComponent : UIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public UIComponentVerificationProvider(TComponent component)
            : base(component)
        {
        }
    }
}
