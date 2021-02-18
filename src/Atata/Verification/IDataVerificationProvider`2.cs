namespace Atata
{
    public interface IDataVerificationProvider<out TData, TOwner> : IVerificationProvider<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the data provider that is verified.
        /// </summary>
        IDataProvider<TData, TOwner> DataProvider { get; }
    }
}
