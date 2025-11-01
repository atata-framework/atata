namespace Atata;

/// <summary>
/// Represents the navigation functionality between pages and windows.
/// </summary>
public sealed class WebDriverSessionNavigator
{
    private readonly WebDriverSession _session;

    internal WebDriverSessionNavigator(WebDriverSession session) =>
        _session = session;

    /// <summary>
    /// Continues with the specified page object type.
    /// Firstly, checks whether the current <see cref="WebSession.PageObject"/>
    /// is <typeparamref name="T"/>, if it is, returns it;
    /// otherwise, creates a new instance of <typeparamref name="T"/> without navigation.
    /// The method is useful in case when in a particular step method (BDD step, for example)
    /// you don't have an instance of current page object but you are sure that a browser is on the needed page.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <returns>The page object.</returns>
    public T On<T>()
        where T : PageObject<T> =>
        (_session.PageObject as T)
            ?? To<T>(null, new GoOptions { Navigate = false });

    /// <summary>
    /// Continues with the specified page object type with rage refresh.
    /// Firstly, checks whether the current <see cref="WebSession.PageObject"/>
    /// is <typeparamref name="T"/>, if it is, returns it;
    /// otherwise, creates a new instance of <typeparamref name="T"/> without navigation.
    /// Then a page is refreshed.
    /// The method is useful in case when you reuse a single test suite driver by tests and
    /// want to refresh a page on start of each test to ensure that the page is in clean start state.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <returns>The page object.</returns>
    public T OnRefreshed<T>()
        where T : PageObject<T> =>
        On<T>()
            .RefreshPage();

    /// <summary>
    /// Continues with the specified page object type or navigates to it.
    /// Firstly, checks whether the current <see cref="WebSession.PageObject"/>
    /// is <typeparamref name="T"/>, if it is, returns it;
    /// otherwise, creates a new instance of <typeparamref name="T"/> with navigation.
    /// The method is useful in case when in a particular step method (BDD step, for example)
    /// you don't have an instance of current page object and you are not sure that a browser is on the needed page, but can be.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <returns>The page object.</returns>
    public T OnOrTo<T>()
        where T : PageObject<T> =>
        (_session.PageObject as T)
            ?? To<T>(null, new GoOptions { Navigate = true });

    /// <summary>
    /// Navigates to the specified page object.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <param name="pageObject">
    /// The page object.
    /// If set to <see langword="null"/> creates an instance using the default constructor.</param>
    /// <param name="url">The URL.</param>
    /// <param name="navigate">If set to <see langword="true"/> executes page object navigation functionality.</param>
    /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
    /// <returns>The page object.</returns>
    public T To<T>(T? pageObject = null, string? url = null, bool navigate = true, bool temporarily = false)
        where T : PageObject<T> =>
        To(pageObject, new GoOptions { Url = url, Navigate = navigate && !UriUtils.IsUrlHasPath(url), Temporarily = temporarily });

    /// <summary>
    /// Navigates to the window by name.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <param name="windowName">Name of the browser window.</param>
    /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
    /// <returns>The page object.</returns>
    public T ToWindow<T>(string windowName, bool temporarily = false)
        where T : PageObject<T> =>
        ToWindow<T>(null, windowName, temporarily);

    /// <summary>
    /// Navigates to the window with the specified page object by name.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <param name="pageObject">
    /// The page object.
    /// If set to <see langword="null"/> creates an instance using the default constructor.</param>
    /// <param name="windowName">Name of the browser window.</param>
    /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
    /// <returns>The page object.</returns>
    public T ToWindow<T>(T? pageObject, string windowName, bool temporarily = false)
        where T : PageObject<T> =>
        To(
            pageObject,
            new GoOptions
            {
                Navigate = false,
                WindowNameResolver = () => windowName,
                NavigationTarget = $"in \"{windowName}\" window",
                Temporarily = temporarily
            });

    /// <summary>
    /// Navigates to the next window with the specified page object.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <param name="pageObject">
    /// The page object.
    /// If set to <see langword="null"/> creates an instance using the default constructor.</param>
    /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
    /// <returns>The page object.</returns>
    public T ToNextWindow<T>(T? pageObject = null, bool temporarily = false)
        where T : PageObject<T> =>
        To(
            pageObject,
            new GoOptions
            {
                Navigate = false,
                WindowNameResolver = GetNextWindowHandle,
                NavigationTarget = "in next window",
                Temporarily = temporarily
            });

    private string GetNextWindowHandle()
    {
        string currentWindowHandle = _session.Driver.CurrentWindowHandle;

        return _session.Driver.WindowHandles
            .SkipWhile(x => x != currentWindowHandle)
            .ElementAt(1);
    }

    /// <summary>
    /// Navigates to the previous window with the specified page object.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <param name="pageObject">
    /// The page object.
    /// If set to <see langword="null"/> creates an instance using the default constructor.</param>
    /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
    /// <returns>The page object.</returns>
    public T ToPreviousWindow<T>(T? pageObject = null, bool temporarily = false)
        where T : PageObject<T> =>
        To(
            pageObject,
            new GoOptions
            {
                Navigate = false,
                WindowNameResolver = GetPreviousWindowHandle,
                NavigationTarget = "in previous window",
                Temporarily = temporarily
            });

