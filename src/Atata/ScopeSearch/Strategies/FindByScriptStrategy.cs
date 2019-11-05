using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Atata
{
    public class FindByScriptStrategy : IComponentScopeLocateStrategy
    {
        private static readonly IComponentScopeLocateStrategy SequalStrategy = new FindFirstDescendantOrSelfStrategy();

        public FindByScriptStrategy(string script)
        {
            Script = script;
        }

        /// <summary>
        /// Gets the script.
        /// </summary>
        public string Script { get; }

        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            object scriptResult = AtataContext.Current.Driver.ExecuteScript(Script, scope);

            if (scriptResult is ReadOnlyCollection<IWebElement> elements)
            {
                return ProcessCollectionOfElements(elements, scope, options, searchOptions);
            }
            else if (scriptResult is IWebElement element)
            {
                return new SequalComponentScopeLocateResult(element, SequalStrategy);
            }
            else if (scriptResult != null)
            {
                throw new InvalidOperationException($"Invalid script result. The script should return an element or collection of elements. But was returned: {scriptResult}");
            }
            else if (searchOptions.IsSafely)
            {
                return new MissingComponentScopeLocateResult();
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

        private ComponentScopeLocateResult ProcessCollectionOfElements(ReadOnlyCollection<IWebElement> elements, IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            if (options.Index.HasValue)
            {
                if (elements.Count <= options.Index.Value)
                {
                    if (searchOptions.IsSafely)
                    {
                        return new MissingComponentScopeLocateResult();
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

                    return new SequalComponentScopeLocateResult(elements[options.Index.Value], SequalStrategy, sequalOptions);
                }
            }
            else
            {
                return new SequalComponentScopeLocateResult(elements, SequalStrategy);
            }
        }
    }
}
