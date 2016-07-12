namespace Atata
{
    public interface ICheckable<TOwner> : IDataProvider<bool, TOwner>
        where TOwner : PageObject<TOwner>
    {
        TOwner Check();
    }
}
