namespace Atata
{
    /// <summary>
    /// Specifies the source of the scope.
    /// </summary>
    public enum ScopeSource
    {
        /// <summary>
        /// Uses the parent's scope.
        /// </summary>
        Parent,

        /// <summary>
        /// Uses the grandparent's (the parent of the parent) scope.
        /// </summary>
        Grandparent,

        /// <summary>
        /// Uses the owner page object's scope.
        /// </summary>
        PageObject,

        /// <summary>
        /// Uses the page's scope (&lt;body&gt; element).
        /// </summary>
        Page
    }
}
