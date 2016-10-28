using System;

namespace Atata
{
    public class ComponentScopeXPathBuilder : XPathBuilder<ComponentScopeXPathBuilder>
    {
        public ComponentScopeXPathBuilder(ComponentScopeLocateOptions options)
        {
            Options = options;
        }

        public ComponentScopeLocateOptions Options { get; private set; }

        public ComponentScopeXPathBuilder ComponentXPath
        {
            get { return _(Options.ElementXPath); }
        }

        public ComponentScopeXPathBuilder TermsConditionOfContent
        {
            get { return _(Options.Match.CreateXPathCondition(Options.Terms)); }
        }

        public static implicit operator string(ComponentScopeXPathBuilder builder)
        {
            return builder.XPath;
        }

        public ComponentScopeXPathBuilder TermsConditionOf(string attributeName)
        {
            return _(Options.Match.CreateXPathCondition(Options.Terms, "@" + attributeName));
        }

        public ComponentScopeXPathBuilder WrapWithIndex(Func<ComponentScopeXPathBuilder, string> buildFunction)
        {
            string subPath = CreateSubPath(buildFunction);

            return Options.Index.HasValue
                ? _($"({subPath})[{Options.Index + 1}]")
                : _(subPath);
        }

        protected override ComponentScopeXPathBuilder CreateInstance()
        {
            return new ComponentScopeXPathBuilder(Options);
        }
    }
}
