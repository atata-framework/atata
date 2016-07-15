namespace Atata.KendoUI
{
    [ControlDefinition("tr[parent::table or parent::tbody][not(@class) or @class!='k-grouping-row']", ComponentTypeName = "grid row")]
    [FindByColumnHeaderSettings(Strategy = typeof(KendoGridFindByColumnHeaderStrategy))]
    public class KendoGridRow<TOwner> : TableRowBase<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
