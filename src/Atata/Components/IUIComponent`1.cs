using System;
using OpenQA.Selenium;

namespace Atata
{
    public interface IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        IPageObject<TOwner> Owner { get; }
        IUIComponent<TOwner> Parent { get; }
        UIComponentContentDataProvider<TOwner> Content { get; }
        ScopeSource ScopeSource { get; }
        IWebElement Scope { get; }
        IScopeLocator ScopeLocator { get; }
        string ComponentName { get; }
        string ComponentTypeName { get; }

        bool Exists();
        bool Missing();

        TOwner VerifyExists();
        TOwner VerifyMissing();
        TControl CreateControl<TControl>(string name, params Attribute[] attributes)
            where TControl : Control<TOwner>;
    }
}
