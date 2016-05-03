using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public abstract class UIComponent
    {
        private IWebElement cachedScope;

        protected UIComponent()
        {
            Children = new List<UIComponent>();
        }

        protected internal UIComponent Owner { get; internal set; }
        protected internal UIComponent Parent { get; internal set; }
        protected internal List<UIComponent> Children { get; private set; }

        protected internal ILogManager Log { get; internal set; }
        protected internal RemoteWebDriver Driver { get; internal set; }

        protected internal ScopeSource ScopeSource { get; internal set; }
        protected internal IScopeLocator ScopeLocator { get; internal set; }
        protected internal bool CacheScopeElement { get; set; }
        protected internal string ComponentName { get; internal set; }
        protected internal string ComponentTypeName { get; internal set; }

        protected internal string ComponentFullName
        {
            get { return string.Format("\"{0}\" {1}", ComponentName, ComponentTypeName ?? "component"); }
        }

        protected internal UIComponentMetadata Metadata { get; internal set; }

        protected internal TriggerAttribute[] Triggers { get; internal set; }

        protected internal IWebElement Scope
        {
            get
            {
                if (CacheScopeElement && cachedScope != null)
                    return cachedScope;
                else
                    return GetScopeElement();
            }
        }

        protected IWebElement GetScopeElement(SearchOptions searchOptions = null)
        {
            if (ScopeLocator == null)
                throw new InvalidOperationException("ScopeLocator is missing.");

            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            IWebElement element = ScopeLocator.GetElement(searchOptions);
            if (!searchOptions.IsSafely && element == null)
                throw ExceptionFactory.CreateForNoSuchElement(ComponentFullName);

            if (CacheScopeElement)
                this.cachedScope = element;

            return element;
        }

        protected internal virtual void ApplyMetadata(UIComponentMetadata metadata)
        {
        }

        public bool Exists()
        {
            return ScopeLocator.GetElement(SearchOptions.Safely()) != null;
        }

        public bool Missing()
        {
            return ScopeLocator.IsMissing(SearchOptions.Safely());
        }

        public void VerifyExists()
        {
            Log.StartVerificationSection("{0} exists", ComponentFullName);
            GetScopeElement();
            Log.EndSection();
        }

        public void VerifyMissing()
        {
            Log.StartVerificationSection("{0} is missing", ComponentFullName);
            ScopeLocator.IsMissing();
            Log.EndSection();
        }

        protected internal void VerifyContent(string[] content, TermMatch match = TermMatch.Equals)
        {
            if (content == null)
                throw new ArgumentNullException("content");
            if (content.Length == 0)
                throw ExceptionFactory.CreateForArgumentEmptyCollection("content");

            string matchAsString = match.ToString(TermFormat.LowerCase);
            string expectedValuesAsString = TermResolver.ToDisplayString(content);

            Log.StartVerificationSection("{0} content {1} '{2}'", ComponentFullName, matchAsString, expectedValuesAsString);

            string actualText = null;

            bool containsText = Driver.Try().Until(_ =>
            {
                actualText = Scope.Text;
                return match.IsMatch(actualText, content);
            });

            if (!containsText)
            {
                string errorMessage = DefaultAsserter.BuildAssertionErrorMessage(
                    "String that {0} \"{1}\"".FormatWith(matchAsString, expectedValuesAsString),
                    string.Format("\"{0}\"", actualText),
                    "{0} content doesn't match criteria", ComponentFullName);

                Assert.That(containsText, errorMessage);
            }

            Log.EndSection();
        }

        protected internal void VerifyContentContainsAll(params string[] content)
        {
            if (content == null)
                throw new ArgumentNullException("content");
            if (content.Length == 0 || content.All(x => string.IsNullOrEmpty(x)))
                throw ExceptionFactory.CreateForArgumentEmptyCollection("content");

            string[] expectedValues = content.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            string matchAsString = TermMatch.Contains.ToString(TermFormat.LowerCase);
            string expectedValuesAsString = expectedValues.ToQuotedValuesListOfString();

            Log.StartVerificationSection("{0} content {1} {2}", ComponentFullName, matchAsString, expectedValuesAsString);

            string actualText = null;
            string notFoundValue = null;

            bool containsText = Driver.Try().Until(_ =>
            {
                actualText = Scope.Text;
                notFoundValue = expectedValues.FirstOrDefault(value => !actualText.Contains(value));
                return notFoundValue == null;
            });

            if (!containsText)
            {
                string errorMessage = DefaultAsserter.BuildAssertionErrorMessage(
                    "String that {0} \"{1}\"".FormatWith(matchAsString, notFoundValue),
                    string.Format("\"{0}\"", actualText),
                    "{0} content doesn't match criteria", ComponentFullName);

                Assert.That(containsText, errorMessage);
            }

            Log.EndSection();
        }
    }
}
