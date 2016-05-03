namespace Atata
{
    public interface IUIComponentValueProvider<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
        string ComponentFullName { get; }
        string ProviderName { get; }
        TOwner Owner { get; }
        TValue Get();
        string ConvertValueToString(TValue value);
    }
}
