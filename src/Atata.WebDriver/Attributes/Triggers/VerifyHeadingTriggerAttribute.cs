namespace Atata.WebDriver;

public abstract class VerifyHeadingTriggerAttribute : TermVerificationTriggerAttribute
{
    protected VerifyHeadingTriggerAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    protected VerifyHeadingTriggerAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    protected VerifyHeadingTriggerAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    protected VerifyHeadingTriggerAttribute(params string[] values)
        : base(values)
    {
    }

    /// <summary>
    /// Gets or sets the index of header.
    /// The default value is <c>-1</c>, meaning that the index is not used.
    /// </summary>
    public int Index { get; set; } = -1;

    protected void Verify<TH, TOwner>(TriggerContext<TOwner> context, string[] values)
        where TH : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        var metadata = context.Component.Metadata;
        var match = ResolveMatch(metadata);

        if (Index >= 0)
        {
            var headingControl = context.Component.Owner.Find<TH>(
                new FindByIndexAttribute(Index).Visible());

            headingControl.Should.WithinSeconds(Timeout, RetryInterval).MatchAny(match, values);
        }
        else
        {
            var headingControl = context.Component.Owner.Find<TH>(
                FormatComponentName(match, values),
                new FindByContentAttribute(match, values) { Visibility = Visibility.Visible });

            headingControl.Should.WithinSeconds(Timeout, RetryInterval).BePresent();
        }
    }

    private static string FormatComponentName(TermMatch match, string[] values)
    {
        var format = match switch
        {
            TermMatch.Contains => "Containing '{0}'",
            TermMatch.Equals => "{0}",
            TermMatch.StartsWith => "Starting with '{0}'",
            TermMatch.EndsWith => "Ending with '{0}'",
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(match)
        };
        string combinedValues = TermResolver.ToDisplayString(values);

        return string.Format(format, combinedValues);
    }
}
