using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the base class for the page objects.
    /// Also executes <see cref="TriggerEvents.Init"/> and <see cref="TriggerEvents.DeInit"/> triggers.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public abstract class PageObject<TOwner> : UIComponent<TOwner>, IPageObject<TOwner>, IPageObject
        where TOwner : PageObject<TOwner>
    {
        private Control<TOwner> activeControl;

        protected PageObject()
        {
            NavigateOnInit = true;
            ScopeLocator = new PlainScopeLocator(CreateScopeBy);

            ScopeSource = ScopeSource.PageObject;
            Owner = (TOwner)this;

            Report = new Report<TOwner>((TOwner)this, Log);
        }

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public new PageObjectVerificationProvider<TOwner> Should => new PageObjectVerificationProvider<TOwner>((TOwner)this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public new PageObjectVerificationProvider<TOwner> ExpectTo => Should.Using<ExpectationReportStrategy>();

        /// <summary>
        /// Gets a value indicating whether the navigation should be performed upon initialization.
        /// </summary>
        protected internal bool NavigateOnInit { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance is temporarily navigated using <see cref="GoTemporarilyAttribute"/> or other approach.
        /// </summary>
        protected bool IsTemporarilyNavigated { get; private set; }

        /// <summary>
        /// Gets the previous page object.
        /// </summary>
        protected UIComponent PreviousPageObject { get; private set; }

        /// <summary>
        /// Gets the instance that provides reporting functionality.
        /// </summary>
        public Report<TOwner> Report { get; private set; }

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the title of the current HTML page.
        /// </summary>
        public DataProvider<string, TOwner> PageTitle => GetOrCreateDataProvider("title", GetTitle);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the URL of the current HTML page.
        /// </summary>
        public DataProvider<string, TOwner> PageUrl => GetOrCreateDataProvider("URL", GetUrl);

        /// <summary>
        /// Gets the active control.
        /// </summary>
        public Control<TOwner> ActiveControl
        {
            get
            {
                return activeControl ?? (activeControl = Controls.Create<Control<TOwner>>(
                    "<Active>",
                    new DynamicScopeLocator(so => Driver.SwitchTo().ActiveElement())));
            }
        }

        /// <summary>
        /// Creates the <see cref="By"/> instance for Scope search.
        /// </summary>
        /// <returns>The <see cref="By"/> instance.</returns>
        protected virtual By CreateScopeBy()
        {
            string scopeXPath = Metadata.ComponentDefinitionAttribute?.ScopeXPath ?? "body";

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
            if (string.IsNullOrEmpty(ComponentName))
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

        /// <summary>
        /// Called upon initialization.
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// Called when initialization is completed.
        /// </summary>
        protected virtual void OnInitCompleted()
        {
        }

        protected virtual void OnVerify()
        {
        }

        protected virtual void Navigate()
        {
            if (GetType().TryGetCustomAttribute(out UrlAttribute attribute) || !AtataContext.Current.IsNavigated)
            {
                Go.ToUrl(attribute?.Url);
            }
        }

        TOther IPageObject.GoTo<TOther>(TOther pageObject, GoOptions options)
        {
            bool isReturnedFromTemporary = TryResolvePreviousPageObjectNavigatedTemporarily(ref pageObject);

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

        private bool TryResolvePreviousPageObjectNavigatedTemporarily<TOther>(ref TOther pageObject)
            where TOther : PageObject<TOther>
        {
            var tempPageObjectsEnumerable = AtataContext.Current.TemporarilyPreservedPageObjects.
                AsEnumerable().
                Reverse().
                OfType<TOther>();

            TOther pageObjectReferenceCopy = pageObject;

            TOther foundPageObject = pageObject == null
                ? tempPageObjectsEnumerable.FirstOrDefault(x => x.GetType() == typeof(TOther))
                : tempPageObjectsEnumerable.FirstOrDefault(x => x == pageObjectReferenceCopy);

            if (foundPageObject == null)
                return false;

            pageObject = foundPageObject;

            var tempPageObjectsToRemove = AtataContext.Current.TemporarilyPreservedPageObjects.
                SkipWhile(x => x != foundPageObject).
                ToArray();

            UIComponentResolver.CleanUpPageObjects(tempPageObjectsToRemove.Skip(1));
            foreach (var item in tempPageObjectsToRemove)
                AtataContext.Current.TemporarilyPreservedPageObjectList.Remove(item);

            return true;
        }

        /// <summary>
        /// Switches to the browser window using the window name.
        /// </summary>
        /// <param name="windowName">Name of the window.</param>
        protected virtual void SwitchTo(string windowName)
        {
            Driver.SwitchTo().Window(windowName);
        }

        /// <summary>
        /// Switches to frame page object using <see cref="By"/> instance that represents the selector for <c>&lt;iframe&gt;</c> tag element.
        /// </summary>
        /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
        /// <param name="frameBy">The frame <see cref="By"/> instance.</param>
        /// <param name="framePageObject">
        /// The frame page object.
        /// If equals <see langword="null"/>, creates an instance of <typeparamref name="TFramePageObject"/> using the default constructor.</param>
        /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
        /// <returns>The instance of the frame page object.</returns>
        public TFramePageObject SwitchToFrame<TFramePageObject>(By frameBy, TFramePageObject framePageObject = null, bool temporarily = false)
            where TFramePageObject : PageObject<TFramePageObject>
        {
            IWebElement frameElement = Scope.Get(frameBy);
            return SwitchToFrame(frameElement, framePageObject, temporarily);
        }

        /// <summary>
        /// Switches to frame page object using <see cref="IWebElement"/> instance that represents <c>&lt;iframe&gt;</c> tag element.
        /// </summary>
        /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
        /// <param name="frameElement">The frame element.</param>
        /// <param name="framePageObject">
        /// The frame page object.
        /// If equals <see langword="null"/>, creates an instance of <typeparamref name="TFramePageObject"/> using the default constructor.</param>
        /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
        /// <returns>The instance of the frame page object.</returns>
        public virtual TFramePageObject SwitchToFrame<TFramePageObject>(IWebElement frameElement, TFramePageObject framePageObject = null, bool temporarily = false)
            where TFramePageObject : PageObject<TFramePageObject>
        {
            Log.Trace("Switch to frame");
            Driver.SwitchTo().Frame(frameElement);
            return Go.To(framePageObject, navigate: false, temporarily: temporarily);
        }

        /// <summary>
        /// Switches to the root page using WebDriver's <c>SwitchTo().DefaultContent()</c> method.
        /// </summary>
        /// <typeparam name="TPageObject">The type of the root page object.</typeparam>
        /// <param name="rootPageObject">
        /// The root page object.
        /// If equals <see langword="null"/>, creates an instance of <typeparamref name="TPageObject"/> using the default constructor.</param>
        /// <returns>The instance of the root page object.</returns>
        public virtual TPageObject SwitchToRoot<TPageObject>(TPageObject rootPageObject = null)
            where TPageObject : PageObject<TPageObject>
        {
            Log.Trace("Switch to root page");
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
        /// Refreshes the current page until the condition specified by <paramref name="predicateExpression"/> argument is met.
        /// </summary>
        /// <param name="predicateExpression">The predicate expression to test the page.</param>
        /// <param name="timeout">The timeout in seconds.</param>
        /// <param name="retryInterval">The retry interval in seconds.</param>
        /// <returns>The instance of this page object.</returns>
        /// <example>
        /// <code>
        /// PageObject.RefreshPageUntil(x => x.SomeControl.IsVisible, timeout: 60, retryInterval: 5);
        /// </code>
        /// </example>
        public TOwner RefreshPageUntil(Expression<Func<TOwner, bool>> predicateExpression, double? timeout = null, double? retryInterval = null)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            TimeSpan timeoutTime = timeout.HasValue
                ? TimeSpan.FromSeconds(timeout.Value)
                : AtataContext.Current.WaitingTimeout;

            TimeSpan retryIntervalTime = retryInterval.HasValue
                ? TimeSpan.FromSeconds(retryInterval.Value)
                : AtataContext.Current.WaitingRetryInterval;

            TOwner activePageObject = (TOwner)this;

            string predicateMessage = ObjectExpressionStringBuilder.ExpressionToString(predicateExpression);

            string actionMessage = $"Refresh page until \"{predicateMessage}\" within {timeoutTime.ToShortIntervalString()} with {retryIntervalTime.ToShortIntervalString()} retry interval";
            AtataContext.Current.Log.Start(actionMessage);

            bool isOk = Driver.
                Try(timeoutTime, retryIntervalTime).
                Until(x =>
                {
                    activePageObject = activePageObject.RefreshPage();
                    return predicate(activePageObject);
                });

            if (!isOk)
                throw new TimeoutException($"Timed out after {timeoutTime.ToShortIntervalString()} waiting for: {actionMessage.ToLowerFirstLetter()}.");

            AtataContext.Current.Log.EndSection();

            return activePageObject;
        }

        /// <summary>
        /// Navigates back to the previous page.
        /// </summary>
        /// <typeparam name="TOther">The type of the page object that represents the previous page.</typeparam>
        /// <param name="previousPageObject">
        /// The instance of the previous page object.
        /// If equals <see langword="null"/>, creates an instance of <typeparamref name="TOther"/> using the default constructor.</param>
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
        /// <param name="nextPageObject">
        /// The instance of the next page object.
        /// If equals <see langword="null"/>, creates an instance of <typeparamref name="TOther"/> using the default constructor.</param>
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

        /// <summary>
        /// Executes the action passing specified owner's component.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="componentSelector">The component selector.</param>
        /// <param name="action">The action.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner Do<TComponent>(Func<TOwner, TComponent> componentSelector, Action<TComponent> action)
        {
            componentSelector.CheckNotNull(nameof(componentSelector));
            action.CheckNotNull(nameof(action));

            TComponent component = componentSelector((TOwner)this);

            action(component);

            return (TOwner)this;
        }

        /// <summary>
        /// Executes the navigation action passing specified owner's component.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
        /// <param name="componentSelector">The component selector.</param>
        /// <param name="navigationAction">The navigation action.</param>
        /// <returns>The instance of the page object to navigate to.</returns>
        public TNavigateTo Do<TComponent, TNavigateTo>(Func<TOwner, TComponent> componentSelector, Func<TComponent, TNavigateTo> navigationAction)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            componentSelector.CheckNotNull(nameof(componentSelector));
            navigationAction.CheckNotNull(nameof(navigationAction));

            TComponent component = componentSelector((TOwner)this);

            return navigationAction(component);
        }

        /// <summary>
        /// Executes the action passing current page object.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner Do(Action<TOwner> action)
        {
            action.CheckNotNull(nameof(action));

            action((TOwner)this);

            return (TOwner)this;
        }

        /// <summary>
        /// Executes the navigation action passing current page object.
        /// </summary>
        /// <typeparam name="TNavigateTo">The type of the navigate to.</typeparam>
        /// <param name="navigationAction">The navigation action.</param>
        /// <returns>The instance of the page object to navigate to.</returns>
        public TNavigateTo Do<TNavigateTo>(Func<TOwner, TNavigateTo> navigationAction)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            navigationAction.CheckNotNull(nameof(navigationAction));

            return navigationAction((TOwner)this);
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner Do(Action action)
        {
            action.CheckNotNull(nameof(action));

            action();

            return (TOwner)this;
        }

        /// <summary>
        /// Executes aggregate assertion for the current page object using <see cref="AtataContext.AggregateAssert(Action, string)" /> method.
        /// </summary>
        /// <param name="action">The action to execute in scope of aggregate assertion.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner AggregateAssert(Action<TOwner> action)
        {
            action.CheckNotNull(nameof(action));

            AtataContext.Current.AggregateAssert(() => action((TOwner)this), ComponentFullName);

            return (TOwner)this;
        }

        /// <summary>
        /// Executes aggregate assertion for the component of the current page object using <see cref="AtataContext.AggregateAssert(Action, string)" /> method.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="componentSelector">The component selector.</param>
        /// <param name="action">The action to execute in scope of aggregate assertion.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner AggregateAssert<TComponent>(Func<TOwner, TComponent> componentSelector, Action<TComponent> action)
        {
            componentSelector.CheckNotNull(nameof(componentSelector));
            action.CheckNotNull(nameof(action));

            TComponent component = componentSelector((TOwner)this);

            string componentName = UIComponentResolver.ResolveComponentFullName<TOwner>(component) ?? ComponentFullName;

            AtataContext.Current.AggregateAssert(() => action(component), componentName);

            return (TOwner)this;
        }

        /// <summary>
        /// Presses the specified keystrokes.
        /// </summary>
        /// <param name="keys">The keystrokes to send to the browser.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner Press(string keys)
        {
            if (!string.IsNullOrEmpty(keys))
            {
                Log.Start(new PressKeysLogSection(this, keys));

                Driver.Perform(x => x.SendKeys(keys));

                Log.EndSection();
            }

            return (TOwner)this;
        }

        /// <summary>
        /// Performs the specified set of actions.
        /// </summary>
        /// <param name="actionsBuilder">The actions builder.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner PerformActions(Func<Actions, Actions> actionsBuilder)
        {
            Driver.Perform(actionsBuilder);

            return (TOwner)this;
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
