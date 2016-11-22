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

        public NegationUIComponentVerificationProvider Not => new NegationUIComponentVerificationProvider(Component, this);

        public class NegationUIComponentVerificationProvider : NegationUIComponentVerificationProvider<NegationUIComponentVerificationProvider>
        {
            internal NegationUIComponentVerificationProvider(TComponent component, IVerificationProvider<TOwner> verificationProvider)
                : base(component, verificationProvider)
            {
            }
        }
    }
}
