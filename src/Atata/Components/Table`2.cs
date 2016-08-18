namespace Atata
{
    [ControlDefinition("table", IgnoreNameEndings = "Table")]
    public class Table<TRow, TOwner> : Control<TOwner>
        where TRow : TableRowBase<TOwner>, new()
        where TOwner : PageObject<TOwner>
    {
        public TableRowList<TRow, TOwner> Rows { get; private set; }

        public ControlList<TableHeader<TOwner>, TOwner> Headers { get; private set; }

        protected int? ColumnIndexToClickOnRow { get; set; }
        protected internal bool GoTemporarilyByClickOnRow { get; set; }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            if (ColumnIndexToClickOnRow == null)
            {
                var columnIndexToClickAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<ColumnIndexToClickAttribute>();
                if (columnIndexToClickAttribute != null)
                    ColumnIndexToClickOnRow = columnIndexToClickAttribute.Index;
            }

            var goTemporarilyAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<GoTemporarilyAttribute>();
            if (goTemporarilyAttribute != null)
                GoTemporarilyByClickOnRow = goTemporarilyAttribute.IsTemporarily;
        }
    }
}
