namespace Atata
{
    [ControlDefinition("table", IgnoreNameEndings = "Table")]
    public class Table<TRow, TOwner> : Control<TOwner>
        where TRow : TableRowBase<TOwner>, new()
        where TOwner : PageObject<TOwner>
    {
        public TableRowList<TRow, TOwner> Rows { get; private set; }

        public ControlList<TableHeader<TOwner>, TOwner> Headers { get; private set; }

        protected internal bool GoTemporarilyByClickOnRow { get; set; }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            var goTemporarilyAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<GoTemporarilyAttribute>();
            if (goTemporarilyAttribute != null)
                GoTemporarilyByClickOnRow = goTemporarilyAttribute.IsTemporarily;
        }
    }
}
