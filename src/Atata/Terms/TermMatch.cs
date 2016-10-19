namespace Atata
{
    /// <summary>
    /// Specifies the match approach for the term finding.
    /// </summary>
    public enum TermMatch
    {
        /// <summary>
        /// Uses the inherited match or none if missing.
        /// </summary>
        Inherit,
        Contains,
        Equals,
        StartsWith,
        EndsWith
    }
}
