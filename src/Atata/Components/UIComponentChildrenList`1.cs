using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public class UIComponentChildrenList<TOwner> : List<Control<TOwner>>
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
            return UIComponentResolver.CreateComponent<TControl, TOwner>(component, name, attributes);
        }

        public ClickableControl<TOwner> CreateClickable(string name, params Attribute[] attributes)
        {
            return Create<ClickableControl<TOwner>>(name, attributes);
        }

        public ClickableControl<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<ClickableControl<TNavigateTo, TOwner>>(name, attributes);
        }

        public ClickableControl<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<ClickableControl<TNavigateTo, TOwner>>(
                name,
                ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));
        }

        public LinkControl<TOwner> CreateLink(string name, params Attribute[] attributes)
        {
            return Create<LinkControl<TOwner>>(name, attributes);
        }

        public LinkControl<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<LinkControl<TNavigateTo, TOwner>>(name, attributes);
        }

        public LinkControl<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<LinkControl<TNavigateTo, TOwner>>(
                name,
                ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));
        }

        public ButtonControl<TOwner> CreateButton(string name, params Attribute[] attributes)
        {
            return Create<ButtonControl<TOwner>>(name, attributes);
        }

        public ButtonControl<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<ButtonControl<TNavigateTo, TOwner>>(name, attributes);
        }

        public ButtonControl<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<ButtonControl<TNavigateTo, TOwner>>(
                name,
                ConcatWithNavigationPageObjectCreatorAttribute(attributes, navigationPageObjectCreator));
        }

        private Attribute[] ConcatWithNavigationPageObjectCreatorAttribute<TNavigateTo>(Attribute[] attributes, Func<TNavigateTo> navigationPageObjectCreator)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return attributes.Concat(new[] { new NavigationPageObjectCreatorAttribute(navigationPageObjectCreator) }).ToArray();
        }
    }
}
