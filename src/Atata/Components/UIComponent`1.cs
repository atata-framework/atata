using OpenQA.Selenium;

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

        public TOwner VerifyExists()
        {
            Log.StartVerificationSection("{0} component exists", ComponentName);
            GetScopeElement();
            Log.EndSection();
            return Owner;
        }

        public TOwner VerifyMissing()
        {
            Log.StartVerificationSection("{0} component missing", ComponentName);
            IWebElement element = GetScopeElement(isSafely: true);
            Asserter.That(element == null, "Found {0} component that should be missing", ComponentName);
            Log.EndSection();
            return Owner;
        }
    }
}
