namespace Atata
{
    public interface IDataProvider<out TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        UIComponent Component { get; }

        string ProviderName { get; }

        TOwner Owner { get; }

        TData Value { get; }

        // TODO: Extract ValueTermOptions to another interface.
        TermOptions ValueTermOptions { get; }

        // TODO: Remove Get method.
        TData Get();
    }
}
