namespace Atata;

/// <summary>
/// Defines the settings to apply for the <see cref="VerifyTitleAttribute"/> trigger.
/// </summary>
public class VerifyTitleSettingsAttribute : TermSettingsAttribute
{
    public VerifyTitleSettingsAttribute()
    {
    }

    public VerifyTitleSettingsAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public VerifyTitleSettingsAttribute(TermMatch match)
        : base(match)
    {
    }

    public VerifyTitleSettingsAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }
}
