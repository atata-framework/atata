namespace Atata
{
    /// <summary>
    /// Spicifies whether to temporarily navigate to page object.
    /// </summary>
    public class GoTemporarilyAttribute : MulticastAttribute
    {
        public GoTemporarilyAttribute(bool isTemporarily = true) =>
            IsTemporarily = isTemporarily;

        public bool IsTemporarily { get; private set; }
    }
}
