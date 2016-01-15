namespace Atata
{
    public class FindItemByLabelAttribute : FindItemAttribute
    {
        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByLabelStrategy();
        }
    }
}
