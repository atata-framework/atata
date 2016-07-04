namespace Atata
{
    public interface IDataVerificationProvider<TData, TOwner> : IVerificationProvider<TOwner>
        where TOwner : PageObject<TOwner>
    {
        IUIComponentValueProvider<TData, TOwner> DataProvider { get; }
    }
}