    private string GetPreviousWindowHandle()
    {
        string currentWindowHandle = _session.Driver.CurrentWindowHandle;

        return _session.Driver.WindowHandles
            .Reverse()
            .SkipWhile(x => x != currentWindowHandle)
            .ElementAt(1);
    }

    /// <summary>
    /// Navigates to the a tab window with the specified page object.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <param name="pageObject">
    /// The page object.
    /// If set to <see langword="null"/> creates an instance using the default constructor.</param>
    /// <param name="url">The URL.</param>
    /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
    /// <returns>The page object.</returns>
    public T ToNewWindowAsTab<T>(T? pageObject = null, string? url = null, bool temporarily = false)
        where T : PageObject<T> =>
        ToNewWindow(pageObject, url, temporarily, WindowType.Tab);

    /// <summary>
    /// Navigates to a new window with the specified page object.
    /// </summary>
    /// <typeparam name="T">The type of the page object.</typeparam>
    /// <param name="pageObject">
    /// The page object.
    /// If set to <see langword="null"/> creates an instance using the default constructor.</param>
    /// <param name="url">The URL.</param>
    /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
    /// <returns>The page object.</returns>
    public T ToNewWindow<T>(T? pageObject = null, string? url = null, bool temporarily = false)
        where T : PageObject<T> =>
        ToNewWindow(pageObject, url, temporarily, WindowType.Window);

    private T ToNewWindow<T>(T? pageObject, string? url, bool temporarily, WindowType windowType)
        where T : PageObject<T> =>
        To(
            pageObject,
            new GoOptions
            {
                Url = url,
                Navigate = !UriUtils.IsUrlHasPath(url),
                NewWindowType = windowType,
                NavigationTarget = $"in new {(windowType == WindowType.Tab ? "tab " : null)}window",
                Temporarily = temporarily
            });

    private T To<T>(T? pageObject, GoOptions options)
        where T : PageObject<T>
    {
        SetSessionAsCurrent();

        var currentPageObject = _session.PageObject;

        return currentPageObject is null
            ? GoToInitialPageObject(pageObject, options)
            : GoToFollowingPageObject(currentPageObject, pageObject, options);
    }

    private T GoToInitialPageObject<T>(T? pageObject, GoOptions options)
        where T : PageObject<T>
    {
        pageObject ??= ActivatorEx.CreateInstance<T>();
        pageObject.AssignToSession(_session);
        _session.PageObject = pageObject;

        string? navigationUrl = options.Navigate
            ? pageObject.NavigationUrlData.Value is null or []
                ? _session.BaseUrl
                : pageObject.NavigationUrlData.Value
            : options.Url;

        navigationUrl = PrepareNavigationUrl(
            navigationUrl,
            options,
            pageObject.NavigationUrlData.Variables);

        _session.Log.ExecuteSection(
            new GoToPageObjectLogSection(pageObject, navigationUrl, options.NavigationTarget),
            () =>
            {
                if (navigationUrl?.Length > 0 || options.Navigate)
                {
                    Uri uri = CreateAbsoluteUriForNavigation(navigationUrl);
                    Navigate(uri);
                }

                pageObject.Init();
            });

        pageObject.CompleteInit();

        return pageObject;
    }

    private T GoToFollowingPageObject<T>(
        UIComponent currentPageObject,
        T? nextPageObject,
        GoOptions options)
        where T : PageObject<T>
    {
        bool isReturnedFromTemporary = TryResolvePreviousPageObjectNavigatedTemporarily(ref nextPageObject);

        nextPageObject ??= ActivatorEx.CreateInstance<T>();

        if (!isReturnedFromTemporary)
        {
            if (!options.Temporarily)
                _session.CleanUpTemporarilyPreservedPageObjectList();

            if (options.Temporarily)
            {
                nextPageObject.IsTemporarilyNavigated = options.Temporarily;
                _session.TemporarilyPreservedPageObjectList.Add(currentPageObject);
            }
        }

        ((IPageObject)currentPageObject).DeInit();

        if (!isReturnedFromTemporary)
            nextPageObject.AssignToSession(_session);

        _session.PageObject = nextPageObject;

        // TODO: Review this condition.
        if (!options.Temporarily)
            UIComponentResolver.CleanUpPageObject(currentPageObject);

        string? navigationUrl = options.Navigate
            ? nextPageObject.NavigationUrlData.Value
            : options.Url;

        navigationUrl = PrepareNavigationUrl(
            navigationUrl,
            options,
            nextPageObject.NavigationUrlData.Variables);

        _session.Log.ExecuteSection(
            new GoToPageObjectLogSection(nextPageObject, navigationUrl, options.NavigationTarget),
            () =>
            {
                if (options.NewWindowType != null)
                    SwitchToNewWindow(options.NewWindowType.Value);

                if (options.WindowNameResolver != null)
                    ((IPageObject)currentPageObject).SwitchToWindow(options.WindowNameResolver.Invoke());

                if (navigationUrl?.Length > 0)
                {
                    Uri uri = CreateAbsoluteUriForNavigation(navigationUrl);
                    Navigate(uri);
                }

                if (!isReturnedFromTemporary)
                {
                    nextPageObject.PreviousPageObject = currentPageObject;
                    nextPageObject.Init();
                }
            });

        if (!isReturnedFromTemporary)
            nextPageObject.CompleteInit();

        return nextPageObject;
    }

