using System;
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
        private readonly AtataContext _context;

        private Control<TOwner> _activeControl;

        protected PageObject()
        {
            _context = AtataContext.Current
                ?? throw new InvalidOperationException(
                    $"Cannot instantiate {GetType().Name} because {nameof(AtataContext)}.{nameof(AtataContext.Current)} is null.");

            ScopeLocator = new PlainScopeLocator(CreateScopeBy);

            Owner = (TOwner)this;

            Report = new Report<TOwner>((TOwner)this, Log);

            PageUri = new UriProvider<TOwner>(this, GetUri, "URI");

            UIComponentResolver.InitPageObject<TOwner>(this);

            ComponentName = UIComponentResolver.ResolvePageObjectName<TOwner>();
            ComponentTypeName = UIComponentResolver.ResolvePageObjectTypeName<TOwner>();

            NavigationUrl = Metadata.Get<UrlAttribute>()?.Value;
        }

        /// <inheritdoc/>
        public sealed override AtataContext Context => _context;

        /// <summary>
        /// Gets the source of the scope.
        /// The default value is <see cref="ScopeSource.PageObject"/>.
        /// </summary>
        public override ScopeSource ScopeSource
            => ScopeSource.PageObject;

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public new PageObjectVerificationProvider<TOwner> Should
            => new PageObjectVerificationProvider<TOwner>((TOwner)this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public new PageObjectVerificationProvider<TOwner> ExpectTo
            => Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public new PageObjectVerificationProvider<TOwner> WaitTo
            => Should.Using<WaitingVerificationStrategy>();

        /// <summary>
        /// Gets a value indicating whether the navigation should be performed upon initialization.
        /// </summary>
        // TODO: Atata v3. Remove NavigateOnInit property.
        [Obsolete("Don't use NavigateOnInit property, as it will be removed.")] // Obsolete since v2.6.0.
        protected internal bool NavigateOnInit { get; internal set; } = true;

        /// <summary>
        /// Gets or sets the navigation URL, which can be used during page object initialization.
        /// </summary>
        protected internal string NavigationUrl { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is temporarily navigated using <see cref="GoTemporarilyAttribute"/> or other approach.
        /// </summary>
        protected internal bool IsTemporarilyNavigated { get; internal set; }

        /// <summary>
        /// Gets the previous page object.
        /// </summary>
        protected internal UIComponent PreviousPageObject { get; internal set; }

        /// <summary>
        /// Gets the instance that provides reporting functionality.
        /// </summary>
        public Report<TOwner> Report { get; }

        /// <summary>
        /// Gets the title provider of the current HTML page.
        /// </summary>
        public ValueProvider<string, TOwner> PageTitle =>
            CreateValueProvider("title", GetTitle);

        /// <summary>
        /// Gets the URL provider of the current HTML page.
        /// </summary>
        public ValueProvider<string, TOwner> PageUrl =>
            CreateValueProvider("URL", GetUrl);

        /// <summary>
        /// Gets the URI provider of the current HTML page.
        /// </summary>
        public UriProvider<TOwner> PageUri { get; }

        /// <summary>
        /// Gets the page source provider of the current HTML page.
        /// </summary>
        public ValueProvider<string, TOwner> PageSource =>
            CreateValueProvider("page source", GetPageSource);

        /// <summary>
        /// Gets the active control.
        /// </summary>
        public Control<TOwner> ActiveControl =>
            _activeControl ?? (_activeControl = Controls.Create<Control<TOwner>>(
                "<Active>",
                new DynamicScopeLocator(_ => Driver.SwitchTo().ActiveElement())));

        /// <summary>
        /// Creates the <see cref="By"/> instance for Scope search.
        /// </summary>
        /// <returns>The <see cref="By"/> instance.</returns>
        protected virtual By CreateScopeBy()
        {
            string scopeXPath = Metadata.ComponentDefinitionAttribute?.ScopeXPath ?? "body";

            return By.XPath($".//{scopeXPath}").Visible();
        }

        private string GetTitle() =>
            Driver.Title;

        private string GetUrl() =>
            Driver.Url;

        private Uri GetUri()
        {
            string url = GetUrl();
            return new Uri(url);
        }

        private string GetPageSource() =>
            Driver.PageSource;

        internal void Init()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            InitComponent();

            if (NavigateOnInit)
                Navigate();
#pragma warning restore CS0618 // Type or member is obsolete

            OnInit();
            Context.EventBus.Publish(new PageObjectInitEvent(this));
        }

        internal void CompleteInit()
        {
            ExecuteTriggers(TriggerEvents.Init);

            OnInitCompleted();
            Context.EventBus.Publish(new PageObjectInitCompletedEvent(this));

            OnVerify();
        }

        protected virtual void OnVerify()
        {
        }

        // TODO: Atata v3. Remove Navigate method.
        [Obsolete(
            "Don't override/use Navigate method, as it will be removed. " +
            "Instead, specify a navigation URL in a page object constructor by setting a URL value into NavigationUrl property.")] // Obsolete since v2.6.0.
        protected virtual void Navigate()
        {
        }

        /// <summary>
        /// Appends the value to <see cref="NavigationUrl"/>.
        /// </summary>
        /// <param name="urlPart">The URL part.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner AppendNavigationUrl(string urlPart) =>
            SetNavigationUrl(NavigationUrl is null ? urlPart : NavigationUrl + urlPart);

        /// <summary>
        /// Sets the <see cref="NavigationUrl"/>.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner SetNavigationUrl(string url)
        {
            NavigationUrl = url;
            return (TOwner)this;
        }

        void IPageObject.SwitchToWindow(string windowHandle) =>
            SwitchToWindow(windowHandle);

        /// <summary>
        /// Switches to the browser window by the window handle.
        /// </summary>
        /// <param name="windowHandle">The handle of the window.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner SwitchToWindow(string windowHandle)
        {
            Log.ExecuteSection(
                new LogSection($@"Switch to window ""{windowHandle}""", LogLevel.Trace),
                (Action)(() => Driver.SwitchTo().Window(windowHandle)));

            return (TOwner)this;
        }

        /// <summary>
        /// Switches to frame represented by <paramref name="component"/> parameter.
        /// </summary>
        /// <param name="component">The component representing frame element.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner SwitchToFrame(IUIComponent<TOwner> component)
        {
            string message = component is Frame<TOwner>
                ? $"Switch to {component.ComponentFullName}"
                : $"Switch to frame of {component.ComponentFullName}";

            Log.ExecuteSection(
                new LogSection(message, LogLevel.Trace),
                (Action)(() => SwitchToFrame(component.Scope)));

            return (TOwner)this;
        }

        /// <summary>
        /// Switches to frame represented by <paramref name="element"/> parameter.
        /// </summary>
        /// <param name="element">The frame element.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner SwitchToFrame(IWebElement element)
        {
            Log.ExecuteSection(
                new LogSection($"Switch to frame of {Stringifier.ToString(element).ToLowerFirstLetter()}", LogLevel.Trace),
                (Action)(() => Driver.SwitchTo().Frame(element)));

            return (TOwner)this;
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
            IWebElement frameElement = Scope.GetWithLogging(frameBy);
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
            SwitchToFrame(frameElement);

            return Go.To(framePageObject, navigate: false, temporarily: temporarily);
        }

        /// <summary>
        /// Switches to the root page using WebDriver's <c>SwitchTo().DefaultContent()</c> method.
        /// </summary>
        /// <returns>The instance of this page object.</returns>
        public TOwner SwitchToRoot()
        {
            Log.ExecuteSection(
                new LogSection($"Switch to page root", LogLevel.Trace),
                (Action)(() => Driver.SwitchTo().DefaultContent()));

            return (TOwner)this;
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
            SwitchToRoot();

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
                : Context.WaitingTimeout;

            TimeSpan retryIntervalTime = retryInterval.HasValue
                ? TimeSpan.FromSeconds(retryInterval.Value)
                : Context.WaitingRetryInterval;

            TOwner activePageObject = (TOwner)this;

            string predicateMessage = Stringifier.ToString(predicateExpression);

            string actionMessage = $"Refresh page until {predicateMessage} within {timeoutTime.ToShortIntervalString()} with {retryIntervalTime.ToShortIntervalString()} retry interval";

            Log.ExecuteSection(
                new LogSection(actionMessage),
                () =>
                {
                    bool isOk = Driver
                        .Try(timeoutTime, retryIntervalTime)
                        .Until(x =>
                        {
                            activePageObject = activePageObject.RefreshPage();
                            return predicate(activePageObject);
                        });

                    if (!isOk)
                        throw new TimeoutException($"Timed out after {timeoutTime.ToShortIntervalString()} waiting for: {actionMessage.ToLowerFirstLetter()}.");
                });

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
                SwitchToWindow(nextWindowHandle);
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
        /// <param name="assertionScopeName">
        /// Name of the scope being asserted.
        /// Is used to identify the assertion section in log.
        /// If it is <see langword="null"/>, <see cref="UIComponent.ComponentFullName"/> is used instead.
        /// </param>
        /// <returns>The instance of this page object.</returns>
        public TOwner AggregateAssert(Action<TOwner> action, string assertionScopeName = null)
        {
            action.CheckNotNull(nameof(action));

            assertionScopeName = assertionScopeName ?? ComponentFullName;

            Context.AggregateAssert(() => action((TOwner)this), assertionScopeName);

            return (TOwner)this;
        }

        /// <summary>
        /// Executes aggregate assertion for the component of the current page object using <see cref="AtataContext.AggregateAssert(Action, string)" /> method.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="componentSelector">The component selector.</param>
        /// <param name="action">The action to execute in scope of aggregate assertion.</param>
        /// /// <param name="assertionScopeName">
        /// Name of the scope being asserted.
        /// Is used to identify the assertion section in log.
        /// If it is <see langword="null"/>, component full name is used instead.
        /// </param>
        /// <returns>The instance of this page object.</returns>
        public TOwner AggregateAssert<TComponent>(Func<TOwner, TComponent> componentSelector, Action<TComponent> action, string assertionScopeName = null)
        {
            componentSelector.CheckNotNull(nameof(componentSelector));
            action.CheckNotNull(nameof(action));

            TComponent component = componentSelector((TOwner)this);

            assertionScopeName = assertionScopeName
                ?? UIComponentResolver.ResolveComponentFullName<TOwner>(component) ?? ComponentFullName;

            Context.AggregateAssert(() => action(component), assertionScopeName);

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
                Log.ExecuteSection(
                    new PressKeysLogSection(this, keys),
                    (Action)(() => Driver.Perform(x => x.SendKeys(keys))));
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

        [Obsolete("Use " + nameof(WaitSeconds) + " instead.")] // Obsolete since v2.0.0.
        public TOwner Wait(double seconds) =>
            WaitSeconds(seconds);

        /// <summary>
        /// Waits the specified time in seconds.
        /// </summary>
        /// <param name="seconds">The time to wait in seconds.</param>
        /// <returns>The instance of this page object.</returns>
        public TOwner WaitSeconds(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));

            return (TOwner)this;
        }

        /// <summary>
        /// Scrolls up.
        /// </summary>
        /// <returns>The instance of this page object.</returns>
        public TOwner ScrollUp()
        {
            Log.Info("Scroll up");

            return Script.Execute(
                "document.body.scrollTop = 0;" +
                "document.documentElement.scrollTop = 0;");
        }

        /// <summary>
        /// Scrolls down.
        /// </summary>
        /// <returns>The instance of this page object.</returns>
        public TOwner ScrollDown()
        {
            Log.Info("Scroll down");

            return Script.Execute(
                "var height = document.body.scrollHeight;" +
                "document.body.scrollTop = height;" +
                "document.documentElement.scrollTop = height;");
        }

        void IPageObject.DeInit()
        {
            ExecuteTriggers(TriggerEvents.DeInit);
            Context.EventBus.Publish(new PageObjectDeInitEvent(this));
        }

        protected override string BuildComponentProviderName() => null;
    }
}
