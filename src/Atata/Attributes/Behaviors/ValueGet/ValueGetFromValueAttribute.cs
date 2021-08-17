using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value getting from <c>value</c> attribute.
    /// </summary>
    [Obsolete("Use " + nameof(GetsValueFromValueAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueGetFromValueAttribute : GetsValueFromValueAttribute
    {
    }
}
