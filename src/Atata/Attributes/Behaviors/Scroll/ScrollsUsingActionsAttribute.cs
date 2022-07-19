using System;

namespace Atata
{
    [Obsolete("Use " + nameof(ScrollsUsingMoveToElementActionAttribute) + " or " + nameof(ScrollsUsingScrollToElementActionAttribute) + " instead.")] // Obsolete since v2.1.0.
    public class ScrollsUsingActionsAttribute : ScrollsUsingMoveToElementActionAttribute
    {
    }
}
