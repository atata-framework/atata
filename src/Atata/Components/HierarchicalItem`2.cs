namespace Atata
{
    public class HierarchicalItem<TItem, TOwner> : HierarchicalControl<TItem, TOwner>
        where TItem : HierarchicalItem<TItem, TOwner>
        where TOwner : PageObject<TOwner>
    {
        [FindFirst(OuterXPath = "ancestor::")]
        public new TItem Parent { get; private set; }

        public DataProvider<bool, TOwner> HasParent => GetOrCreateDataProvider("has parent", OnHasParent);

        protected virtual bool OnHasParent()
        {
            return Parent.Exists(SearchOptions.SafelyAtOnce());
        }
    }
}
