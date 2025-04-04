﻿namespace Atata;

public class ComponentScopeXPathBuilder : XPathBuilder<ComponentScopeXPathBuilder>
{
    public ComponentScopeXPathBuilder(ComponentScopeFindOptions options) =>
        Options = options;

    public ComponentScopeFindOptions Options { get; private set; }

    public ComponentScopeXPathBuilder OuterXPath =>
        Options.OuterXPath != null ? _(Options.OuterXPath) : Descendant;

    public ComponentScopeXPathBuilder ComponentXPath =>
        _(Options.ElementXPath);

    public ComponentScopeXPathBuilder TermsConditionOfContent =>
        _(Options.Match.CreateXPathCondition(Options.Terms));

    public ComponentScopeXPathBuilder TermsConditionOfText =>
        _(Options.Match.CreateXPathCondition(Options.Terms, "text()"));

    [return: NotNullIfNotNull(nameof(builder))]
    public static implicit operator string?(ComponentScopeXPathBuilder? builder) =>
        builder?.XPath;

    public ComponentScopeXPathBuilder TermsConditionOf(string attributeName) =>
        _(Options.Match.CreateXPathCondition(Options.Terms, "@" + attributeName));

    public ComponentScopeXPathBuilder WrapWithIndex(Func<ComponentScopeXPathBuilder, string> buildFunction)
    {
        string subPath = CreateSubPath(buildFunction);

        if (Options.Index.HasValue)
        {
            subPath = subPath[0] == '(' && subPath[^1] == ')'
                ? subPath
                : $"({subPath})";

            return _($"{subPath}[{Options.Index + 1}]");
        }
        else
        {
            return _(subPath);
        }
    }

    protected override ComponentScopeXPathBuilder CreateInstance() =>
        new(Options);
}
