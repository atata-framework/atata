using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atata
{
    public class ExtendedSearchContext<T> : IExtendedSearchContext
        where T : ISearchContext
    {
        public ExtendedSearchContext(T context)
            : this(context, RetrySettings.Timeout)
        {
        }

        public ExtendedSearchContext(T context, TimeSpan timeout)
            : this(context, timeout, RetrySettings.RetryInterval)
        {
        }

        public ExtendedSearchContext(T context, TimeSpan timeout, TimeSpan retryInterval)
        {
            Context = context;
            Timeout = timeout;
            RetryInterval = retryInterval;
        }

        public T Context { get; private set; }

        public TimeSpan Timeout { get; set; }
        public TimeSpan RetryInterval { get; set; }

        public IWebElement FindElement(By by)
        {
            return Find(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return FindAll(by);
        }

        public IWebElement FindElementById(string id)
        {
            return Find(By.Id(id));
        }

        public ReadOnlyCollection<IWebElement> FindElementsById(string id)
        {
            return FindAll(By.Id(id));
        }

        public IWebElement FindElementByName(string name)
        {
            return Find(By.Name(name));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByName(string name)
        {
            return FindAll(By.Name(name));
        }

        public IWebElement FindElementByTagName(string tagName)
        {
            return Find(By.TagName(tagName));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByTagName(string tagName)
        {
            return FindAll(By.TagName(tagName));
        }

        public IWebElement FindElementByClassName(string className)
        {
            return Find(By.ClassName(className));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByClassName(string className)
        {
            return FindAll(By.ClassName(className));
        }

        public IWebElement FindElementByLinkText(string linkText)
        {
            return Find(By.LinkText(linkText));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByLinkText(string linkText)
        {
            return FindAll(By.LinkText(linkText));
        }

        public IWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            return Find(By.PartialLinkText(partialLinkText));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            return FindAll(By.PartialLinkText(partialLinkText));
        }

        public IWebElement FindElementByCssSelector(string cssSelector)
        {
            return Find(By.CssSelector(cssSelector));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector)
        {
            return FindAll(By.CssSelector(cssSelector));
        }

        public IWebElement FindElementByXPath(string xpath)
        {
            return Find(By.XPath(xpath));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath)
        {
            return FindAll(By.XPath(xpath));
        }

        private IWebElement Find(By by)
        {
            ByOptions options = ByOptionsMap.GetOrDefault(by);
            var elements = FindAll(by, options);
            var element = elements.FirstOrDefault();

            if (options.ThrowOnFail && element == null)
                throw ExceptionFactory.CreateForNoSuchElement(options.GetNameWithKind(), by);
            else
                return element;
        }

        private ReadOnlyCollection<IWebElement> FindAll(By by, ByOptions options = null)
        {
            options = options ?? ByOptionsMap.GetOrDefault(by);

            Func<T, ReadOnlyCollection<IWebElement>> findFunction = x => x.FindElements(by);

            // TODO: Extend this.
            if (options.Visibility != ElementVisibility.Any)
            {
                var originalFindFunction = findFunction;
                findFunction = x => originalFindFunction(Context).Where(CreateVisibilityPredicate(options.Visibility)).ToReadOnly();
            }

            return options.Timeout > TimeSpan.Zero ? Until(findFunction) : findFunction(Context);
        }

        private Func<IWebElement, bool> CreateVisibilityPredicate(ElementVisibility visibility)
        {
            switch (visibility)
            {
                case ElementVisibility.Visible:
                    return x => x.Displayed;
                case ElementVisibility.Invisible:
                    return x => !x.Displayed;
                case ElementVisibility.Any:
                    return x => true;
                default:
                    throw new ArgumentException("Unknown ElementVisibility value", "visibility");
            }
        }

        public TResult Until<TResult>(Func<T, TResult> condition)
        {
            var wait = CreateWait();
            return wait.Until(condition);
        }

        public bool Exists(By by)
        {
            return Find(by) != null;
        }

        public bool Missing(By by)
        {
            ByOptions options = ByOptionsMap.GetOrDefault(by);

            Func<T, bool> findFunction;
            if (options.Visibility == ElementVisibility.Any)
                findFunction = x => x.FindElements(by).Count == 0;
            else
                findFunction = x => !x.FindElements(by).Where(CreateVisibilityPredicate(options.Visibility)).Any();

            bool isMissing = options.Timeout > TimeSpan.Zero ? Until(findFunction) : findFunction(Context);
            if (options.ThrowOnFail && !isMissing)
                throw ExceptionFactory.CreateForNotMissingElement(options.GetNameWithKind(), by);
            else
                return isMissing;
        }

        private IWait<T> CreateWait()
        {
            IWait<T> wait = new SafeWait<T>(Context)
            {
                Timeout = Timeout,
                PollingInterval = RetryInterval
            };
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NotFoundException));
            return wait;
        }
    }
}
