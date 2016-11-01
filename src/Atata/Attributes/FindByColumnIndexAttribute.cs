namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found within the table column (&lt;td&gt;) that has the nth index.
    /// </summary>
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
