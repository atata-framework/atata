namespace Atata
{
    public interface ICheckable<TOwner> : IUIComponentValueProvider<bool, TOwner>
        where TOwner : PageObject<TOwner>
    {
        TOwner Check();
    }
}
