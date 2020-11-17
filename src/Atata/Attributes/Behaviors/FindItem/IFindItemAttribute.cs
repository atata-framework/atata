namespace Atata
{
    // TODO: Remove IFindItemAttribute.
    public interface IFindItemAttribute
    {
        IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata);
    }
}
