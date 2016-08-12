namespace Atata
{
    public interface IDataProvider<out TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        string ComponentFullName { get; }
        string ProviderName { get; }
        TOwner Owner { get; }
        TermOptions ValueTermOptions { get; }
        TData Get();
    }
}
