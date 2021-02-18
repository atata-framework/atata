using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    [Obsolete("Should be removed in v2.0.0")]
    public class ComponentScopeLocateStrategyTests : UITestFixture
    {
        [Test]
        public void ComponentScopeLocateStrategy_Custom()
        {
            var page = Go.To<InputPage>();
            var control = page.Controls.Create<TextInput<InputPage>>(
                "test",
                new FindByIdAttribute("text-input") { Strategy = typeof(CustomComponentScopeLocateStrategy) });

            control.Should.BeVisible();
        }

        public class CustomComponentScopeLocateStrategy : IComponentScopeLocateStrategy
        {
            public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
            {
                ComponentScopeXPathBuilder builder = new ComponentScopeXPathBuilder(options);

                string xPath = builder.
                    WrapWithIndex(x => x.OuterXPath.Any[y => y.TermsConditionOf("id")]).
                    Self.ComponentXPath;

                return new XPathComponentScopeFindResult(xPath, scope, searchOptions);
            }
        }
    }
}
