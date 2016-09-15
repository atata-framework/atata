using System;
using System.Linq;

namespace Atata
{
    public static class Go
    {
        public static T To<T>(T pageObject = null, string url = null, bool navigate = true, bool temporarily = false)
            where T : PageObject<T>
        {
            return To(pageObject, new GoOptions { Url = url, Navigate = string.IsNullOrWhiteSpace(url) && navigate, Temporarily = temporarily });
        }

        public static T ToWindow<T>(T pageObject, string windowName, bool temporarily = false)
            where T : PageObject<T>
        {
            AtataContext.Current.Log.Info("Switch to \"{0}\" window", windowName);

            return To(pageObject, new GoOptions { Navigate = false, WindowName = windowName, Temporarily = temporarily });
        }

        public static T ToWindow<T>(string windowName, bool temporarily = false)
            where T : PageObject<T>
        {
            AtataContext.Current.Log.Info("Switch to \"{0}\" window", windowName);

            return To<T>(null, new GoOptions { Navigate = false, WindowName = windowName, Temporarily = temporarily });
        }

        public static T ToNextWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
        {
            AtataContext.Current.Log.Info("Switch to next window");

            string windowHandle = AtataContext.Current.Driver.WindowHandles.
                SkipWhile(x => x != AtataContext.Current.Driver.CurrentWindowHandle).
                ElementAt(1);

            return To(pageObject, new GoOptions { Navigate = false, WindowName = windowHandle, Temporarily = temporarily });
        }

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
            if (AtataContext.Current == null)
                AtataContext.Build().SetUp();

            if (AtataContext.Current.PageObject == null)
            {
                pageObject = pageObject ?? ActivatorEx.CreateInstance<T>();
                AtataContext.Current.PageObject = pageObject;

                if (!string.IsNullOrWhiteSpace(options.Url))
                {
                    ToUrl(options.Url);
                }

                pageObject.NavigateOnInit = options.Navigate;
                pageObject.Init(new PageObjectContext(AtataContext.Current.Driver, AtataContext.Current.Log));
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

        public static void ToUrl(string url)
        {
            if (AtataContext.Current == null)
                AtataContext.Build().SetUp();

            Uri absoluteUri;

            if (!Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                if (!AtataContext.Current.IsNavigated && AtataContext.Current.BaseUrl == null)
                    throw new InvalidOperationException("Cannot navigate to relative URL \"{0}\". ATContext.Current.BaseUrl can be set with base URL.".FormatWith(absoluteUri));

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

            AtataContext.Current.Log.Info("Go to URL \"{0}\"", absoluteUri);
            AtataContext.Current.Driver.Navigate().GoToUrl(absoluteUri);
            AtataContext.Current.IsNavigated = true;
        }

        private static Uri ConcatWithBaseUrl(string relativeUri)
        {
            string baseUrl = AtataContext.Current.BaseUrl;
            string fullUrl = baseUrl;

            if (baseUrl.EndsWith("/") && relativeUri.StartsWith("/"))
                fullUrl += relativeUri.Substring(1);
            else if (!baseUrl.EndsWith("/") && !relativeUri.StartsWith("/"))
                fullUrl += "/" + relativeUri;
            else
                fullUrl += relativeUri;

            return new Uri(fullUrl);
        }
    }
}
