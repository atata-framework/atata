using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace Atata
{
    public class ControlList<TItem, TOwner> : UIComponentPart<TOwner>, IDataProvider<IEnumerable<TItem>, TOwner>, IEnumerable<TItem>
        where TItem : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected ControlDefinitionAttribute ItemDefinition { get; } = UIComponentResolver.GetControlDefinition(typeof(TItem));

        /// <summary>
        /// Gets the verification provider that gives a set of verification extension methods.
        /// </summary>
        public DataVerificationProvider<IEnumerable<TItem>, TOwner> Should => new DataVerificationProvider<IEnumerable<TItem>, TOwner>(this);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the controls count.
        /// </summary>
        public DataProvider<int, TOwner> Count => Component.GetOrCreateDataProvider($"{ItemDefinition.ComponentTypeName} count", GetCount);

        UIComponent IDataProvider<IEnumerable<TItem>, TOwner>.Component => (UIComponent)Component;

        string IDataProvider<IEnumerable<TItem>, TOwner>.ProviderName => $"{ItemDefinition.ComponentTypeName} list";

        TOwner IDataProvider<IEnumerable<TItem>, TOwner>.Owner => Component.Owner;

        TermOptions IDataProvider<IEnumerable<TItem>, TOwner>.ValueTermOptions { get; }

        IEnumerable<TItem> IDataProvider<IEnumerable<TItem>, TOwner>.Value => GetAll();

        public TItem this[int index]
        {
            get
            {
                index.CheckIndexNonNegative();

                return GetItemByIndex(index);
            }
        }

        public TItem this[Expression<Func<TItem, bool>> predicateExpression]
        {
            get
            {
                predicateExpression.CheckNotNull(nameof(predicateExpression));

                string itemName = UIComponentResolver.ResolveControlName<TItem, TOwner>(predicateExpression);

                return GetItem(itemName, predicateExpression);
            }
        }

        protected virtual int GetCount()
        {
            By itemBy = CreateItemBy();
            return Component.Scope.GetAll(itemBy.AtOnce()).Count;
        }

        protected TItem GetItemByIndex(int index)
        {
            string itemName = OrdinalizeNumber(index + 1);

            return CreateItem(itemName, new FindByIndexAttribute(index));
        }

        protected TItem GetItemByInnerXPath(string itemName, string xPath)
        {
            return CreateItem(itemName, new FindByInnerXPathAttribute(xPath));
        }

        protected virtual TItem GetItem(string name, Expression<Func<TItem, bool>> predicateExpression)
        {
            By itemBy = CreateItemBy();
            var predicate = predicateExpression.Compile();

            ControlListScopeLocator scopeLocator = new ControlListScopeLocator(options =>
            {
                return Component.Scope.GetAll(itemBy.With(options).SafelyAtOnce()).
                    Where(element => predicate(CreateItem(new DefinedScopeLocator(element), name)));
            });

            return CreateItem(scopeLocator, name);
        }

        protected virtual By CreateItemBy()
        {
            return By.XPath($".//{ItemDefinition.ScopeXPath}").OfKind(ItemDefinition.ComponentTypeName);
        }

        protected virtual TItem CreateItem(string name, params Attribute[] attributes)
        {
            return Component.Controls.Create<TItem>(name, attributes);
        }

        protected TItem CreateItem(IScopeLocator scopeLocator, string name)
        {
            TItem item = CreateItem(name);
            item.ScopeLocator = scopeLocator;

            return item;
        }

        private string OrdinalizeNumber(int number)
        {
            string ending = "th";

            int tensDigit = (number % 100) / 10;

            if (tensDigit != 1)
            {
                int unitDigit = number % 10;

                ending = unitDigit == 1 ? "st"
                    : unitDigit == 2 ? "nd"
                    : unitDigit == 3 ? "rd"
                    : ending;
            }

            return $"{number}{ending}";
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TItem> GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        protected virtual IEnumerable<TItem> GetAll()
        {
            By itemBy = CreateItemBy();

            return Component.Scope.GetAll(itemBy.AtOnce()).
                Select((element, index) => CreateItem(new DefinedScopeLocator(element), OrdinalizeNumber(index + 1))).
                ToArray();
        }

        IEnumerable<TItem> IDataProvider<IEnumerable<TItem>, TOwner>.Get() => GetAll();
    }
}
