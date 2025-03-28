#nullable enable

namespace Atata;

public abstract class UIComponentVerificationProvider<TComponent, TVerificationProvider, TOwner> :
    VerificationProvider<TVerificationProvider, TOwner>,
    IUIComponentVerificationProvider<TComponent, TOwner>
    where TComponent : UIComponent<TOwner>
    where TVerificationProvider : UIComponentVerificationProvider<TComponent, TVerificationProvider, TOwner>
    where TOwner : PageObject<TOwner>
{
    protected UIComponentVerificationProvider(TComponent component)
        : base(component.Session.ExecutionUnit)
        =>
        Component = component;

    protected internal TComponent Component { get; }

    TComponent IUIComponentVerificationProvider<TComponent, TOwner>.Component =>
        Component;

    protected override TOwner Owner =>
        Component.Owner;

    public abstract class NegationUIComponentVerificationProvider<TNegationUIComponentVerificationProvider> :
        NegationVerificationProvider<TNegationUIComponentVerificationProvider, TOwner>,
        IUIComponentVerificationProvider<TComponent, TOwner>
        where TNegationUIComponentVerificationProvider : NegationUIComponentVerificationProvider<TNegationUIComponentVerificationProvider>
    {
        protected NegationUIComponentVerificationProvider(TComponent component, IVerificationProvider<TOwner> verificationProvider)
            : base(verificationProvider) =>
            Component = component;

        protected internal TComponent Component { get; }

        TComponent IUIComponentVerificationProvider<TComponent, TOwner>.Component =>
            Component;

        protected override TOwner Owner =>
            Component.Owner;
    }
}
