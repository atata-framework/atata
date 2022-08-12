using System;

namespace Atata
{
    /// <summary>
    /// Indicates that a control should use the first occurring element matching the control's definition.
    /// </summary>
    public class FindFirstAttribute : FindAttribute
    {
        public new int Index => base.Index;

        protected override Type DefaultStrategy => typeof(FindFirstDescendantStrategy);

        public override string BuildComponentName(UIComponentMetadata metadata) =>
            1.Ordinalize();
    }
}
