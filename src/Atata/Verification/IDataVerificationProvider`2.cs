namespace Atata
{
    public interface IDataVerificationProvider<TData, TOwner> : IVerificationProvider<TOwner>
        where TOwner : PageObject<TOwner>
    {
        IUIComponentDataProvider<TData, TOwner> DataProvider { get; }
    }
}
