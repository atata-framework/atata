using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public class UIComponentChildrenList<TOwner> : List<UIComponent<TOwner>>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;

        public UIComponentChildrenList(UIComponent<TOwner> component)
        {
            this.component = component;
        }

        public TControl Create<TControl>(string name, params Attribute[] attributes)
            where TControl : Control<TOwner>
        {
            return UIComponentResolver.CreateControl<TControl, TOwner>(component, name, attributes);
        }

        public TControl Create<TControl>(string name, IScopeLocator scopeLocator, params Attribute[] attributes)
            where TControl : Control<TOwner>
        {
            name.CheckNotNullOrWhitespace(nameof(name));

            var control = Create<TControl>(name, attributes);
            control.ScopeLocator = scopeLocator;
            return control;
        }

        public TControl Resolve<TControl>(string propertyName, Func<IEnumerable<Attribute>> additionalAttributesFactory = null)
            where TControl : Control<TOwner>
        {
            propertyName.CheckNotNullOrWhitespace(nameof(propertyName));

            TControl control = component.Controls.OfType<TControl>().FirstOrDefault(x => x.Metadata.Name == propertyName);

            if (control != null)
                return control;

            var additionalAttributes = additionalAttributesFactory?.Invoke()?.ToArray();
            return UIComponentResolver.CreateControlForProperty<TControl, TOwner>(component, propertyName, additionalAttributes);
        }

        public Clickable<TOwner> CreateClickable(string name, params Attribute[] attributes)
        {
            return Create<Clickable<TOwner>>(name, attributes);
        }

        public Clickable<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<Clickable<TNavigateTo, TOwner>>(name, attributes);
        }

        public Clickable<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<Clickable<TNavigateTo, TOwner>>(
                name,
                ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));
        }

        public Link<TOwner> CreateLink(string name, params Attribute[] attributes)
        {
            return Create<Link<TOwner>>(name, attributes);
        }

        public Link<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<Link<TNavigateTo, TOwner>>(name, attributes);
        }

        public Link<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<Link<TNavigateTo, TOwner>>(
                name,
                ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));
        }

        public Button<TOwner> CreateButton(string name, params Attribute[] attributes)
        {
            return Create<Button<TOwner>>(name, attributes);
        }

        public Button<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<Button<TNavigateTo, TOwner>>(name, attributes);
        }

        public Button<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<Button<TNavigateTo, TOwner>>(
                name,
                ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));
        }

        private static Attribute[] ConcatWithNavigationPageObjectCreatorAttribute<TNavigateTo>(Attribute[] attributes, Func<TNavigateTo> navigationPageObjectCreator)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return attributes.Concat(new[] { new NavigationPageObjectCreatorAttribute(navigationPageObjectCreator) }).ToArray();
        }
    }
}
