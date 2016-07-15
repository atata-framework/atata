namespace Atata.KendoUI
{
    [ControlDefinition("div", ContainingClass = "k-grid", ComponentTypeName = "grid", IgnoreNameEndings = "DataGrid,Grid,Table")]
    public class KendoGrid<TRow, TOwner> : Table<TRow, TOwner>
        where TRow : KendoGridRow<TOwner>, new()
        where TOwner : PageObject<TOwner>
    {
    }
}
