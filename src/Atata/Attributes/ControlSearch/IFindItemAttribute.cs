namespace Atata
{
    public interface IFindItemAttribute
    {
        IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata);
    }
}
