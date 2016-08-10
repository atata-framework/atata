using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace Atata
{
    public class ControlList<TItem, TOwner> : UIComponentPart<TOwner>, IEnumerable<TItem>
        where TItem : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public ControlList()
        {
            ItemDefinition = UIComponentResolver.GetControlDefinition(typeof(TItem));
        }

        protected ControlDefinitionAttribute ItemDefinition { get; private set; }

        public DataProvider<int, TOwner> Count => Component.GetOrCreateDataProvider($"{ItemDefinition.ComponentTypeName} count", GetCount);

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
            get { return null; }
        }

        protected virtual TItem GetItem(string name, By by)
        {
            Component.Log.StartSection($"Find \"{name}\" {ItemDefinition.ComponentTypeName} in {Component.ComponentFullName}");

            IScopeLocator scopeLocator = CreateItemScopeLocator(by);
            TItem item = CreateItem(scopeLocator, name);

            Component.Log.EndSection();

            return item;
        }

        protected virtual int GetCount()
        {
            By itemBy = CreateItemBy();
            return Component.Scope.GetAll(itemBy).Count;
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

        private string OrdinalizeNumber(int number)
        {
            int mod100 = number % 100;

            string ending;

            if (mod100 >= 11 && mod100 <= 13)
            {
                ending = "th";
            }
            else
            {
                switch (number % 10)
                {
                    case 1:
                        ending = "st";
                        break;
                    case 2:
                        ending = "nd";
                        break;
                    case 3:
                        ending = "rd";
                        break;
                    default:
                        ending = "th";
                        break;
                }
            }
            return $"{number}{ending}";
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
