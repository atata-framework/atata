﻿namespace Atata;

public class FindByScriptStrategy : IComponentScopeFindStrategy
{
    private static readonly IComponentScopeFindStrategy s_sequalStrategy = new FindFirstDescendantOrSelfStrategy();

    public FindByScriptStrategy(string script) =>
        Script = script;

    /// <summary>
    /// Gets the script.
    /// </summary>
    public string Script { get; }

    public ComponentScopeFindResult Find(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
    {
        object? scriptResult = ExecuteScript(scope, options.Component.Session);

        if (scriptResult is ReadOnlyCollection<IWebElement> elements)
        {
            return ProcessCollectionOfElements(elements, scope, options, searchOptions);
        }
        else if (scriptResult is IWebElement element)
        {
            return new SubsequentComponentScopeFindResult(element, s_sequalStrategy);
        }
        else if (scriptResult is not null)
        {
            throw new InvalidOperationException($"Invalid script result. The script should return an element or collection of elements. But was returned: {scriptResult}");
        }
        else if (searchOptions.IsSafely)
        {
            return ComponentScopeFindResult.Missing;
        }
        else
        {
            throw ElementExceptionFactory.CreateForNotFound(
                new SearchFailureData
                {
                    ElementName = $"by script: {Script}",
                    SearchOptions = searchOptions,
                    SearchContext = scope
                });
        }
    }

    private object? ExecuteScript(ISearchContext scope, WebDriverSession session)
    {
        IJavaScriptExecutor scriptExecutor = session.Driver.AsScriptExecutor();

        if (scope is IWebElement element)
        {
            return scriptExecutor.ExecuteScriptWithLogging(session.Log, Script, element);
        }
        else if (Script.Contains("arguments"))
        {
            var scopeElement = scope.GetWithLogging(
                session.Log,
                By.XPath("*").With(SearchOptions.OfAnyVisibility()));
            return scriptExecutor.ExecuteScriptWithLogging(session.Log, Script, scopeElement);
        }
        else
        {
            return scriptExecutor.ExecuteScriptWithLogging(session.Log, Script);
        }
    }

    private ComponentScopeFindResult ProcessCollectionOfElements(ReadOnlyCollection<IWebElement> elements, ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
    {
        if (options.Index.HasValue)
        {
            if (elements.Count <= options.Index.Value)
            {
                if (searchOptions.IsSafely)
                {
                    return ComponentScopeFindResult.Missing;
                }
                else
                {
                    throw ElementExceptionFactory.CreateForNotFound(
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
                ComponentScopeFindOptions sequalOptions = options.Clone();
                sequalOptions.Index = null;

                return new SubsequentComponentScopeFindResult(elements[options.Index.Value], s_sequalStrategy, sequalOptions);
            }
        }
        else
        {
            return new SubsequentComponentScopeFindResult(elements, s_sequalStrategy);
        }
    }
}
