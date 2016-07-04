namespace Atata
{
    public interface IControlVerificationProvider<TControl, TOwner> : IVerificationProvider<TOwner>
        where TControl : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        TControl Control { get; }
    }
}
