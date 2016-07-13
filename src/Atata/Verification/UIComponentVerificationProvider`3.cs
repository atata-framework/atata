namespace Atata
{
    public class UIComponentVerificationProvider<TComponent, TVerificationProvider, TOwner> :
        VerificationProvider<TVerificationProvider, TOwner>,
        IUIComponentVerificationProvider<TComponent, TOwner>
        where TComponent : UIComponent<TOwner>
        where TVerificationProvider : UIComponentVerificationProvider<TComponent, TVerificationProvider, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public UIComponentVerificationProvider(TComponent component)
        {
            Component = component;
        }

        protected internal TComponent Component { get; private set; }

        TComponent IUIComponentVerificationProvider<TComponent, TOwner>.Component => Component;

        protected override TOwner Owner
        {
            get { return Component.Owner; }
        }

        public NegationControlVerificationProvider Not => new NegationControlVerificationProvider(Component);

        public class NegationControlVerificationProvider :
            NegationVerificationProvider<TVerificationProvider, TOwner>,
            IUIComponentVerificationProvider<TComponent, TOwner>
        {
            public NegationControlVerificationProvider(TComponent component)
            {
                Component = component;
            }

            protected internal TComponent Component { get; private set; }

            TComponent IUIComponentVerificationProvider<TComponent, TOwner>.Component => Component;

            protected override TOwner Owner
            {
                get { return Component.Owner; }
            }
        }
    }
}
