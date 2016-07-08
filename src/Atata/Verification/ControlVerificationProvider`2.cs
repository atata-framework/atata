namespace Atata
{
    public class ControlVerificationProvider<TControl, TOwner> :
        ControlVerificationProvider<TControl, ControlVerificationProvider<TControl, TOwner>, TOwner>
        where TControl : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public ControlVerificationProvider(TControl control)
            : base(control)
        {
        }
    }
}
