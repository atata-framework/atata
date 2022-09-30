using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents a strategy that finds a control in a cell that corresponds the column searched by the column header text.
    /// First finds the index of the column header and then finds the cell by this index.
    /// </summary>
    public class FindByColumnHeaderStrategy : IComponentScopeFindStrategy
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
        /// <param name="headerXPath">The XPath of the header element.</param>
        public FindByColumnHeaderStrategy(string headerXPath) =>
            HeaderXPath = headerXPath;

        /// <summary>
        /// Gets or sets the XPath of the header element.
        /// The default value is <c>"ancestor::table[1]//th"</c>.
        /// </summary>
        public string HeaderXPath { get; set; }

        public ComponentScopeFindResult Find(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
        {
            int? columnIndex = GetColumnIndex(scope, options, searchOptions);

            if (columnIndex == null)
            {
                if (searchOptions.IsSafely)
                {
                    return ComponentScopeFindResult.Missing;
                }
                else
                {
                    throw ExceptionFactory.CreateForNoSuchElement(
                        new SearchFailureData
                        {
                            ElementName = $"\"{options.GetTermsAsString()}\" column header",
                            SearchOptions = searchOptions,
                            SearchContext = scope
                        });
                }
            }

            IComponentScopeFindStrategy nextStrategy = CreateColumnIndexStrategy(columnIndex.Value);
            return new SubsequentComponentScopeFindResult(scope, nextStrategy);
        }

        /// <summary>
        /// Gets the index of the column.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="options">The component scope locate options.</param>
        /// <param name="searchOptions">The search options.</param>
        /// <returns>The index of the column or <see langword="null"/> if not found.</returns>
        protected virtual int? GetColumnIndex(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
        {
            var headerNamePredicate = options.Match.GetPredicate();

            if (HeaderXPath == DefaultHeaderXPath && options.Component.Parent?.Parent is ITable table)
            {
                return table.GetColumnHeaderTexts()
                    .Select((x, i) => (Text: x, Index: i))
                    .Where(x => options.Terms.Any(term => headerNamePredicate(x.Text, term)))
                    .Select(x => (int?)x.Index)
                    .FirstOrDefault();
            }
            else
            {
                var headers = scope.GetAllWithLogging(By.XPath(HeaderXPath).With(searchOptions).OfAnyVisibility());

                return headers
                    .Select((x, i) => new { x.Text, Index = i })
                    .Where(x => options.Terms.Any(term => headerNamePredicate(x.Text, term)))
                    .Select(x => (int?)x.Index)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Creates the strategy to find a component by the column index.
        /// By default creates an instance of <see cref="FindByColumnIndexStrategy"/>.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>An instance of <see cref="FindByColumnIndexStrategy"/>.</returns>
        protected virtual IComponentScopeFindStrategy CreateColumnIndexStrategy(int columnIndex) =>
            new FindByColumnIndexStrategy(columnIndex);
    }
}
