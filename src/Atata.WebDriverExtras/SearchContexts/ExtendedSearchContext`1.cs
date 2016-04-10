using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
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

        private ByOptions ResolveOptions(By by)
        {
            ByOptions options = ByOptionsMap.GetOrDefault(by);

            if (options.Timeout == null)
                options.Timeout = Timeout;
            if (options.RetryInterval == null)
                options.RetryInterval = RetryInterval;

            return options;
        }

        private IWebElement Find(By by)
        {
            ByOptions options = ResolveOptions(by);
            var elements = FindAll(by, options);
            var element = elements.FirstOrDefault();

            if (options.ThrowOnFail && element == null)
                throw ExceptionFactory.CreateForNoSuchElement(options.GetNameWithKind(), by);
            else
                return element;
        }

        private ReadOnlyCollection<IWebElement> FindAll(By by, ByOptions options = null)
        {
            options = options ?? ResolveOptions(by);

            Func<T, ReadOnlyCollection<IWebElement>> findFunction = x => x.FindElements(by);

            // TODO: Extend this.
            if (options.Visibility != ElementVisibility.Any)
            {
                var originalFindFunction = findFunction;
                findFunction = x => originalFindFunction(Context).Where(CreateVisibilityPredicate(options.Visibility)).ToReadOnly();
            }

            return Until(findFunction, options.Timeout, options.RetryInterval);
        }

        private static Func<IWebElement, bool> CreateVisibilityPredicate(ElementVisibility visibility)
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

        public TResult Until<TResult>(Func<T, TResult> condition, TimeSpan? timeout = null, TimeSpan? retryInterval = null)
        {
            TimeSpan actualTimeout = timeout ?? Timeout;
            TimeSpan actualRetryInterval = retryInterval ?? RetryInterval;

            if (timeout.HasValue && timeout.Value > TimeSpan.Zero)
            {
                var wait = CreateWait(actualTimeout, actualRetryInterval);
                return wait.Until(condition);
            }
            else
            {
                return condition(Context);
            }
        }

        public bool Exists(By by)
        {
            return Find(by) != null;
        }

        public bool Missing(By by)
        {
            ByOptions options = ResolveOptions(by);

            Func<T, bool> findFunction;
            if (options.Visibility == ElementVisibility.Any)
                findFunction = x => x.FindElements(by).Count == 0;
            else
                findFunction = x => !x.FindElements(by).Where(CreateVisibilityPredicate(options.Visibility)).Any();

            bool isMissing = Until(findFunction, options.Timeout, options.RetryInterval);

            if (options.ThrowOnFail && !isMissing)
                throw ExceptionFactory.CreateForNotMissingElement(options.GetNameWithKind(), by);
            else
                return isMissing;
        }

        public bool MissingAll(params By[] byArray)
        {
            return MissingAll(byArray.ToDictionary(x => x, x => (ISearchContext)Context));
        }

        public bool MissingAll(Dictionary<By, ISearchContext> byContextPairs)
        {
            if (byContextPairs == null)
                throw new ArgumentNullException("byContextPairs");
            if (byContextPairs.Count == 0)
                throw new ArgumentException("Collection should not be empty.", "byContextPairs");

            Dictionary<By, ByOptions> byOptions = byContextPairs.Keys.ToDictionary(x => x, x => ResolveOptions(x));

            List<By> leftBys = byContextPairs.Keys.ToList();

            Func<T, bool> findFunction = _ =>
            {
                By[] currentByArray = leftBys.ToArray();
                foreach (By by in currentByArray)
                {
                    if (IsMissing(byContextPairs[by], by, byOptions[by]))
                        leftBys.Remove(by);
                }
                if (!leftBys.Any())
                {
                    leftBys = byContextPairs.Keys.Except(currentByArray).Where(by => !IsMissing(byContextPairs[by], by, byOptions[by])).ToList();
                    if (!leftBys.Any())
                        return true;
                }
                return false;
            };

            TimeSpan maxTimeout = byOptions.Values.Max(x => x.Timeout.Value);
            TimeSpan maxRetryInterval = byOptions.Values.Max(x => x.RetryInterval.Value);

            bool isMissing = Until(findFunction, maxTimeout, maxRetryInterval);

            if (byOptions.Values.Any(x => x.ThrowOnFail) && !isMissing)
            {
                throw ExceptionFactory.CreateForNotMissingElement(
                    byOptions[leftBys.First()].GetNameWithKind(),
                    leftBys.First());
            }
            else
            {
                return isMissing;
            }
        }

        private static bool IsMissing(ISearchContext context, By by, ByOptions options)
        {
            return !context.FindElements(by).Where(CreateVisibilityPredicate(options.Visibility)).Any();
        }

        private IWait<T> CreateWait(TimeSpan timeout, TimeSpan retryInterval)
        {
            IWait<T> wait = new SafeWait<T>(Context)
            {
                Timeout = timeout,
                PollingInterval = retryInterval
            };
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NotFoundException));
            return wait;
        }
    }
}
