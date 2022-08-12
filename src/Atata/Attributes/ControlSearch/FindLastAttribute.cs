using System;

namespace Atata
{
    /// <summary>
    /// Indicates that a control should use the last occurring element matching the control's definition.
    /// </summary>
    public class FindLastAttribute : FindAttribute
    {
        protected override Type DefaultStrategy => typeof(FindLastDescendantStrategy);

        public override string BuildComponentName(UIComponentMetadata metadata) => "Last";
    }
}
