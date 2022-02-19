namespace Atata
{
    public interface ICheckable<TOwner> : IObjectProvider<bool, TOwner>
        where TOwner : PageObject<TOwner>
    {
        TOwner Check();
    }
}
