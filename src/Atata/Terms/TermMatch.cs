namespace Atata
{
    /// <summary>
    /// Specifies the match approach for the term finding.
    /// </summary>
    public enum TermMatch
    {
        /// <summary>
        /// Uses inherited match or none if missing.
        /// </summary>
        Inherit,
        Contains,
        Equals,
        StartsWith,
        EndsWith
    }
}
