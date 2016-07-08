namespace Atata
{
    public class ControlVerificationProvider<TControl, TVerificationProvider, TOwner> :
        VerificationProvider<TVerificationProvider, TOwner>,
        IControlVerificationProvider<TControl, TOwner>
        where TControl : Control<TOwner>
        where TVerificationProvider : ControlVerificationProvider<TControl, TVerificationProvider, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public ControlVerificationProvider(TControl control)
        {
            Control = control;
        }

        protected TControl Control { get; private set; }

        TControl IControlVerificationProvider<TControl, TOwner>.Control => Control;

        protected override TOwner Owner
        {
            get { return Control.Owner; }
        }

        public NegationControlVerificationProvider Not => new NegationControlVerificationProvider(Control);

        public class NegationControlVerificationProvider :
            NegationVerificationProvider<TVerificationProvider, TOwner>,
            IControlVerificationProvider<TControl, TOwner>
        {
            public NegationControlVerificationProvider(TControl control)
            {
                Control = control;
            }

            protected TControl Control { get; private set; }

            TControl IControlVerificationProvider<TControl, TOwner>.Control => Control;

            protected override TOwner Owner
            {
                get { return Control.Owner; }
            }
        }
    }
}
