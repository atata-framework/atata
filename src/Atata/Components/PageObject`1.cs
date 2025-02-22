#nullable enable

namespace Atata;

/// <summary>
/// Represents the base class for the page objects.
/// Also executes <see cref="TriggerEvents.Init"/> and <see cref="TriggerEvents.DeInit"/> triggers.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public abstract class PageObject<TOwner> : UIComponent<TOwner>, IPageObject<TOwner>, IPageObject
    where TOwner : PageObject<TOwner>
{
    private WebDriverSession _session = null!;

    private PageObjectNavigationUrlData<TOwner>? _navigationUrlData;

    private Control<TOwner>? _activeControl;

    protected PageObject()
    {
        Owner = (TOwner)this;

        ComponentName = UIComponentResolver.ResolvePageObjectName<TOwner>();
        ComponentTypeName = UIComponentResolver.ResolvePageObjectTypeName<TOwner>();

        Type thisType = typeof(TOwner);
        Metadata = new(thisType.Name, thisType, null);
    }

    /// <inheritdoc/>
    public sealed override WebDriverSession Session => _session;

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
        => new((TOwner)this);

    /// <summary>
    /// Gets the expectation verification provider that has a set of verification extension methods.
    /// </summary>
    public new PageObjectVerificationProvider<TOwner> ExpectTo
        => Should.Using(ExpectationVerificationStrategy.Instance);

    /// <summary>
    /// Gets the waiting verification provider that has a set of verification extension methods.
    /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/>
    /// of executing <see cref="AtataContext"/> for timeout and retry interval.
    /// </summary>
    public new PageObjectVerificationProvider<TOwner> WaitTo
        => Should.Using(WaitingVerificationStrategy.Instance);

    [Obsolete("Use SetNavigationUrl(...) or AppendNavigationUrl(...) instead.")] // Obsolete since v4.0.0.
    protected string? NavigationUrl
    {
        get => _navigationUrlData?.Value;
        set => NavigationUrlData.Set(value);
    }

    internal PageObjectNavigationUrlData<TOwner> NavigationUrlData =>
        _navigationUrlData ??= new();

    /// <summary>
    /// Gets a value indicating whether this instance is temporarily navigated using <see cref="GoTemporarilyAttribute"/> or other approach.
    /// </summary>
    protected internal bool IsTemporarilyNavigated { get; internal set; }

    /// <summary>
    /// Gets the previous page object.
    /// </summary>
    protected internal UIComponent? PreviousPageObject { get; internal set; }

    /// <summary>
    /// Gets the <see cref="IWebSessionReport{TOwner}"/> instance that provides a reporting functionality.
    /// </summary>
    public IWebSessionReport<TOwner> Report { get; private set; } = null!;

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
    public UriProvider<TOwner> PageUri { get; private set; } = null!;

    /// <summary>
    /// Gets the page source provider of the current HTML page.
    /// </summary>
    public ValueProvider<string, TOwner> PageSource =>
        CreateValueProvider("page source", GetPageSource);

    /// <summary>
    /// Gets the active control.
    /// </summary>
    public Control<TOwner> ActiveControl =>
        _activeControl ??= Controls.Create<Control<TOwner>>(
            "<Active>",
            new DynamicScopeLocator(_ => Driver.SwitchTo().ActiveElement()));

    internal void AssignToSession(WebDriverSession session)
    {
        if (_session is null)
        {
            _session = session;

            ScopeLocator = new PlainScopeLocator(_session, CreateScopeBy);

            Report = new WebSessionReport<TOwner>((TOwner)this, _session);

            PageUri = new UriProvider<TOwner>(this, GetUri, "URI");

            UIComponentResolver.FillComponentMetadata(Metadata, session.Context.Attributes, typeof(TOwner).Assembly);

            NavigationUrlData.Value = GetNavigationUrlOrUrlFromMetadata();
        }
        else if (session != _session)
        {
            throw new InvalidOperationException($"{ComponentFullName} is already assigned to {_session} and cannot be reassigned to {session}.");
        }
    }

    private string? GetNavigationUrlOrUrlFromMetadata()
    {
        string? navigationUrl = NavigationUrlData.Value;

        if (navigationUrl?.Length > 0)
        {
            if (NavigationUrlData.Appends)
            {
                string? metadataUrl = Metadata.Get<UrlAttribute>()?.Value;
                return metadataUrl?.Length > 0
                    ? metadataUrl + navigationUrl
                    : navigationUrl;
            }
            else
            {
                return navigationUrl;
            }
        }
        else
        {
            return Metadata.Get<UrlAttribute>()?.Value;
        }
    }

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
        UIComponentResolver.Resolve(this);

        OnInit();

#pragma warning disable CS0618 // Type or member is obsolete
        Session.EventBus.Publish(new PageObjectInitEvent(this));
#pragma warning restore CS0618 // Type or member is obsolete

        Session.EventBus.Publish(new PageObjectInitStartedEvent(this));
    }

    internal void CompleteInit()
    {
        ExecuteTriggers(TriggerEvents.Init);

        OnInitCompleted();
        Session.EventBus.Publish(new PageObjectInitCompletedEvent(this));

        OnVerify();
    }

    protected virtual void OnVerify()
    {
    }

    /// <summary>
    /// Appends the value to the navigation URL.
    /// </summary>
    /// <param name="urlPart">The URL part.</param>
    /// <returns>The instance of this page object.</returns>
    public TOwner AppendNavigationUrl(string urlPart)
    {
        NavigationUrlData.Append(urlPart);
        return (TOwner)this;
    }

    /// <summary>
    /// Sets the navigation URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns>The instance of this page object.</returns>
    public TOwner SetNavigationUrl(string url)
    {
        NavigationUrlData.Set(url);
        return (TOwner)this;
    }

    /// <summary>
    /// Sets the URL variable for the navigation URL.
    /// </summary>
    /// <param name="value">The variable value.</param>
    /// <param name="key">The variable key.</param>
    /// <returns>The instance of this page object.</returns>
    public TOwner SetNavigationUrlVariable(object value, [CallerArgumentExpression(nameof(value))] string? key = null)
    {
        NavigationUrlData.SetVariable(key!, value);
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
    public TFramePageObject SwitchToFrame<TFramePageObject>(By frameBy, TFramePageObject? framePageObject = null, bool temporarily = false)
        where TFramePageObject : PageObject<TFramePageObject>
    {
        IWebElement frameElement = Scope.GetWithLogging(Log, frameBy);
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
    public virtual TFramePageObject SwitchToFrame<TFramePageObject>(IWebElement frameElement, TFramePageObject? framePageObject = null, bool temporarily = false)
        where TFramePageObject : PageObject<TFramePageObject>
    {
        SwitchToFrame(frameElement);

        return _session.Go.To(framePageObject, navigate: false, temporarily: temporarily);
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
    public virtual TPageObject SwitchToRoot<TPageObject>(TPageObject? rootPageObject = null)
        where TPageObject : PageObject<TPageObject>
    {
        SwitchToRoot();

        return _session.Go.To(rootPageObject, navigate: false);
    }

    /// <summary>
    /// Waits and switches to the open alert box.
    /// By default, if <paramref name="waitTimeout"/> and <paramref name="waitRetryInterval"/>
    /// arguments are not specified, the <see cref="AtataContext.WaitingTimeout"/>
    /// and <see cref="AtataContext.WaitingRetryInterval"/> values are used correspondingly.
    /// If alert box does not appear within the specified time, the <see cref="TimeoutException"/> is thrown.
    /// </summary>
    /// <param name="waitTimeout">The wait timeout.</param>
    /// <param name="waitRetryInterval">The wait retry interval.</param>
    /// <exception cref="TimeoutException">Timed out waiting for alert box.</exception>
    /// <returns>The new <see cref="AlertBox{TOwner}"/> instance.</returns>
    public AlertBox<TOwner> SwitchToAlertBox(TimeSpan? waitTimeout = null, TimeSpan? waitRetryInterval = null) =>
        new AlertBox<TOwner>((TOwner)this)
            .WaitForAppearance(waitTimeout, waitRetryInterval);

    /// <summary>
    /// Waits and switches to the open confirm box.
    /// By default, if <paramref name="waitTimeout"/> and <paramref name="waitRetryInterval"/>
    /// arguments are not specified, the <see cref="AtataContext.WaitingTimeout"/>
    /// and <see cref="AtataContext.WaitingRetryInterval"/> values are used correspondingly.
    /// If confirm box does not appear within the specified time, the <see cref="TimeoutException"/> is thrown.
    /// </summary>
    /// <param name="waitTimeout">The wait timeout.</param>
    /// <param name="waitRetryInterval">The wait retry interval.</param>
    /// <exception cref="TimeoutException">Timed out waiting for confirm box.</exception>
    /// <returns>The new <see cref="ConfirmBox{TOwner}"/> instance.</returns>
    public ConfirmBox<TOwner> SwitchToConfirmBox(TimeSpan? waitTimeout = null, TimeSpan? waitRetryInterval = null) =>
        new ConfirmBox<TOwner>((TOwner)this)
            .WaitForAppearance(waitTimeout, waitRetryInterval);

    /// <summary>
    /// Waits and switches to the open prompt box.
    /// By default, if <paramref name="waitTimeout"/> and <paramref name="waitRetryInterval"/>
    /// arguments are not specified, the <see cref="AtataContext.WaitingTimeout"/>
    /// and <see cref="AtataContext.WaitingRetryInterval"/> values are used correspondingly.
    /// If confirm box does not appear within the specified time, the <see cref="TimeoutException"/> is thrown.
    /// </summary>
    /// <param name="waitTimeout">The wait timeout.</param>
    /// <param name="waitRetryInterval">The wait retry interval.</param>
    /// <exception cref="TimeoutException">Timed out waiting for prompt box.</exception>
    /// <returns>The new <see cref="PromptBox{TOwner}"/> instance.</returns>
    public PromptBox<TOwner> SwitchToPromptBox(TimeSpan? waitTimeout = null, TimeSpan? waitRetryInterval = null) =>
        new PromptBox<TOwner>((TOwner)this)
            .WaitForAppearance(waitTimeout, waitRetryInterval);

    /// <summary>
    /// Refreshes the current page.
    /// </summary>
    /// <returns>The instance of this page object.</returns>
    public virtual TOwner RefreshPage()
    {
        Log.ExecuteSection(
            "Refresh page",
            () => Driver.Navigate().Refresh());

        return _session.Go.To<TOwner>(navigate: false);
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
            : Session.WaitingTimeout;

        TimeSpan retryIntervalTime = retryInterval.HasValue
            ? TimeSpan.FromSeconds(retryInterval.Value)
            : Session.WaitingRetryInterval;

        TOwner activePageObject = (TOwner)this;

        string predicateMessage = Stringifier.ToString(predicateExpression);

        string actionMessage = $"Refresh page until {predicateMessage} within {timeoutTime.ToShortIntervalString()} with {retryIntervalTime.ToShortIntervalString()} retry interval";

        Log.ExecuteSection(
            new LogSection(actionMessage),
            () =>
            {
                bool isOk = Driver
                    .Try(timeoutTime, retryIntervalTime)
                    .Until(_ =>
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
    public virtual TOther GoBack<TOther>(TOther? previousPageObject = null)
        where TOther : PageObject<TOther>
    {
        Log.ExecuteSection(
            "Go back",
            () => Driver.Navigate().Back());

        return _session.Go.To(previousPageObject, navigate: false);
    }

    /// <summary>
    /// Navigates forward to the next page.
    /// </summary>
    /// <typeparam name="TOther">The type of the page object that represents the next page.</typeparam>
    /// <param name="nextPageObject">
    /// The instance of the next page object.
    /// If equals <see langword="null"/>, creates an instance of <typeparamref name="TOther"/> using the default constructor.</param>
    /// <returns>The instance of the next page object.</returns>
    public virtual TOther GoForward<TOther>(TOther? nextPageObject = null)
        where TOther : PageObject<TOther>
    {
        Log.ExecuteSection(
            "Go forward",
            () => Driver.Navigate().Forward());

        return _session.Go.To(nextPageObject, navigate: false);
    }

    /// <summary>
    /// Closes the current window.
    /// </summary>
    public virtual void CloseWindow()
    {
        string? nextWindowHandle = null;

        _session.Log.ExecuteSection(
            "Close window",
            () =>
            {
                nextWindowHandle = ResolveWindowHandleToSwitchAfterClose();
                Driver.Close();
            });

        if (nextWindowHandle is not null)
            SwitchToWindow(nextWindowHandle);
    }

    private string? ResolveWindowHandleToSwitchAfterClose()
    {
        string currentWindowHandle = Driver.CurrentWindowHandle;
        var allWindowHandles = Driver.WindowHandles;

        int indexOfCurrent = allWindowHandles.IndexOf(currentWindowHandle);

        if (indexOfCurrent > 0)
            return allWindowHandles[indexOfCurrent - 1];
        else if (allWindowHandles.Count > 1)
            return allWindowHandles[1];
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
    /// Executes aggregate assertion for the current page object using <see cref="AtataSession.AggregateAssert(Action, string)" /> method.
    /// </summary>
    /// <param name="action">The action to execute in scope of aggregate assertion.</param>
    /// <param name="assertionScopeName">
    /// Name of the scope being asserted.
    /// Is used to identify the assertion section in log.
    /// If it is <see langword="null"/>, <see cref="UIComponent.ComponentFullName"/> is used instead.
    /// </param>
    /// <returns>The instance of this page object.</returns>
    public TOwner AggregateAssert(Action<TOwner> action, string? assertionScopeName = null)
    {
        action.CheckNotNull(nameof(action));

        assertionScopeName ??= ComponentFullName;

        Session.AggregateAssert(() => action((TOwner)this), assertionScopeName);

        return (TOwner)this;
    }

    /// <summary>
    /// Executes aggregate assertion for the component of the current page object using <see cref="AtataSession.AggregateAssert(Action, string)" /> method.
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
    public TOwner AggregateAssert<TComponent>(Func<TOwner, TComponent> componentSelector, Action<TComponent> action, string? assertionScopeName = null)
    {
        componentSelector.CheckNotNull(nameof(componentSelector));
        action.CheckNotNull(nameof(action));

        TComponent component = componentSelector((TOwner)this);

        assertionScopeName ??= UIComponentResolver.ResolveComponentFullName<TOwner>(component)
            ?? ComponentFullName;

        Session.AggregateAssert(() => action(component), assertionScopeName);

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
        const string script = "document.body.scrollTop = 0;" +
            "document.documentElement.scrollTop = 0;";

        Log.ExecuteSection(
            "Scroll up",
            (Action)(() => Script.Execute(script)));

        return (TOwner)this;
    }

    /// <summary>
    /// Scrolls down.
    /// </summary>
    /// <returns>The instance of this page object.</returns>
    public TOwner ScrollDown()
    {
        const string script = "var height = document.body.scrollHeight;" +
            "document.body.scrollTop = height;" +
            "document.documentElement.scrollTop = height;";

        Log.ExecuteSection(
            "Scroll down",
            (Action)(() => Script.Execute(script)));

        return (TOwner)this;
    }

    void IPageObject.DeInit()
    {
        ExecuteTriggers(TriggerEvents.DeInit);

#pragma warning disable CS0618 // Type or member is obsolete
        Session.EventBus.Publish(new PageObjectDeInitEvent(this));
#pragma warning restore CS0618 // Type or member is obsolete

        Session.EventBus.Publish(new PageObjectDeInitCompletedEvent(this));
    }

    protected override string? BuildComponentProviderName() => null;
}
