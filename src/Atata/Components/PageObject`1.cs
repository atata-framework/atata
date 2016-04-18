using OpenQA.Selenium;
using System;
using System.Threading;

namespace Atata
{
    public abstract class PageObject<T> : UIComponent<T>, IPageObject
        where T : PageObject<T>
    {
        protected PageObject()
        {
            NavigateOnInit = true;
            ScopeLocator = new DynamicScopeLocator(GetScope);
        }

        protected bool NavigateOnInit { get; private set; }
        protected bool IsTemporarilyNavigated { get; private set; }

        protected UIComponent PreviousPageObject { get; private set; }

        protected virtual IWebElement GetScope(SearchOptions searchOptions)
        {
            return Driver.Get(By.TagName("body").With(searchOptions));
        }

        internal void Init(PageObjectContext context)
        {
            ApplyContext(context);

            ComponentName = UIComponentResolver.ResolvePageObjectName<T>();
            ComponentTypeName = UIComponentResolver.ResolvePageObjectTypeName<T>();

            Log.StartSection("Go to {0}", ComponentFullName);

            OnInit();

            if (NavigateOnInit)
                Navigate();

            InitComponent();

            Log.EndSection();

            ExecuteTriggers(TriggerEvents.OnPageObjectInit);

            VerifyCurrentPage();
        }

        protected virtual void ApplyContext(PageObjectContext context)
        {
            Log = context.Logger;
            Driver = context.Driver;
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void Navigate()
        {
            NavigateToAttribute attribute;

            if (GetType().TryGetCustomAttribute(out attribute, true))
            {
                Go.ToUrl(attribute.Url);
            }
        }

        private void VerifyCurrentPage()
        {
            OnVerify();
        }

        protected virtual void OnVerify()
        {
        }

        protected void Wait(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));
        }

        TOther IPageObject.GoTo<TOther>(TOther pageObject, GoOptions options)
        {
            bool isReturnedFromTemporary = pageObject == null && IsTemporarilyNavigated && PreviousPageObject is TOther;

            pageObject = isReturnedFromTemporary ? (TOther)PreviousPageObject : pageObject ?? Activator.CreateInstance<TOther>();

            pageObject.NavigateOnInit = options.Navigate;
            pageObject.IsTemporarilyNavigated = options.Temporarily;

            ExecuteTriggers(TriggerEvents.OnPageObjectLeave);
            if (!pageObject.IsTemporarilyNavigated)
                UIComponentResolver.CleanUpPageObject(this);

            if (!string.IsNullOrWhiteSpace(options.Url))
                Go.ToUrl(options.Url);

            if (!string.IsNullOrWhiteSpace(options.WindowName))
                SwitchTo(options.WindowName);

            if (!isReturnedFromTemporary)
            {
                pageObject.PreviousPageObject = this;
                pageObject.Init(new PageObjectContext(Driver, Log));
            }

            return pageObject;
        }

        protected virtual void SwitchTo(string windowName)
        {
            Log.Info("Switch to window '{0}'", windowName);
            Driver.SwitchTo().Window(windowName);
        }

        public virtual void CloseWindow()
        {
            Driver.ExecuteScript("window.close();");
        }

        public T Do<TComponent>(Func<T, TComponent> childControlGetter, params Action<TComponent>[] actions)
        {
            TComponent component = childControlGetter((T)this);

            foreach (var action in actions)
                action(component);

            return (T)this;
        }
    }
}
