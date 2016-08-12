namespace Atata
{
    public interface IDataVerificationProvider<out TData, TOwner> : IVerificationProvider<TOwner>
        where TOwner : PageObject<TOwner>
    {
        IDataProvider<TData, TOwner> DataProvider { get; }
    }
}
