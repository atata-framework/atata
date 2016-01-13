using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;

namespace Atata
{
    public abstract class GroupField<TEnum, TOwner> : EditableField<TEnum, TOwner>, IItemsControl
        where TEnum : struct, IComparable, IFormattable
        where TOwner : PageObject<TOwner>
    {
        protected GroupField()
        {
        }

        IElementFindStrategy IItemsControl.ItemsFindStrategy { get; set; }
        ElementFindOptions IItemsControl.ItemsFindOptions { get; set; }
        IItemElementFindStrategy IItemsControl.ItemFindStrategy { get; set; }

        protected IWebElement GetItem(object parameter, bool isSafely = false)
        {
            ElementLocator locator = GetItemElementLocator(parameter, isSafely);
            return locator.GetElement(isSafely);
        }

        protected ReadOnlyCollection<IWebElement> GetItems()
        {
            ElementLocator locator = GetItemsElementLocator(true);
            return locator.GetElements();
        }

        protected bool IsChecked(object parameter)
        {
            ElementLocator locator = GetItemElementLocator(parameter, true);
            locator.XPath += "[@checked]";
            return locator.GetElement(true) != null;
        }

        protected ElementLocator GetItemElementLocator(object parameter, bool isSafely = false)
        {
            ElementLocator itemsLocator = GetItemsElementLocator(isSafely);

            if (itemsLocator == null && isSafely)
                return null;

            IItemsControl itemsControl = (IItemsControl)this;
            string itemXPath = itemsControl.ItemFindStrategy.Find(itemsLocator.XPath, parameter);
            return new ElementLocator(itemsLocator.Scope, itemXPath);
        }

        protected ElementLocator GetItemsElementLocator(bool isSafely = false)
        {
            IItemsControl itemsControl = (IItemsControl)this;

            ElementFindOptions findOptions = itemsControl.ItemsFindOptions.Clone();
            findOptions.IsSafely = isSafely;
            return itemsControl.ItemsFindStrategy.Find(Scope, findOptions);
        }
    }
}
