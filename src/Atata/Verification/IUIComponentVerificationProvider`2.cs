namespace Atata
{
    public interface IUIComponentVerificationProvider<TControl, TOwner> : IVerificationProvider<TOwner>
        where TControl : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        TControl Component { get; }
    }
}
