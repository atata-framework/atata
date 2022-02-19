namespace Atata
{
    public interface IDataVerificationProvider<out TData, TOwner> : IVerificationProvider<TOwner>
    {
        /// <summary>
        /// Gets the object provider that is verified.
        /// </summary>
        IObjectProvider<TData, TOwner> DataProvider { get; }
    }
}
