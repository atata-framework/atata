namespace Atata
{
    public interface ICheckable<TOwner> : IUIComponentDataProvider<bool, TOwner>
        where TOwner : PageObject<TOwner>
    {
        TOwner Check();
    }
}
