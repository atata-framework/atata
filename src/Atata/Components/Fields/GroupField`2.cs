using OpenQA.Selenium;

namespace Atata
{
    public abstract class GroupField<T, TOwner> : EditableField<T, TOwner>, IItemsControl
        where TOwner : PageObject<TOwner>
    {
        protected GroupField()
        {
        }

        protected IItemElementFindStrategy ItemElementFindStrategy { get; private set; }

        void IItemsControl.Apply(IItemElementFindStrategy itemElementFindStrategy)
        {
            ItemElementFindStrategy = itemElementFindStrategy;
        }

        protected IWebElement GetItemElement(object parameter, bool isSafely = false, string xPathCondition = null)
        {
            string itemConditionXPath = ItemElementFindStrategy.GetXPathCondition(parameter);
            itemConditionXPath += xPathCondition;
            return ScopeLocator.GetElement(SearchOptions.Safely(isSafely), itemConditionXPath);
        }

        protected IWebElement[] GetItemElements()
        {
            return ScopeLocator.GetElements();
        }

        protected bool IsChecked(object parameter)
        {
            IWebElement element = GetItemElement(parameter);
            return element.Selected;
        }

        protected internal override string ConvertValueToString(T value)
        {
            return ItemElementFindStrategy.ConvertToString(value);
        }
    }
}
