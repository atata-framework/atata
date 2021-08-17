using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by performing "Shift+Home, Delete" keyboard shortcut.
    /// Note that "End" key is not pressed in the beginning of the shortcut, as the caret on element by default goes to the end.
    /// </summary>
    [Obsolete("Use " + nameof(ClearsValueUsingShiftHomeDeleteKeysAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueClearUsingShiftHomeDeleteKeysAttribute : ClearsValueUsingShiftHomeDeleteKeysAttribute
    {
    }
}
