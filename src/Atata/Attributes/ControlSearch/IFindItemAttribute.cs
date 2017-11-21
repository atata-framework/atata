namespace Atata
{
    public interface IFindItemAttribute
    {
        IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata);
    }
}
