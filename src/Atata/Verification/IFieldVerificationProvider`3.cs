namespace Atata
{
    public interface IFieldVerificationProvider<out TValue, out TField, out TOwner> :
        IUIComponentVerificationProvider<TField, TOwner>,
        IObjectVerificationProvider<TValue, TOwner>
        where TField : Field<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
