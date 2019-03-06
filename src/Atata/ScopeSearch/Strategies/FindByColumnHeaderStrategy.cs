using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents a strategy that finds a control in a cell that corresponds the column searched by the column header text.
    /// First finds the index of the column header and then finds the cell by this index.
    /// </summary>
    public class FindByColumnHeaderStrategy : IComponentScopeLocateStrategy
    {
        /// <summary>
        /// The default XPath of the header, which is <c>"ancestor::table[1]//th"</c>.
        /// </summary>
        public const string DefaultHeaderXPath = "ancestor::table[1]//th";

        /// <summary>
        /// Initializes a new instance of the <see cref="FindByColumnHeaderStrategy"/> class
        /// using <see cref="DefaultHeaderXPath"/>.
        /// </summary>
        public FindByColumnHeaderStrategy()
            : this(DefaultHeaderXPath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindByColumnHeaderStrategy"/> class
        /// using the specified <paramref name="headerXPath"/> argument value.
        /// </summary>
        /// <param name="headerXPath">The header x path.</param>
        public FindByColumnHeaderStrategy(string headerXPath)
        {
            HeaderXPath = headerXPath;
        }

        /// <summary>
        /// Gets the XPath of the header element.
        /// The default value is <c>"ancestor::table[1]//th"</c>.
        /// </summary>
        public string HeaderXPath { get; }

        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            var headers = scope.GetAll(By.XPath(HeaderXPath).With(searchOptions).OfAnyVisibility());
            var headerNamePredicate = options.Match.GetPredicate();

            int? columnIndex = headers.
                Select((x, i) => new { x.Text, Index = i }).
                Where(x => options.Terms.Any(term => headerNamePredicate(x.Text, term))).
                Select(x => (int?)x.Index).
                FirstOrDefault();

            if (columnIndex == null)
            {
                if (searchOptions.IsSafely)
                    return new MissingComponentScopeLocateResult();
                else
                    throw ExceptionFactory.CreateForNoSuchElement(options.GetTermsAsString(), searchContext: scope);
            }

            IComponentScopeLocateStrategy nextStrategy = CreateColumnIndexStrategy(columnIndex.Value);
            return new SequalComponentScopeLocateResult(scope, nextStrategy);
        }

        protected virtual IComponentScopeLocateStrategy CreateColumnIndexStrategy(int columnIndex)
        {
            return new FindByColumnIndexStrategy(columnIndex);
        }
    }
}
