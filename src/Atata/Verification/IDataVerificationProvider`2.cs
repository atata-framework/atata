namespace Atata
{
    public interface IDataVerificationProvider<out TData, TOwner> : IVerificationProvider<TOwner>
    {
        /// <summary>
        /// Gets the data provider that is verified.
        /// </summary>
        IDataProvider<TData, TOwner> DataProvider { get; }
    }
}
