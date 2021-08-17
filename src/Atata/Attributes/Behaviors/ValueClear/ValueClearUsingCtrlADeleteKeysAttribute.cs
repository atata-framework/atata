using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by performing "Ctrl+A, Delete" keyboard shortcut.
    /// </summary>
    [Obsolete("Use " + nameof(ClearsValueUsingCtrlADeleteKeysAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueClearUsingCtrlADeleteKeysAttribute : ClearsValueUsingCtrlADeleteKeysAttribute
    {
    }
}
