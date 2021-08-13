using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Atata
{
    internal class UIComponentScopeCache
    {
        private readonly Dictionary<UIComponent, Dictionary<Visibility, IWebElement>> _accessChainItems =
            new Dictionary<UIComponent, Dictionary<Visibility, IWebElement>>();

        public bool IsAccessChainActive { get; private set; }

        public bool TryGet(UIComponent component, Visibility visibility, out IWebElement scope)
        {
            scope = null;

            return _accessChainItems.TryGetValue(component, out Dictionary<Visibility, IWebElement> visibiltyElementMap)
                && visibiltyElementMap.TryGetValue(visibility, out scope);
        }

        public bool AcquireActivationOfAccessChain()
        {
            if (IsAccessChainActive)
                return false;

            IsAccessChainActive = true;
            return true;
        }

        public void AddToAccessChain(UIComponent component, Visibility visibility, IWebElement scope)
        {
            if (IsAccessChainActive)
            {
                if (!_accessChainItems.TryGetValue(component, out Dictionary<Visibility, IWebElement> visibiltyElementMap))
                {
                    visibiltyElementMap = new Dictionary<Visibility, IWebElement>();
                    _accessChainItems.Add(component, visibiltyElementMap);
                }

                visibiltyElementMap[visibility] = scope;
            }
        }

        public void ReleaseAccessChain()
        {
            _accessChainItems.Clear();
            IsAccessChainActive = false;
        }

        public void Clear()
        {
            ReleaseAccessChain();
        }

        public void ExecuteWithin(Action action)
        {
            action.CheckNotNull(nameof(action));

            bool isActivatedAccessChainCache = AcquireActivationOfAccessChain();

            if (isActivatedAccessChainCache)
            {
                try
                {
                    action.Invoke();
                }
                finally
                {
                    ReleaseAccessChain();
                }
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
