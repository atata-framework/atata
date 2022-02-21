using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the whole HTML page and is the main base class to inherit for the pages.
    /// Uses the <c>&lt;body&gt;</c> tag as a scope.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="PageObject{TOwner}" />
    [PageObjectDefinition("body", ComponentTypeName = "page", IgnoreNameEndings = "Page,PageObject")]
    public abstract class Page<TOwner> : PageObject<TOwner>
        where TOwner : Page<TOwner>
    {
        /// <summary>
        /// Gets the source of the scope.
        /// The default value is <see cref="ScopeSource.Page"/>.
        /// </summary>
        public override ScopeSource ScopeSource => ScopeSource.Page;

        protected override ISearchContext OnGetScopeContext(SearchOptions searchOptions)
        {
            return Driver;
        }
    }
}
