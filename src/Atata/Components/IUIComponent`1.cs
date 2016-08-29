using System;
using OpenQA.Selenium;

namespace Atata
{
    public interface IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        TOwner Owner { get; }
        IUIComponent<TOwner> Parent { get; }
        DataProvider<string, TOwner> Content { get; }
        ScopeSource ScopeSource { get; }
        IWebElement Scope { get; }
        IScopeLocator ScopeLocator { get; }

        string ComponentName { get; }
        string ComponentTypeName { get; }
        string ComponentFullName { get; }

        UIComponentChildrenList<TOwner> Controls { get; }
        UIComponentMetadata Metadata { get; }

        UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should { get; }

        bool Exists(SearchOptions options = null);
        bool Missing(SearchOptions options = null);

        DataProvider<TValue, TOwner> GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction);
    }
}
