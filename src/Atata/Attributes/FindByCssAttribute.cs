namespace Atata
{
    public class FindByCssAttribute : FindAttribute, IQualifierAttribute
    {
        public FindByCssAttribute(params string[] values)
        {
            Values = values;
        }

        public string[] Values { get; private set; }

        public string[] GetQualifiers(UIComponentMetadata metadata)
        {
            return Values;
        }

        public override IElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByCssStrategy();
        }
    }
}
