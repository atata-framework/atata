using OpenQA.Selenium;
using System;

namespace Atata
{
    public abstract class GroupField<TEnum, TOwner> : EditableField<TEnum, TOwner>, IItemsControl
        where TEnum : struct, IComparable, IFormattable
        where TOwner : PageObject<TOwner>
    {
        protected GroupField()
        {
        }

        IItemElementFindStrategy IItemsControl.ItemFindStrategy { get; set; }

        protected IWebElement GetItem(object parameter, bool isSafely = false, string extraXPath = null)
        {
            string itemConditionXPath = ((IItemsControl)this).ItemFindStrategy.Find(parameter);
            itemConditionXPath += extraXPath;
            return ScopeLocator.GetElement(SearchOptions.Safely(isSafely), itemConditionXPath);
        }

        protected bool IsChecked(object parameter)
        {
            IWebElement element = GetItem(parameter);
            return element.GetAttribute("checked") != null;
        }
    }
}
