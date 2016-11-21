namespace Atata
{
    public interface IUIComponentVerificationProvider<TComponent, TOwner> : IVerificationProvider<TOwner>
        where TComponent : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        TComponent Component { get; }
    }
}
