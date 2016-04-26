using Humanizer;
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
            ATContext.Current.Log.Info("Switch to '{0}' window", windowName);

            return To(pageObject, new GoOptions { Navigate = false, WindowName = windowName, Temporarily = temporarily });
        }

        public static T ToWindow<T>(string windowName, bool temporarily = false)
            where T : PageObject<T>
        {
            ATContext.Current.Log.Info("Switch to '{0}' window", windowName);

            return To<T>(null, new GoOptions { Navigate = false, WindowName = windowName, Temporarily = temporarily });
        }

        public static T ToNextWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
        {
            ATContext.Current.Log.Info("Switch to next window");

            string windowHandle = ATContext.Current.Driver.WindowHandles.
                SkipWhile(x => x != ATContext.Current.Driver.CurrentWindowHandle).
                ElementAt(1);

            return To(pageObject, new GoOptions { Navigate = false, WindowName = windowHandle, Temporarily = temporarily });
        }

        public static T ToPreviousWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
        {
            ATContext.Current.Log.Info("Switch to previous window");

            string windowHandle = ATContext.Current.Driver.WindowHandles.
                Reverse().
                SkipWhile(x => x != ATContext.Current.Driver.CurrentWindowHandle).
                ElementAt(1);

            return To(pageObject, new GoOptions { Navigate = false, WindowName = windowHandle, Temporarily = temporarily });
        }

        private static T To<T>(T pageObject, GoOptions options)
            where T : PageObject<T>
        {
            if (ATContext.Current.PageObject == null)
            {
                pageObject = pageObject ?? Activator.CreateInstance<T>();
                ATContext.Current.PageObject = pageObject;

                if (!string.IsNullOrWhiteSpace(options.Url))
                {
                    ToUrl(options.Url);
                }

                pageObject.NavigateOnInit = options.Navigate;
                pageObject.Init(new PageObjectContext(ATContext.Current.Driver, ATContext.Current.Log));
                return pageObject;
            }
            else
            {
                IPageObject currentPageObject = (IPageObject)ATContext.Current.PageObject;
                T newPageObject = currentPageObject.GoTo(pageObject, options);
                ATContext.Current.PageObject = newPageObject;
                return newPageObject;
            }
        }

        public static void ToUrl(string url)
        {
            Uri absoluteUri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                if (!ATContext.Current.IsNavigated)
                {
                    if (!Uri.TryCreate(ATContext.Current.StartUri, UriKind.Absolute, out absoluteUri))
                        throw new InvalidOperationException("Cannot navigate to relative URI '{0}'. ATContext.Current.StartUri ".FormatWith(absoluteUri));
                    absoluteUri = new Uri(absoluteUri, url);
                }
                else
                {
                    Uri currentUri = new Uri(ATContext.Current.Driver.Url, UriKind.Absolute);

                    string domainPart = currentUri.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
                    Uri domainUri = new Uri(domainPart);

                    absoluteUri = new Uri(domainUri, url);
                }
            }
            ATContext.Current.Log.Info("Go to URL '{0}'", absoluteUri);
            ATContext.Current.Driver.Navigate().GoToUrl(absoluteUri);
            ATContext.Current.IsNavigated = true;
        }
    }
}
