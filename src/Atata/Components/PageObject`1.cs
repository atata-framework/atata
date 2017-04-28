using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the base class for the page objects.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public abstract class PageObject<TOwner> : UIComponent<TOwner>, IPageObject<TOwner>, IPageObject
        where TOwner : PageObject<TOwner>
    {
        protected PageObject()
        {
            NavigateOnInit = true;
            ScopeLocator = new PlainScopeLocator(CreateScopeBy);

            ScopeSource = ScopeSource.PageObject;
            Owner = (TOwner)this;
        }

        protected internal bool NavigateOnInit { get; internal set; }

        protected bool IsTemporarilyNavigated { get; private set; }

        protected UIComponent PreviousPageObject { get; private set; }

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the title of the current HTML page.
        /// </summary>
        public DataProvider<string, TOwner> PageTitle => GetOrCreateDataProvider("title", GetTitle);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the URL of the current HTML page.
        /// </summary>
        public DataProvider<string, TOwner> PageUrl => GetOrCreateDataProvider("URL", GetUrl);

        protected virtual By CreateScopeBy()
        {
            string scopeXPath = Metadata.ComponentDefinitonAttribute?.ScopeXPath ?? "body";

            return By.XPath($".//{scopeXPath}");
        }

        private string GetTitle()
        {
            return Driver.Title;
        }

        private string GetUrl()
        {
            return Driver.Url;
        }

        internal void Init()
        {
            ComponentName = UIComponentResolver.ResolvePageObjectName<TOwner>();
            ComponentTypeName = UIComponentResolver.ResolvePageObjectTypeName<TOwner>();

            Log.Info($"Go to {ComponentFullName}");

            InitComponent();

            if (NavigateOnInit)
                Navigate();

            OnInit();

            ExecuteTriggers(TriggerEvents.Init);

            OnInitCompleted();

            OnVerify();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnInitCompleted()
        {
        }

        protected virtual void OnVerify()
        {
        }

        protected virtual void Navigate()
        {
            UrlAttribute attribute;

            if (GetType().TryGetCustomAttribute(out attribute) || !AtataContext.Current.IsNavigated)
            {
                Go.ToUrl(attribute?.Url);
            }
        }

        TOther IPageObject.GoTo<TOther>(TOther pageObject, GoOptions options)
        {
            bool isReturnedFromTemporary = pageObject == null && TryResolvePreviousPageObjectNavigatedTemporarily(out pageObject);

            pageObject = pageObject ?? ActivatorEx.CreateInstance<TOther>();

            if (!isReturnedFromTemporary)
            {
                if (!options.Temporarily)
                {
                    AtataContext.Current.CleanUpTemporarilyPreservedPageObjectList();
                }

                pageObject.NavigateOnInit = options.Navigate;

                if (options.Temporarily)
                {
                    pageObject.IsTemporarilyNavigated = options.Temporarily;
                    AtataContext.Current.TemporarilyPreservedPageObjectList.Add(this);
                }
            }

            ExecuteTriggers(TriggerEvents.DeInit);

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
                pageObject.Init();
            }

            return pageObject;
        }

        private bool TryResolvePreviousPageObjectNavigatedTemporarily<TOther>(out TOther pageObject)
            where TOther : PageObject<TOther>
        {
            UIComponent foundPageObject = AtataContext.Current.TemporarilyPreservedPageObjects.
                AsEnumerable().
                Reverse().
                FirstOrDefault(x => x is TOther);

            pageObject = (TOther)foundPageObject;

            if (foundPageObject == null)
                return false;

            var tempPageObjectsToRemove = AtataContext.Current.TemporarilyPreservedPageObjects.
                SkipWhile(x => x != foundPageObject).
                ToArray();

            UIComponentResolver.CleanUpPageObjects(tempPageObjectsToRemove.Skip(1));
            foreach (var item in tempPageObjectsToRemove)
                AtataContext.Current.TemporarilyPreservedPageObjectList.Remove(item);

            return true;
        }

        protected virtual void SwitchTo(string windowName)
        {
            Driver.SwitchTo().Window(windowName);
        }

        /// <summary>
        /// Switches to frame page object using <see cref="By"/> instance.
        /// </summary>
        /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
        /// <param name="frameBy">The frame <see cref="By"/> instance.</param>
        /// <param name="framePageObject">The frame page object. If equals null, creates an instance of <typeparamref name="TFramePageObject"/> using the default constructor.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The instance of the frame page object.</returns>
        public TFramePageObject SwitchToFrame<TFramePageObject>(By frameBy, TFramePageObject framePageObject = null, bool temporarily = false)
            where TFramePageObject : PageObject<TFramePageObject>
        {
            IWebElement frameElement = Scope.Get(frameBy);
            return SwitchToFrame(frameElement, framePageObject, temporarily);
        }

        /// <summary>
        /// Switches to frame page object using <see cref="IWebElement"/> instance that represents &lt;iframe&gt; tag element.
        /// </summary>
        /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
        /// <param name="frameElement">The frame element.</param>
        /// <param name="framePageObject">The frame page object. If equals null, creates an instance of <typeparamref name="TFramePageObject"/> using the default constructor.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The instance of the frame page object.</returns>
        public virtual TFramePageObject SwitchToFrame<TFramePageObject>(IWebElement frameElement, TFramePageObject framePageObject = null, bool temporarily = false)
            where TFramePageObject : PageObject<TFramePageObject>
        {
            Log.Info("Switch to frame");
            Driver.SwitchTo().Frame(frameElement);
            return Go.To(framePageObject, navigate: false, temporarily: temporarily);
        }

        /// <summary>
        /// Switches to the root page using WebDriver's <code>SwitchTo().DefaultContent()</code> method.
        /// </summary>
        /// <typeparam name="TPageObject">The type of the root page object.</typeparam>
        /// <param name="rootPageObject">The root page object.</param>
        /// <returns>The instance of the root page object.</returns>
        public virtual TPageObject SwitchToRoot<TPageObject>(TPageObject rootPageObject = null)
            where TPageObject : PageObject<TPageObject>
        {
            Log.Info("Switch to root page");
            Driver.SwitchTo().DefaultContent();
            return Go.To(rootPageObject, navigate: false);
        }

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        /// <returns>The instance of this page object.</returns>
        public virtual TOwner RefreshPage()
        {
            Log.Info("Refresh page");
            Driver.Navigate().Refresh();
            return Go.To<TOwner>(navigate: false);
        }

        /// <summary>
        /// Navigates back to the previous page.
        /// </summary>
        /// <typeparam name="TOther">The type of the page object that represents the previous page.</typeparam>
        /// <param name="previousPageObject">The instance of the previous page object. If equals null, creates an instance of <typeparamref name="TOther"/> using the default constructor.</param>
        /// <returns>The instance of the previous page object.</returns>
        public virtual TOther GoBack<TOther>(TOther previousPageObject = null)
            where TOther : PageObject<TOther>
        {
            Log.Info("Go back");
            Driver.Navigate().Back();
            return Go.To(previousPageObject, navigate: false);
        }

        /// <summary>
        /// Navigates forward to the next page.
        /// </summary>
        /// <typeparam name="TOther">The type of the page object that represents the next page.</typeparam>
        /// <param name="nextPageObject">The instance of the next page object. If equals null, creates an instance of <typeparamref name="TOther"/> using the default constructor.</param>
        /// <returns>The instance of the next page object.</returns>
        public virtual TOther GoForward<TOther>(TOther nextPageObject = null)
            where TOther : PageObject<TOther>
        {
            Log.Info("Go forward");
            Driver.Navigate().Forward();
            return Go.To(nextPageObject, navigate: false);
        }

        /// <summary>
        /// Closes the current window.
        /// </summary>
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

        public TOwner Do<TComponent>(Func<TOwner, TComponent> childControlGetter, Action<TComponent> action)
        {
            childControlGetter.CheckNotNull(nameof(childControlGetter));
            action.CheckNotNull(nameof(action));

            TComponent component = childControlGetter((TOwner)this);

            action(component);

            return (TOwner)this;
        }

        public TNavigateTo Do<TComponent, TNavigateTo>(Func<TOwner, TComponent> childControlGetter, Func<TComponent, TNavigateTo> navigationAction)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            childControlGetter.CheckNotNull(nameof(childControlGetter));
            navigationAction.CheckNotNull(nameof(navigationAction));

            TComponent component = childControlGetter((TOwner)this);

            return navigationAction(component);
        }

        public TOwner Do(Action<TOwner> action)
        {
            action.CheckNotNull(nameof(action));

            action((TOwner)this);

            return (TOwner)this;
        }

        public TNavigateTo Do<TNavigateTo>(Func<TOwner, TNavigateTo> navigationAction)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            navigationAction.CheckNotNull(nameof(navigationAction));

            return navigationAction((TOwner)this);
        }

        /// <summary>
        /// Waits the specified time.
        /// </summary>
        /// <param name="time">The time to wait.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner Wait(TimeSpan time)
        {
            Thread.Sleep(time);

            return (TOwner)this;
        }

        /// <summary>
        /// Waits the specified time in seconds.
        /// </summary>
        /// <param name="seconds">The time to wait in seconds.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner Wait(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));

            return (TOwner)this;
        }
    }
}
