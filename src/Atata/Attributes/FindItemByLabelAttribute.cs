namespace Atata
{
    public class FindItemByLabelAttribute : FindItemAttribute
    {
        public override IItemElementFindStrategy CreateStrategy(UIPropertyMetadata metadata)
        {
            return new FindItemByLabelStrategy();
        }
    }
}
