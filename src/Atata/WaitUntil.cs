namespace Atata
{
    /// <summary>
    /// Specifies the wait approach.
    /// </summary>
    public enum WaitUntil
    {
        Missing,
        Hidden,
        MissingOrHidden,
        Visible,
        Exists,
        VisibleAndHidden,
        VisibleAndMissing,
        MissingAndVisible,
        HiddenAndVisible
    }
}
