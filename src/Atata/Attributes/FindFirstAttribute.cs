namespace Atata
{
    /// <summary>
    /// Indicates that a control should use the first occurring element matching the control's definition.
    /// </summary>
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
