namespace Atata
{
    public interface IFieldVerificationProvider<TData, TField, TOwner> :
        IUIComponentVerificationProvider<TField, TOwner>,
        IDataVerificationProvider<TData, TOwner>
        where TField : Field<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
