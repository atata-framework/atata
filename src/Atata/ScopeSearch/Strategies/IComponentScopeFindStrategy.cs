#nullable enable

namespace Atata;

/// <summary>
/// Defines a strategy of UI component scope element finding.
/// </summary>
public interface IComponentScopeFindStrategy
{
    /// <summary>
    /// Finds the component scope.
    /// </summary>
    /// <param name="scope">The scope where to search in.</param>
    /// <param name="options">The component options to use for search.</param>
    /// <param name="searchOptions">The element search options.</param>
    /// <returns>
    /// Returns an instance of one of the following types:
    /// <list type="bullet">
    /// <item><see cref="XPathComponentScopeFindResult"/></item>
    /// <item><see cref="SubsequentComponentScopeFindResult"/></item>
    /// <item><see cref="MissingComponentScopeFindResult"/></item>
    /// </list>
    /// </returns>
    ComponentScopeFindResult Find(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions);
}
