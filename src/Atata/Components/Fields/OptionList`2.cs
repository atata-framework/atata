using OpenQA.Selenium;

namespace Atata
{
    public abstract class OptionList<T, TOwner> : EditableField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected OptionList()
        {
        }

        public FindItemAttribute FindItemAttribute =>
            Metadata.Get<FindItemAttribute>();

        protected IItemElementFindStrategy ItemElementFindStrategy =>
            FindItemAttribute.CreateStrategy(this, Metadata);

        protected IWebElement GetItemElement(object parameter, bool isSafely = false, string xPathCondition = null)
        {
            ExecuteTriggers(TriggerEvents.BeforeAccess);

            string itemConditionXPath = ItemElementFindStrategy.GetXPathCondition(parameter, ValueTermOptions);
            itemConditionXPath += xPathCondition;

            IWebElement element = ScopeLocator.GetElement(SearchOptions.Safely(isSafely), itemConditionXPath);

            ExecuteTriggers(TriggerEvents.AfterAccess);

            return element;
        }

        protected IWebElement[] GetItemElements()
        {
            IWebElement[] elements = ScopeLocator.GetElements();

            // TODO: Review to throw more detailed exception.
            if (elements.Length == 0)
                throw ExceptionFactory.CreateForNoSuchElement(ComponentFullName);

            return elements;
        }

        protected T GetElementValue(IWebElement element)
        {
            return ItemElementFindStrategy.GetParameter<T>(element, ValueTermOptions);
        }

        protected bool IsChecked(object parameter)
        {
            IWebElement element = GetItemElement(parameter);
            return element.Selected;
        }

        protected override void InitValueTermOptions(TermOptions termOptions, UIComponentMetadata metadata)
        {
            base.InitValueTermOptions(termOptions, metadata);

            if (FindItemAttribute is IPropertySettings findItemAttributeAsSettings)
                termOptions.MergeWith(findItemAttributeAsSettings);
        }
    }
}
