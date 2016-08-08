using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace Atata
{
    public class ControlList<T, TOwner> : UIComponentPart<TOwner>, IEnumerable<T>
        where T : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly ControlDefinitionAttribute itemDefinition;

        public ControlList()
        {
            itemDefinition = UIComponentResolver.GetControlDefinition(typeof(T));
        }

        public DataProvider<int, TOwner> Count => Component.GetOrCreateDataProvider($"{itemDefinition.ComponentTypeName} count", GetCount);

        public T this[int index]
        {
            get { return null; }
        }

        public T this[Expression<Func<T, bool>> predicateExpression]
        {
            get { return null; }
        }

        protected virtual int GetCount()
        {
            By itemBy = CreateItemBy();
            return Component.Scope.GetAll(itemBy).Count;
        }

        protected virtual By CreateItemBy()
        {
            return By.XPath($".//{itemDefinition.ScopeXPath}").OfKind(itemDefinition.ComponentTypeName);
        }

        protected virtual IScopeLocator CreateItemScopeLocator(By by)
        {
            return new DynamicScopeLocator(options => Component.Scope.Get(by.With(options)));
        }

        protected virtual T CreateItem(IScopeLocator scopeLocator, string name)
        {
            T item = Component.CreateControl<T>(name);
            item.ScopeLocator = scopeLocator;

            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
