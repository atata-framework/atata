namespace Atata.KendoUI
{
    public class KendoGrid<TOwner> : KendoGrid<TableHeader<TOwner>, KendoGridRow<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
