namespace Atata;

public interface IUIComponentVerificationProvider<out TComponent, out TOwner> : IVerificationProvider<TOwner>
    where TComponent : IUIComponent<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the component that is verified.
    /// </summary>
    TComponent Component { get; }
}
