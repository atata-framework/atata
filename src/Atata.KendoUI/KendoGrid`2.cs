namespace Atata.KendoUI
{
    public class KendoGrid<TRow, TOwner> : KendoGrid<TableHeader<TOwner>, TRow, TOwner>
        where TRow : KendoGridRow<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
