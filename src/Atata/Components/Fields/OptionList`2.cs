namespace Atata;

public abstract class OptionList<TValue, TOwner> : EditableField<TValue, TOwner>
    where TOwner : PageObject<TOwner>
{
    public FindItemAttribute FindItemAttribute =>
        Metadata.Get<FindItemAttribute>();

    protected IItemElementFindStrategy ItemElementFindStrategy =>
        FindItemAttribute.CreateStrategy(this, Metadata);

    protected IWebElement GetItemElement(object parameter, bool isSafely = false, string? xPathCondition = null)
    {
        ExecuteTriggers(TriggerEvents.BeforeAccess);

        string itemConditionXPath = ItemElementFindStrategy.GetXPathCondition(parameter, GetValueTermOptions());
        itemConditionXPath += xPathCondition;

        IWebElement? element = ScopeLocator.GetElement(SearchOptions.Safely(isSafely), itemConditionXPath);

        ExecuteTriggers(TriggerEvents.AfterAccess);

        return element!;
    }

    protected IWebElement[] GetItemElements()
    {
        IWebElement[] elements = ScopeLocator.GetElements();

        // TODO: Review to throw more detailed exception.
        if (elements is [])
            throw ElementNotFoundException.Create(ComponentFullName);

        return elements;
    }

    protected TValue GetElementValue(IWebElement element) =>
        ItemElementFindStrategy.GetParameter<TValue>(element, GetValueTermOptions());

    protected bool IsChecked(object parameter)
    {
        IWebElement element = GetItemElement(parameter);
        return element.Selected;
    }

    protected override TermOptions GetValueTermOptions()
    {
        var options = base.GetValueTermOptions();

        if (FindItemAttribute is IHasOptionalProperties optionalPropertiesHolder)
            options.MergeWith(optionalPropertiesHolder);

        return options;
    }
}
