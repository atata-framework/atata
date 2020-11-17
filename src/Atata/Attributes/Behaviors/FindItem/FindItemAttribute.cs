namespace Atata
{
    public abstract class FindItemAttribute : MulticastAttribute, IFindItemAttribute
    {
        public abstract IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata);
    }
}
