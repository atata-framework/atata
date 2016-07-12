namespace Atata
{
    public interface IDataProvider<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        string ComponentFullName { get; }
        string ProviderName { get; }
        TOwner Owner { get; }
        TData Get();
        string ConvertValueToString(TData value);
    }
}
