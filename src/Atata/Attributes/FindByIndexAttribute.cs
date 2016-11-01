namespace Atata
{
    /// <summary>
    /// Specifies that a control should use the nth occurring element matching the control's definition.
    /// </summary>
    public class FindByIndexAttribute : FindAttribute
    {
        public FindByIndexAttribute(int index)
        {
            Index = index;
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByIndexStrategy();
        }
    }
}
