namespace Atata
{
    public class FindItemByIndexAttribute : FindItemAttribute
    {
        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByIndexStrategy();
        }
    }
}
