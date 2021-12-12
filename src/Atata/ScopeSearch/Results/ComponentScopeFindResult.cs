namespace Atata
{
    /// <summary>
    /// Represents the result of UI component scope element finding.
    /// </summary>
    public abstract class ComponentScopeFindResult
    {
        /// <summary>
        /// Gets the missing result.
        /// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
        public static MissingComponentScopeFindResult Missing { get; } = new MissingComponentScopeFindResult();
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
