namespace Atata
{
    public class FindByIndexAttribute : FindAttribute
    {
        public FindByIndexAttribute()
        {
        }

        public FindByIndexAttribute(int index)
        {
            Index = index;
        }

        public override IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata)
        {
            return new SimpleElementFindStrategy();
        }
    }
}
