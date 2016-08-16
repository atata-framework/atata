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

        public DataVerificationProvider<IEnumerable<TItem>, TOwner> Should => new DataVerificationProvider<IEnumerable<TItem>, TOwner>(this);

        public DataProvider<int, TOwner> Count => Component.GetOrCreateDataProvider($"{ItemDefinition.ComponentTypeName} count", GetCount);

        string IDataProvider<IEnumerable<TItem>, TOwner>.ComponentFullName => Component.ComponentFullName;

        string IDataProvider<IEnumerable<TItem>, TOwner>.ProviderName => $"{ItemDefinition.ComponentTypeName} list";

        TOwner IDataProvider<IEnumerable<TItem>, TOwner>.Owner => Component.Owner;

        TermOptions IDataProvider<IEnumerable<TItem>, TOwner>.ValueTermOptions { get; }

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
            return Component.Scope.GetAll(itemBy).Count;
        }

        private string BuildLogFindMessage(string name)
        {
            return $"Find \"{name}\" {ItemDefinition.ComponentTypeName} in {Component.ComponentFullName}";
        }

        protected TItem GetItemByIndex(int index)
        {
            string itemName = OrdinalizeNumber(index + 1);

            Component.Log.StartSection(BuildLogFindMessage(itemName));

            TItem item = CreateItem(itemName, new FindByIndexAttribute(index));

            Component.Log.EndSection();

            return item;
        }

        protected TItem GetItemByInnerXPath(string itemName, string xPath)
        {
            Component.Log.StartSection(BuildLogFindMessage(itemName));

            TItem item = CreateItem(itemName, new FindByInnerXPathAttribute(xPath));

            Component.Log.EndSection();

            return item;
        }

        protected virtual TItem GetItem(string name, Expression<Func<TItem, bool>> predicateExpression)
        {
            Component.Log.StartSection(BuildLogFindMessage(name));

            By itemBy = CreateItemBy();
            var predicate = predicateExpression.Compile();

            TItem item = Component.Scope.GetAll(itemBy).
                Select(element => CreateItem(new DefinedScopeLocator(element), name)).
                FirstOrDefault(predicate);

            if (item == null)
            {
                item = CreateItem(
                    new DynamicScopeLocator(options =>
                    {
                        if (options.IsSafely)
                            return null;
                        else
                            throw ExceptionFactory.CreateForNoSuchElement(name);
                    }),
                    name);
            }

            Component.Log.EndSection();

            return item;
        }

        protected virtual By CreateItemBy()
        {
            return By.XPath($".//{ItemDefinition.ScopeXPath}").OfKind(ItemDefinition.ComponentTypeName);
        }

        protected TItem CreateItem(string name, params Attribute[] attributes)
        {
            return Component.CreateControl<TItem>(name, attributes);
        }

        protected virtual TItem CreateItem(IScopeLocator scopeLocator, string name)
        {
            TItem item = Component.CreateControl<TItem>(name);
            item.ScopeLocator = scopeLocator;

            return item;
        }

        private string OrdinalizeNumber(int number)
        {
            string ending = "th";

            int tensDigit = number % 100 / 10;

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

            return Component.Scope.GetAll(itemBy).
                Select((element, index) => CreateItem(new DefinedScopeLocator(element), OrdinalizeNumber(index + 1))).
                ToArray();
        }

        IEnumerable<TItem> IDataProvider<IEnumerable<TItem>, TOwner>.Get() => GetAll();
    }
}
