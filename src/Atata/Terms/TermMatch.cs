namespace Atata
{
    /// <summary>
    /// Specifies the match approach for the term finding.
    /// </summary>
    public enum TermMatch
    {
        /// <summary>
        /// Uses the inherited match or the default one.
        /// </summary>
        Inherit,

        /// <summary>
        /// Checks whether the text equals the specified term.
        /// </summary>
        Equals,

        /// <summary>
        /// Checks whether the text contains the specified term.
        /// </summary>
        Contains,

        /// <summary>
        /// Checks whether the text starts with the specified term.
        /// </summary>
        StartsWith,

        /// <summary>
        /// Checks whether the text ends with the specified term.
        /// </summary>
        EndsWith
    }
}
