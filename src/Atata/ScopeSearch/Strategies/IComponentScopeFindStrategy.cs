using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the strategy of component scope finding.
    /// </summary>
    public interface IComponentScopeFindStrategy
    {
        /// <summary>
        /// Finds the component scope.
        /// </summary>
        /// <param name="scope">The scope where to search in.</param>
        /// <param name="options">The options to use for search.</param>
        /// <param name="searchOptions">The search options.</param>
        /// <returns>
        /// Returns an instance of one of the following types:
        /// <list type="bullet">
        /// <item><see cref="XPathComponentScopeFindResult"/></item>
        /// <item><see cref="SubsequentComponentScopeFindResult"/></item>
        /// <item><see cref="MissingComponentScopeFindResult"/></item>
        /// </list>
        /// </returns>
        ComponentScopeLocateResult Find(ISearchContext scope, ComponentScopeLocateOptions options, SearchOptions searchOptions);
    }
}
