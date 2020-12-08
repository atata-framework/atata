using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Atata
{
    public class FindByScriptStrategy : IComponentScopeFindStrategy
    {
        private static readonly IComponentScopeFindStrategy SequalStrategy = new FindFirstDescendantOrSelfStrategy();

        public FindByScriptStrategy(string script)
        {
            Script = script;
        }

        /// <summary>
        /// Gets the script.
        /// </summary>
        public string Script { get; }

        public ComponentScopeLocateResult Find(ISearchContext scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            object scriptResult = ExecuteScript(scope);

            if (scriptResult is ReadOnlyCollection<IWebElement> elements)
            {
                return ProcessCollectionOfElements(elements, scope, options, searchOptions);
            }
            else if (scriptResult is IWebElement element)
            {
                return new SubsequentComponentScopeFindResult(element, SequalStrategy);
            }
            else if (scriptResult != null)
            {
                throw new InvalidOperationException($"Invalid script result. The script should return an element or collection of elements. But was returned: {scriptResult}");
            }
            else if (searchOptions.IsSafely)
            {
                return new MissingComponentScopeFindResult();
            }
            else
            {
                throw ExceptionFactory.CreateForNoSuchElement(
                    new SearchFailureData
                    {
                        ElementName = $"by script: {Script}",
                        SearchOptions = searchOptions,
                        SearchContext = scope
                    });
            }
        }

        private object ExecuteScript(ISearchContext scope)
        {
            var driver = AtataContext.Current.Driver;

            if (scope is IWebElement element)
            {
                return driver.ExecuteScript(Script, element);
            }
            else if (Script.Contains("arguments"))
            {
                var scopeElement = scope.GetWithLogging(By.XPath("*").With(SearchOptions.OfAnyVisibility()));
                return driver.ExecuteScript(Script, scopeElement);
            }
            else
            {
                return driver.ExecuteScript(Script);
            }
        }

        private ComponentScopeLocateResult ProcessCollectionOfElements(ReadOnlyCollection<IWebElement> elements, ISearchContext scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            if (options.Index.HasValue)
            {
                if (elements.Count <= options.Index.Value)
                {
                    if (searchOptions.IsSafely)
                    {
                        return new MissingComponentScopeFindResult();
                    }
                    else
                    {
                        throw ExceptionFactory.CreateForNoSuchElement(
                            new SearchFailureData
                            {
                                ElementName = $"{(options.Index.Value + 1).Ordinalize()} by script: {Script}",
                                SearchOptions = searchOptions,
                                SearchContext = scope
                            });
                    }
                }
                else
                {
                    ComponentScopeLocateOptions sequalOptions = options.Clone();
                    sequalOptions.Index = null;

                    return new SubsequentComponentScopeFindResult(elements[options.Index.Value], SequalStrategy, sequalOptions);
                }
            }
            else
            {
                return new SubsequentComponentScopeFindResult(elements, SequalStrategy);
            }
        }
    }
}
