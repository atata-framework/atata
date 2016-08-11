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

        string IDataProvider<IEnumerable<TItem>, TOwner>.ProviderName => $"{ItemDefinition.ComponentTypeName} items";

        TOwner IDataProvider<IEnumerable<TItem>, TOwner>.Owner => Component.Owner;

        public TItem this[int index]
        {
            get
            {
                index.CheckIndexNonNegative();

                string itemName = OrdinalizeNumber(index + 1);
                By itemBy = CreateItemBy(index);

                return GetItem(itemName, itemBy);
            }
        }

        public TItem this[Expression<Func<TItem, bool>> predicateExpression]
        {
            get
            {
                predicateExpression.CheckNotNull(nameof(predicateExpression));

                string itemName = BuildItemName(predicateExpression);

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

        protected virtual TItem GetItem(string name, By by)
        {
            Component.Log.StartSection(BuildLogFindMessage(name));

            IScopeLocator scopeLocator = CreateItemScopeLocator(by);
            TItem item = CreateItem(scopeLocator, name);

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

        protected virtual By CreateItemBy(int index)
        {
            return By.XPath($"(.//{ItemDefinition.ScopeXPath})[{index + 1}]").OfKind(ItemDefinition.ComponentTypeName);
        }

        protected virtual IScopeLocator CreateItemScopeLocator(By by)
        {
            return new DynamicScopeLocator(options => Component.Scope.Get(by.With(options)));
        }

        protected virtual TItem CreateItem(IScopeLocator scopeLocator, string name)
        {
            TItem item = Component.CreateControl<TItem>(name);
            item.ScopeLocator = scopeLocator;

            return item;
        }

        private string BuildItemName(Expression<Func<TItem, bool>> predicateExpression)
        {
            string parameterName = predicateExpression.Parameters[0].Name;
            string itemName = predicateExpression.Body.ToString();
            if (itemName.StartsWith("(") && itemName.EndsWith(")"))
                itemName = itemName.Substring(1, itemName.Length - 2);

            itemName = itemName.Replace(parameterName + ".", string.Empty);
            return itemName;
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
                Select((element, index) => CreateItem(new DefinedScopeLocator(element), OrdinalizeNumber(index + 1)));
        }

        IEnumerable<TItem> IDataProvider<IEnumerable<TItem>, TOwner>.Get() => GetAll();

        string IDataProvider<IEnumerable<TItem>, TOwner>.ConvertValueToString(IEnumerable<TItem> value)
        {
            return TermResolver.ToString(value);
        }
    }
}
