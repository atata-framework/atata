namespace Atata
{
    public class FindByCssAttribute : FindAttribute, IQualifierAttribute
    {
        public FindByCssAttribute(params string[] values)
        {
            Values = values;
        }

        public string[] Values { get; private set; }

        public string[] GetQualifiers(UIPropertyMetadata metadata)
        {
            return Values;
        }

        public override IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata)
        {
            return new FindByCssStrategy();
        }
    }
}
