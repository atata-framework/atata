using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the navigation functionality between pages and windows.
    /// </summary>
    public class AtataNavigator
    {
        private readonly AtataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtataNavigator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public AtataNavigator(AtataContext context) =>
            _context = context.CheckNotNull(nameof(context));

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
        public T To<T>(T pageObject = null, string url = null, bool navigate = true, bool temporarily = false)
            where T : PageObject<T> =>
            To(pageObject, new GoOptions { Url = url, Navigate = string.IsNullOrWhiteSpace(url) && navigate, Temporarily = temporarily });

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
        public T ToWindow<T>(T pageObject, string windowName, bool temporarily = false)
            where T : PageObject<T>
        {
            SetContextAsCurrent();

            return To(
                pageObject,
                new GoOptions
                {
                    Navigate = false,
                    WindowNameResolver = () => windowName,
                    NavigationTarget = $"in \"{windowName}\" window",
                    Temporarily = temporarily
                });
        }

        /// <summary>
        /// Navigates to the next window with the specified page object.
        /// </summary>
        /// <typeparam name="T">The type of the page object.</typeparam>
        /// <param name="pageObject">
        /// The page object.
        /// If set to <see langword="null"/> creates an instance using the default constructor.</param>
        /// <param name="temporarily">If set to <see langword="true"/> navigates temporarily preserving current page object state.</param>
        /// <returns>The page object.</returns>
        public T ToNextWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
        {
            SetContextAsCurrent();

            return To(
                pageObject,
                new GoOptions
                {
                    Navigate = false,
                    WindowNameResolver = GetNextWindowHandle,
                    NavigationTarget = "in next window",
                    Temporarily = temporarily
                });
        }

        private string GetNextWindowHandle()
        {
            string currentWindowHandle = _context.Driver.CurrentWindowHandle;

            return _context.Driver.WindowHandles
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
        public T ToPreviousWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
        {
            SetContextAsCurrent();

            return To(
                pageObject,
                new GoOptions
                {
                    Navigate = false,
                    WindowNameResolver = GetPreviousWindowHandle,
                    NavigationTarget = "in previous window",
                    Temporarily = temporarily
                });
        }

        private string GetPreviousWindowHandle()
        {
            string currentWindowHandle = _context.Driver.CurrentWindowHandle;

            return _context.Driver.WindowHandles
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
        public T ToNewWindowAsTab<T>(T pageObject = null, string url = null, bool temporarily = false)
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
        public T ToNewWindow<T>(T pageObject = null, string url = null, bool temporarily = false)
            where T : PageObject<T> =>
            ToNewWindow(pageObject, url, temporarily, WindowType.Window);

        private T ToNewWindow<T>(T pageObject, string url, bool temporarily, WindowType windowType)
            where T : PageObject<T> =>
            To(
                pageObject,
                new GoOptions
                {
                    Url = url,
                    Navigate = string.IsNullOrWhiteSpace(url),
                    NewWindowType = windowType,
                    NavigationTarget = $"in new {(windowType == WindowType.Tab ? "tab " : null)}window",
                    Temporarily = temporarily
                });

        private T To<T>(T pageObject, GoOptions options)
            where T : PageObject<T>
        {
            SetContextAsCurrent();

            var currentPageObject = _context.PageObject;

            return currentPageObject is null
                ? GoToInitialPageObject(pageObject, options)
                : GoToFollowingPageObject(currentPageObject, pageObject, options);
        }

        private T GoToInitialPageObject<T>(T pageObject, GoOptions options)
            where T : PageObject<T>
        {
            pageObject = pageObject ?? ActivatorEx.CreateInstance<T>();
            _context.PageObject = pageObject;

            string navigationUrl = NormalizeAsAbsoluteUrlSafely(
                options.Navigate
                    ? string.IsNullOrEmpty(pageObject.NavigationUrl)
                        ? _context.BaseUrl
                        : pageObject.NavigationUrl
                    : options.Url);

            _context.Log.ExecuteSection(
                new GoToPageObjectLogSection(pageObject, navigationUrl, options.NavigationTarget),
                () =>
                {
                    if (!string.IsNullOrEmpty(navigationUrl) || options.Navigate)
                        ToUrl(navigationUrl, false);

#pragma warning disable CS0618 // Type or member is obsolete
                    pageObject.NavigateOnInit = options.Navigate;
#pragma warning restore CS0618 // Type or member is obsolete
                    pageObject.Init();
                });

            pageObject.CompleteInit();

            return pageObject;
        }

        private T GoToFollowingPageObject<T>(
            UIComponent currentPageObject,
            T nextPageObject,
            GoOptions options)
            where T : PageObject<T>
        {
            bool isReturnedFromTemporary = TryResolvePreviousPageObjectNavigatedTemporarily(ref nextPageObject);

            nextPageObject = nextPageObject ?? ActivatorEx.CreateInstance<T>();

            if (!isReturnedFromTemporary)
            {
                if (!options.Temporarily)
                    _context.CleanUpTemporarilyPreservedPageObjectList();

#pragma warning disable CS0618 // Type or member is obsolete
                nextPageObject.NavigateOnInit = options.Navigate;
#pragma warning restore CS0618 // Type or member is obsolete

                if (options.Temporarily)
                {
                    nextPageObject.IsTemporarilyNavigated = options.Temporarily;
                    _context.TemporarilyPreservedPageObjectList.Add(currentPageObject);
                }
            }

            ((IPageObject)currentPageObject).DeInit();

            _context.PageObject = nextPageObject;

            // TODO: Review this condition.
            if (!options.Temporarily)
                UIComponentResolver.CleanUpPageObject(currentPageObject);

            string navigationUrl = NormalizeAsAbsoluteUrlSafely(
                options.Navigate ? nextPageObject.NavigationUrl : options.Url);

            _context.Log.ExecuteSection(
                new GoToPageObjectLogSection(nextPageObject, navigationUrl, options.NavigationTarget),
                () =>
                {
                    if (options.NewWindowType != null)
                        SwitchToNewWindow(options.NewWindowType.Value);

                    if (options.WindowNameResolver != null)
                        ((IPageObject)currentPageObject).SwitchToWindow(options.WindowNameResolver.Invoke());

                    if (!string.IsNullOrEmpty(navigationUrl))
                        ToUrl(navigationUrl, false);

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

        private void SwitchToNewWindow(WindowType windowType) =>
            _context.Log.ExecuteSection(
                new LogSection($"Switch to new {(windowType == WindowType.Tab ? "tab " : null)}window", LogLevel.Trace),
                (Action)(() => _context.Driver.SwitchTo().NewWindow(windowType)));

        private bool TryResolvePreviousPageObjectNavigatedTemporarily<TPageObject>(ref TPageObject pageObject)
            where TPageObject : PageObject<TPageObject>
        {
            var tempPageObjectsEnumerable = _context.TemporarilyPreservedPageObjects
                .AsEnumerable()
                .Reverse()
                .OfType<TPageObject>();

            TPageObject pageObjectReferenceCopy = pageObject;

            TPageObject foundPageObject = pageObject == null
                ? tempPageObjectsEnumerable.FirstOrDefault(x => x.GetType() == typeof(TPageObject))
                : tempPageObjectsEnumerable.FirstOrDefault(x => x == pageObjectReferenceCopy);

            if (foundPageObject == null)
                return false;

            pageObject = foundPageObject;

            var tempPageObjectsToRemove = _context.TemporarilyPreservedPageObjects
                .SkipWhile(x => x != foundPageObject)
                .ToArray();

            UIComponentResolver.CleanUpPageObjects(tempPageObjectsToRemove.Skip(1));

            foreach (var item in tempPageObjectsToRemove)
                _context.TemporarilyPreservedPageObjectList.Remove(item);

            return true;
        }

        /// <summary>
        /// Navigates to the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void ToUrl(string url)
            => ToUrl(url, true);

        private void ToUrl(string url, bool logAsInfo)
        {
            SetContextAsCurrent();

            if (!UriUtils.TryCreateAbsoluteUrl(url, out Uri absoluteUrl))
            {
                if (!_context.IsNavigated && _context.BaseUrl is null)
                {
                    if (string.IsNullOrWhiteSpace(url))
                        throw new InvalidOperationException("Cannot navigate to empty or null URL. AtataContext.Current.BaseUrl can be set with base URL.");
                    else
                        throw new InvalidOperationException($"Cannot navigate to relative URL \"{url}\". AtataContext.Current.BaseUrl can be set with base URL.");
                }

                if (_context.BaseUrl is null)
                {
                    Uri currentUri = new Uri(_context.Driver.Url, UriKind.Absolute);

                    string domainPart = currentUri.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
                    Uri domainUri = new Uri(domainPart);

                    absoluteUrl = new Uri(domainUri, url);
                }
                else
                {
                    absoluteUrl = UriUtils.Concat(_context.BaseUrl, url);
                }
            }

            _context.Log.ExecuteSection(
                new GoToUrlLogSection(absoluteUrl, logAsInfo),
                () => Navigate(absoluteUrl));
        }

        private string NormalizeAsAbsoluteUrlSafely(string url) =>
            !string.IsNullOrEmpty(url) && !UriUtils.TryCreateAbsoluteUrl(url, out _) && _context.BaseUrl != null
                ? UriUtils.Concat(_context.BaseUrl, url).ToString()
                : url;

        private void Navigate(Uri uri)
        {
            _context.Driver.Navigate().GoToUrl(uri);
            _context.IsNavigated = true;
        }

        private void SetContextAsCurrent() =>
            AtataContext.Current = _context;
    }
}