    private string? PrepareNavigationUrl(string? navigationUrl, GoOptions options, IEnumerable<KeyValuePair<string, object?>>? navigationUrlVariables)
    {
        if (navigationUrl?.Length > 0)
            navigationUrl = _session.Variables.FillUriTemplateString(navigationUrl, navigationUrlVariables);

        navigationUrl = NormalizeAsAbsoluteUrlSafely(navigationUrl);

        if (options.Navigate && options.Url?.Length > 0)
        {
            string additionalUrl = _session.Variables.FillUriTemplateString(options.Url, navigationUrlVariables);
            navigationUrl = UriUtils.MergeAsString(navigationUrl, additionalUrl);
        }

        return navigationUrl;
    }

    private void SwitchToNewWindow(WindowType windowType) =>
        _session.Log.ExecuteSection(
            new LogSection($"Switch to new {(windowType == WindowType.Tab ? "tab " : null)}window", LogLevel.Trace),
            (Action)(() => _session.Driver.SwitchTo().NewWindow(windowType)));

    private bool TryResolvePreviousPageObjectNavigatedTemporarily<TPageObject>(ref TPageObject? pageObject)
        where TPageObject : PageObject<TPageObject>
    {
        var tempPageObjectsEnumerable = _session.TemporarilyPreservedPageObjects
            .AsEnumerable()
            .Reverse()
            .OfType<TPageObject>();

        TPageObject? pageObjectReferenceCopy = pageObject;

        TPageObject foundPageObject = pageObject is null
            ? tempPageObjectsEnumerable.FirstOrDefault(x => x.GetType() == typeof(TPageObject))
            : tempPageObjectsEnumerable.FirstOrDefault(x => x == pageObjectReferenceCopy);

        if (foundPageObject is null)
            return false;

        pageObject = foundPageObject;

        var tempPageObjectsToRemove = _session.TemporarilyPreservedPageObjects
            .SkipWhile(x => x != foundPageObject)
            .ToArray();

        UIComponentResolver.CleanUpPageObjects(tempPageObjectsToRemove.Skip(1));

        foreach (var item in tempPageObjectsToRemove)
            _session.TemporarilyPreservedPageObjectList.Remove(item);

        return true;
    }

    /// <summary>
    /// Navigates to the specified URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    public void ToUrl(string url)
    {
        SetSessionAsCurrent();

        Uri absoluteUrl = CreateAbsoluteUriForNavigation(url);

        _session.Log.ExecuteSection(
            new GoToUrlLogSection(absoluteUrl),
            () => Navigate(absoluteUrl));
    }

    private Uri CreateAbsoluteUriForNavigation(string? url)
    {
        if (UriUtils.TryCreateAbsoluteUrl(url, out Uri? absoluteUrl))
            return absoluteUrl;

        if (!_session.IsNavigated && _session.BaseUrl is null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("Cannot navigate to empty or null URL. AtataContext.Current.BaseUrl can be set with base URL.");
            else
                throw new InvalidOperationException($"Cannot navigate to relative URL \"{url}\". AtataContext.Current.BaseUrl can be set with base URL.");
        }

        if (_session.BaseUrl is null)
        {
            Uri currentUri = new Uri(_session.Driver.Url, UriKind.Absolute);

            string domainPart = currentUri.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
            Uri domainUri = new Uri(domainPart);

            return new Uri(domainUri, url);
        }
        else
        {
            return UriUtils.Concat(_session.BaseUrl, url);
        }
    }

    private string? NormalizeAsAbsoluteUrlSafely(string? url) =>
        url?.Length > 0 && !UriUtils.TryCreateAbsoluteUrl(url, out _) && _session.BaseUrl != null
            ? UriUtils.Concat(_session.BaseUrl, url).ToString()
            : url;

    private void Navigate(Uri uri)
    {
        _session.Driver.Navigate().GoToUrl(uri);
        _session.IsNavigated = true;
    }

    private void SetSessionAsCurrent() =>
        _session.SetAsCurrent();

    private sealed class GoOptions
    {
        public string? Url { get; set; }

        public Func<string>? WindowNameResolver { get; set; }

        public WindowType? NewWindowType { get; set; }

        public bool Navigate { get; set; }

        public bool Temporarily { get; set; }

        public string? NavigationTarget { get; set; }
    }
}
