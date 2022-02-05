using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Atata
{
    public class UIComponentAccessChainScopeCache
    {
        private readonly Dictionary<UIComponent, Dictionary<Visibility, IWebElement>> _accessChainItems =
            new Dictionary<UIComponent, Dictionary<Visibility, IWebElement>>();

        internal bool IsActive { get; private set; }

        internal bool TryGet(UIComponent component, Visibility visibility, out IWebElement scope)
        {
            scope = null;

            return _accessChainItems.TryGetValue(component, out Dictionary<Visibility, IWebElement> visibiltyElementMap)
                && visibiltyElementMap.TryGetValue(visibility, out scope);
        }

        internal bool AcquireActivation()
        {
            if (IsActive)
                return false;

            IsActive = true;
            return true;
        }

        internal void Add(UIComponent component, Visibility visibility, IWebElement scope)
        {
            if (IsActive)
            {
                if (!_accessChainItems.TryGetValue(component, out Dictionary<Visibility, IWebElement> visibiltyElementMap))
                {
                    visibiltyElementMap = new Dictionary<Visibility, IWebElement>();
                    _accessChainItems.Add(component, visibiltyElementMap);
                }

                visibiltyElementMap[visibility] = scope;
            }
        }

        internal void Release()
        {
            Clear();
            IsActive = false;
        }

        public void Clear()
        {
            _accessChainItems.Clear();
        }

        public void ExecuteWithin(Action action)
        {
            action.CheckNotNull(nameof(action));

            bool isActivatedAccessChainCache = AcquireActivation();

            if (isActivatedAccessChainCache)
            {
                try
                {
                    action.Invoke();
                }
                finally
                {
                    Release();
                }
            }
            else
            {
                action.Invoke();
            }
        }

        public TResult ExecuteWithin<TResult>(Func<TResult> function)
        {
            function.CheckNotNull(nameof(function));

            bool isActivatedAccessChainCache = AcquireActivation();

            if (isActivatedAccessChainCache)
            {
                try
                {
                    return function.Invoke();
                }
                finally
                {
                    Release();
                }
            }
            else
            {
                return function.Invoke();
            }
        }
    }
}
