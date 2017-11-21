using OpenQA.Selenium;

namespace Atata
{
    public abstract class OptionList<T, TOwner> : EditableField<T, TOwner>, IItemsControl
        where TOwner : PageObject<TOwner>
    {
        protected OptionList()
        {
        }

        protected IItemElementFindStrategy ItemElementFindStrategy { get; private set; }

        void IItemsControl.Apply(IItemElementFindStrategy itemElementFindStrategy)
        {
            ItemElementFindStrategy = itemElementFindStrategy;
        }

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
            return ScopeLocator.GetElements();
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

            IPropertySettings findItemAttributeAsSettings = metadata.Get<IFindItemAttribute>(AttributeLevels.Declared) as IPropertySettings;
            if (findItemAttributeAsSettings != null)
                termOptions.MergeWith(findItemAttributeAsSettings);
        }
    }
}
