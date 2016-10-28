namespace Atata
{
    public class FindFirstAttribute : FindAttribute
    {
        public new int Index
        {
            get { return base.Index; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindFirstDescendantStrategy();
        }
    }
}
