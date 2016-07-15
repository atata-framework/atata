namespace Atata.KendoUI
{
    public class KendoGridFindByColumnHeaderStrategy : FindByColumnHeaderStrategy
    {
        public KendoGridFindByColumnHeaderStrategy()
            : base("(ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' k-grid ')])[1]//th")
        {
        }
    }
}
