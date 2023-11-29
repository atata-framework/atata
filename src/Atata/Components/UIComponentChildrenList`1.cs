namespace Atata;

public class UIComponentChildrenList<TOwner> : List<UIComponent<TOwner>>
    where TOwner : PageObject<TOwner>
{
    private readonly UIComponent<TOwner> _component;

    public UIComponentChildrenList(UIComponent<TOwner> component) =>
        _component = component;

    public TControl Create<TControl>(string name, params Attribute[] attributes)
        where TControl : Control<TOwner>
    {
        var control = UIComponentResolver.CreateControl<TControl, TOwner>(_component, name, attributes);
        Add(control);

        return control;
    }

    public TControl Create<TControl>(string name, IScopeLocator scopeLocator, params Attribute[] attributes)
        where TControl : Control<TOwner>
    {
        name.CheckNotNullOrWhitespace(nameof(name));

        var control = Create<TControl>(name, attributes);
        control.ScopeLocator = scopeLocator;
        return control;
    }

    public TControl Resolve<TControl>(Func<IEnumerable<Attribute>> additionalAttributesFactory = null, [CallerMemberName] string propertyName = null)
        where TControl : Control<TOwner> =>
        Resolve<TControl>(propertyName, additionalAttributesFactory);

    public TControl Resolve<TControl>(string propertyName, Func<IEnumerable<Attribute>> additionalAttributesFactory = null)
        where TControl : Control<TOwner>
    {
        propertyName.CheckNotNullOrWhitespace(nameof(propertyName));

        TControl control = _component.Controls.OfType<TControl>().FirstOrDefault(x => x.Metadata.Name == propertyName);

        if (control != null)
            return control;

        var additionalAttributes = additionalAttributesFactory?.Invoke()?.ToArray();
        return UIComponentResolver.CreateControlForProperty<TControl, TOwner>(_component, propertyName, additionalAttributes);
    }

    public Clickable<TOwner> CreateClickable(string name, params Attribute[] attributes) =>
        Create<Clickable<TOwner>>(name, attributes);

    public Clickable<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, params Attribute[] attributes)
        where TNavigateTo : PageObject<TNavigateTo> =>
        Create<Clickable<TNavigateTo, TOwner>>(name, attributes);

    public Clickable<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
        where TNavigateTo : PageObject<TNavigateTo> =>
        Create<Clickable<TNavigateTo, TOwner>>(
            name,
            ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));

    public Link<TOwner> CreateLink(string name, params Attribute[] attributes) =>
        Create<Link<TOwner>>(name, attributes);

    public Link<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, params Attribute[] attributes)
        where TNavigateTo : PageObject<TNavigateTo> =>
        Create<Link<TNavigateTo, TOwner>>(name, attributes);

    public Link<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
        where TNavigateTo : PageObject<TNavigateTo> =>
        Create<Link<TNavigateTo, TOwner>>(
            name,
            ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));

    public Button<TOwner> CreateButton(string name, params Attribute[] attributes) =>
        Create<Button<TOwner>>(name, attributes);

    public Button<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, params Attribute[] attributes)
        where TNavigateTo : PageObject<TNavigateTo> =>
        Create<Button<TNavigateTo, TOwner>>(name, attributes);

    public Button<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
        where TNavigateTo : PageObject<TNavigateTo> =>
        Create<Button<TNavigateTo, TOwner>>(
            name,
            ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));

    private static Attribute[] ConcatWithNavigationPageObjectCreatorAttribute<TNavigateTo>(Attribute[] attributes, Func<TNavigateTo> navigationPageObjectCreator)
        where TNavigateTo : PageObject<TNavigateTo> =>
        [.. attributes, new NavigationPageObjectCreatorAttribute(navigationPageObjectCreator)];
}
