using System.Collections.Generic;
using OpenQA.Selenium;

namespace Atata
{
    internal class UIComponentScopeCache
    {
        private readonly Dictionary<UIComponent, Dictionary<Visibility, IWebElement>> accessChainItems = new Dictionary<UIComponent, Dictionary<Visibility, IWebElement>>();

        public bool IsAccessChainActive { get; private set; }

        public bool TryGet(UIComponent component, Visibility visibility, out IWebElement scope)
        {
            scope = null;

            return accessChainItems.TryGetValue(component, out Dictionary<Visibility, IWebElement> visibiltyElementMap)
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
                Dictionary<Visibility, IWebElement> visibiltyElementMap;

                if (!accessChainItems.TryGetValue(component, out visibiltyElementMap))
                {
                    visibiltyElementMap = new Dictionary<Visibility, IWebElement>();
                    accessChainItems.Add(component, visibiltyElementMap);
                }

                visibiltyElementMap[visibility] = scope;
            }
        }

        public void ReleaseAccessChain()
        {
            accessChainItems.Clear();
            IsAccessChainActive = false;
        }

        public void Clear()
        {
            ReleaseAccessChain();
        }
    }
}
