namespace Atata
{
    public class FindByColumnIndexAttribute : FindAttribute
    {
        public FindByColumnIndexAttribute(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }

        public int ColumnIndex { get; private set; }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByColumnIndexStrategy(ColumnIndex);
        }
    }
}
