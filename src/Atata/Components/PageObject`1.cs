using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public abstract class PageObject<T> : UIComponent<T>, IPageObject<T>, IPageObject
        where T : PageObject<T>
    {
        protected PageObject()
        {
            NavigateOnInit = true;
            ScopeLocator = new DynamicScopeLocator(GetScope);

            Owner = (T)this;

            PageTitle = CreateDataProvider(GetTitle, "title");
            PageUrl = CreateDataProvider(GetUrl, "URL");
        }

        protected internal bool NavigateOnInit { get; internal set; }
        protected bool IsTemporarilyNavigated { get; private set; }

        protected UIComponent PreviousPageObject { get; private set; }

        public UIComponentDataProvider<string, T> PageTitle { get; private set; }
        public UIComponentDataProvider<string, T> PageUrl { get; private set; }

        private string GetTitle()
        {
            return Driver.Title;
        }

        private string GetUrl()
        {
            return Driver.Url;
        }

        protected virtual IWebElement GetScope(SearchOptions searchOptions)
        {
            return Driver.Get(By.TagName("body").With(searchOptions));
        }

        internal void Init(PageObjectContext context)
        {
            ApplyContext(context);

            ComponentName = UIComponentResolver.ResolvePageObjectName<T>();
            ComponentTypeName = UIComponentResolver.ResolvePageObjectTypeName<T>();

            Log.Info("Go to {0}", ComponentFullName);

            OnInit();

            if (NavigateOnInit)
                Navigate();

            InitComponent();

            ExecuteTriggers(TriggerEvents.OnPageObjectInit);

            OnVerify();
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

        protected virtual void OnVerify()
        {
        }

        TOther IPageObject.GoTo<TOther>(TOther pageObject, GoOptions options)
        {
            bool isReturnedFromTemporary = pageObject == null && TryResolvePreviousPageObjectNavigatedTemporarily(out pageObject);

            pageObject = pageObject ?? ActivatorEx.CreateInstance<TOther>();

            if (!isReturnedFromTemporary)
            {
                if (!options.Temporarily)
                {
                    ATContext.Current.CleanUpTemporarilyPreservedPageObjectList();
                }

                pageObject.NavigateOnInit = options.Navigate;

                if (options.Temporarily)
                {
                    pageObject.IsTemporarilyNavigated = options.Temporarily;
                    ATContext.Current.TemporarilyPreservedPageObjectList.Add(this);
                }
            }

            ExecuteTriggers(TriggerEvents.OnPageObjectLeave);

            // TODO: Review that condition.
            if (!options.Temporarily)
                UIComponentResolver.CleanUpPageObject(this);

            if (!string.IsNullOrWhiteSpace(options.Url))
                Go.ToUrl(options.Url);

            if (!string.IsNullOrWhiteSpace(options.WindowName))
                SwitchTo(options.WindowName);

            if (isReturnedFromTemporary)
            {
                Log.Info("Go to {0}", pageObject.ComponentFullName);
            }
            else
            {
                pageObject.PreviousPageObject = this;
                pageObject.Init(new PageObjectContext(Driver, Log));
            }

            return pageObject;
        }

        private bool TryResolvePreviousPageObjectNavigatedTemporarily<TOther>(out TOther pageObject)
            where TOther : PageObject<TOther>
        {
            UIComponent foundPageObject = ATContext.Current.TemporarilyPreservedPageObjects.
                AsEnumerable().
                Reverse().
                FirstOrDefault(x => x is TOther);

            pageObject = (TOther)foundPageObject;

            if (foundPageObject == null)
                return false;

            var tempPageObjectsToRemove = ATContext.Current.TemporarilyPreservedPageObjects.
                SkipWhile(x => x != foundPageObject).
                ToArray();

            UIComponentResolver.CleanUpPageObjects(tempPageObjectsToRemove.Skip(1));
            foreach (var item in tempPageObjectsToRemove)
                ATContext.Current.TemporarilyPreservedPageObjectList.Remove(item);

            return true;
        }

        protected virtual void SwitchTo(string windowName)
        {
            Driver.SwitchTo().Window(windowName);
        }

        public virtual T RefreshPage()
        {
            Log.Info("Refresh page");
            Driver.Navigate().Refresh();
            return Go.To<T>(navigate: false);
        }

        public virtual TOther GoBack<TOther>(TOther previousPageObject = null)
            where TOther : PageObject<TOther>
        {
            Log.Info("Go back");
            Driver.Navigate().Back();
            return Go.To(previousPageObject, navigate: false);
        }

        public virtual TOther GoForward<TOther>(TOther nextPageObject = null)
            where TOther : PageObject<TOther>
        {
            Log.Info("Go forward");
            Driver.Navigate().Forward();
            return Go.To(nextPageObject, navigate: false);
        }

        public virtual void CloseWindow()
        {
            string nextWindowHandle = ResolveWindowHandleToSwitchAfterClose();

            Log.Info("Close window");
            Driver.Close();

            if (nextWindowHandle != null)
                SwitchTo(nextWindowHandle);
        }

        private string ResolveWindowHandleToSwitchAfterClose()
        {
            string currentWindowHandle = Driver.CurrentWindowHandle;
            int indexOfCurrent = Driver.WindowHandles.IndexOf(currentWindowHandle);
            if (indexOfCurrent > 0)
                return Driver.WindowHandles[indexOfCurrent - 1];
            else if (Driver.WindowHandles.Count > 1)
                return Driver.WindowHandles[1];
            else
                return null;
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
