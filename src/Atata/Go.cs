using System;
using System.Linq;

namespace Atata
{
    public static class Go
    {
        /// <summary>
        /// Navigates to the specified page object.
        /// </summary>
        /// <typeparam name="T">The type of the page object.</typeparam>
        /// <param name="pageObject">The page object. If set to <c>null</c> creates an instance using the default constructor.</param>
        /// <param name="url">The URL.</param>
        /// <param name="navigate">If set to <c>true</c> executes page object navigation functionality.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The page object.</returns>
        public static T To<T>(T pageObject = null, string url = null, bool navigate = true, bool temporarily = false)
            where T : PageObject<T>
        {
            return To(pageObject, new GoOptions { Url = url, Navigate = string.IsNullOrWhiteSpace(url) && navigate, Temporarily = temporarily });
        }

        /// <summary>
        /// Navigates to the window with the specified page object by name.
        /// </summary>
        /// <typeparam name="T">The type of the page object.</typeparam>
        /// <param name="pageObject">The page object. If set to <c>null</c> creates an instance using the default constructor.</param>
        /// <param name="windowName">Name of the browser window.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The page object.</returns>
        public static T ToWindow<T>(T pageObject, string windowName, bool temporarily = false)
            where T : PageObject<T>
        {
            AtataContext.Current.Log.Info("Switch to \"{0}\" window", windowName);

            return To(pageObject, new GoOptions { Navigate = false, WindowName = windowName, Temporarily = temporarily });
        }

        /// <summary>
        /// Navigates to the window by name.
        /// </summary>
        /// <typeparam name="T">The type of the page object.</typeparam>
        /// <param name="windowName">Name of the browser window.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The page object.</returns>
        public static T ToWindow<T>(string windowName, bool temporarily = false)
            where T : PageObject<T>
        {
            AtataContext.Current.Log.Info("Switch to \"{0}\" window", windowName);

            return To<T>(null, new GoOptions { Navigate = false, WindowName = windowName, Temporarily = temporarily });
        }

        /// <summary>
        /// Navigates to the next window with the specified page object.
        /// </summary>
        /// <typeparam name="T">The type of the page object.</typeparam>
        /// <param name="pageObject">The page object. If set to <c>null</c> creates an instance using the default constructor.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The page object.</returns>
        public static T ToNextWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
        {
            AtataContext.Current.Log.Info("Switch to next window");

            string windowHandle = AtataContext.Current.Driver.WindowHandles.
                SkipWhile(x => x != AtataContext.Current.Driver.CurrentWindowHandle).
                ElementAt(1);

            return To(pageObject, new GoOptions { Navigate = false, WindowName = windowHandle, Temporarily = temporarily });
        }

        /// <summary>
        /// Navigates to the previous window with the specified page object.
        /// </summary>
        /// <typeparam name="T">The type of the page object.</typeparam>
        /// <param name="pageObject">The page object. If set to <c>null</c> creates an instance using the default constructor.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The page object.</returns>
        public static T ToPreviousWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
        {
            AtataContext.Current.Log.Info("Switch to previous window");

            string windowHandle = AtataContext.Current.Driver.WindowHandles.
                Reverse().
                SkipWhile(x => x != AtataContext.Current.Driver.CurrentWindowHandle).
                ElementAt(1);

            return To(pageObject, new GoOptions { Navigate = false, WindowName = windowHandle, Temporarily = temporarily });
        }

        private static T To<T>(T pageObject, GoOptions options)
            where T : PageObject<T>
        {
            CheckAtataContext();

            if (AtataContext.Current.PageObject == null)
            {
                pageObject = pageObject ?? ActivatorEx.CreateInstance<T>();
                AtataContext.Current.PageObject = pageObject;

                if (!string.IsNullOrWhiteSpace(options.Url))
                    ToUrl(options.Url);

                pageObject.NavigateOnInit = options.Navigate;
                pageObject.Init();
                return pageObject;
            }
            else
            {
                IPageObject currentPageObject = (IPageObject)AtataContext.Current.PageObject;
                T newPageObject = currentPageObject.GoTo(pageObject, options);
                AtataContext.Current.PageObject = newPageObject;
                return newPageObject;
            }
        }

        /// <summary>
        /// Navigates to the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        public static void ToUrl(string url)
        {
            CheckAtataContext();

            Uri absoluteUri;

            if (!Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                if (!AtataContext.Current.IsNavigated && AtataContext.Current.BaseUrl == null)
                {
                    if (string.IsNullOrWhiteSpace(url))
                        throw new InvalidOperationException("Cannot navigate to empty or null URL. AtataContext.Current.BaseUrl can be set with base URL.");
                    else
                        throw new InvalidOperationException($"Cannot navigate to relative URL \"{url}\". AtataContext.Current.BaseUrl can be set with base URL.");
                }

                if (AtataContext.Current.BaseUrl == null)
                {
                    Uri currentUri = new Uri(AtataContext.Current.Driver.Url, UriKind.Absolute);

                    string domainPart = currentUri.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
                    Uri domainUri = new Uri(domainPart);

                    absoluteUri = new Uri(domainUri, url);
                }
                else
                {
                    absoluteUri = ConcatWithBaseUrl(url);
                }
            }

            Navigate(absoluteUri);
        }

        private static void CheckAtataContext()
        {
            if (AtataContext.Current == null)
                AtataContext.Configure().Build();
        }

        private static void Navigate(Uri uri)
        {
            AtataContext.Current.Log.Info($"Go to URL \"{uri}\"");
            AtataContext.Current.Driver.Navigate().GoToUrl(uri);
            AtataContext.Current.IsNavigated = true;
        }

        private static Uri ConcatWithBaseUrl(string relativeUri)
        {
            string baseUrl = AtataContext.Current.BaseUrl;
            string fullUrl = baseUrl;

            if (!string.IsNullOrWhiteSpace(relativeUri))
            {
                if (baseUrl.EndsWith("/") && relativeUri.StartsWith("/"))
                    fullUrl += relativeUri.Substring(1);
                else if (!baseUrl.EndsWith("/") && !relativeUri.StartsWith("/"))
                    fullUrl += "/" + relativeUri;
                else
                    fullUrl += relativeUri;
            }

            return new Uri(fullUrl);
        }
    }
}
