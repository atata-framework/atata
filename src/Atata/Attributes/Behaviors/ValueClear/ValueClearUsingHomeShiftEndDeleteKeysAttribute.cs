using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by performing "Home, Shift+End, Delete" keyboard shortcut.
    /// </summary>
    [Obsolete("Use " + nameof(ClearsValueUsingHomeShiftEndDeleteKeysAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueClearUsingHomeShiftEndDeleteKeysAttribute : ClearsValueUsingHomeShiftEndDeleteKeysAttribute
    {
    }
}
