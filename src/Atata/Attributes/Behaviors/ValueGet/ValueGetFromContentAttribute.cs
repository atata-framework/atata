using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value getting from <see cref="IUIComponent{TOwner}.Content"/> property.
    /// </summary>
    [Obsolete("Use " + nameof(GetsValueFromContentAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueGetFromContentAttribute : GetsValueFromContentAttribute
    {
    }
}
