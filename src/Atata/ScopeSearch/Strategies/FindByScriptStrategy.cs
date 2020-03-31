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
            object scriptResult = AtataContext.Current.Driver.ExecuteScript(Script, scope);

            if (scriptResult is ReadOnlyCollection<IWebElement> elements)
            {
                return ProcessCollectionOfElements(elements, scope, options, searchOptions);
            }
            else if (scriptResult is IWebElement element)
            {
                return new SequalComponentScopeFindResult(element, SequalStrategy);
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

                    return new SequalComponentScopeFindResult(elements[options.Index.Value], SequalStrategy, sequalOptions);
                }
            }
            else
            {
                return new SequalComponentScopeFindResult(elements, SequalStrategy);
            }
        }
    }
}
