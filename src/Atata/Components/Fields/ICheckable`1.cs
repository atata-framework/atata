namespace Atata
{
    public interface ICheckable<out TOwner> : IObjectProvider<bool, TOwner>
        where TOwner : PageObject<TOwner>
    {
        TOwner Check();
    }
}
