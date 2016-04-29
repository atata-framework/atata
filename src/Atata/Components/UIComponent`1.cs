using System;

namespace Atata
{
    public abstract class UIComponent<TOwner> : UIComponent
        where TOwner : PageObject<TOwner>
    {
        protected UIComponent()
        {
        }

        protected internal new TOwner Owner
        {
            get { return (TOwner)base.Owner; }
            internal set { base.Owner = value; }
        }

        protected internal new UIComponent<TOwner> Parent
        {
            get { return (UIComponent<TOwner>)base.Parent; }
            internal set { base.Parent = value; }
        }

        protected internal virtual void InitComponent()
        {
            UIComponentResolver.Resolve<TOwner>(this);
        }

        public new TOwner VerifyExists()
        {
            base.VerifyExists();
            return Owner;
        }

        public new TOwner VerifyMissing()
        {
            base.VerifyMissing();
            return Owner;
        }

        public TOwner VerifyContent(string content, TermMatch match = TermMatch.Equals)
        {
            return this.VerifyContent(new[] { content }, match);
        }

        public new TOwner VerifyContent(string[] content, TermMatch match = TermMatch.Equals)
        {
            base.VerifyContent(content, match);
            return Owner;
        }

        public new TOwner VerifyContentContainsAll(params string[] content)
        {
            base.VerifyContentContainsAll(content);
            return Owner;
        }

        protected TComponent Create<TComponent>(string name, params Attribute[] attributes)
            where TComponent : UIComponent<TOwner>
        {
            return UIComponentResolver.CreateComponent<TComponent, TOwner>(this, name, attributes);
        }

        protected ClickableControl<TOwner> CreateClickable(string name, params Attribute[] attributes)
        {
            return Create<ClickableControl<TOwner>>(name, attributes);
        }

        protected ClickableControl<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<ClickableControl<TNavigateTo, TOwner>>(name, attributes);
        }

        protected ClickableControl<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            var control = Create<ClickableControl<TNavigateTo, TOwner>>(name, attributes);
            control.NavigationPageObjectCreator = navigationPageObjectCreator;
            return control;
        }

        protected LinkControl<TOwner> CreateLink(string name, params Attribute[] attributes)
        {
            return Create<LinkControl<TOwner>>(name, attributes);
        }

        protected LinkControl<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<LinkControl<TNavigateTo, TOwner>>(name, attributes);
        }

        protected LinkControl<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            var control = Create<LinkControl<TNavigateTo, TOwner>>(name, attributes);
            control.NavigationPageObjectCreator = navigationPageObjectCreator;
            return control;
        }

        protected ButtonControl<TOwner> CreateButton(string name, params Attribute[] attributes)
        {
            return Create<ButtonControl<TOwner>>(name, attributes);
        }

        protected ButtonControl<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return Create<ButtonControl<TNavigateTo, TOwner>>(name, attributes);
        }

        protected ButtonControl<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            var control = Create<ButtonControl<TNavigateTo, TOwner>>(name, attributes);
            control.NavigationPageObjectCreator = navigationPageObjectCreator;
            return control;
        }
    }
}
