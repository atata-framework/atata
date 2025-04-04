namespace Atata;

public sealed class WaitUnit
{
    public enum WaitMethod
    {
        Presence,
        Absence
    }

    public required WaitMethod Method { get; init; }

    public required Until Until { get; init; }

    public required SearchOptions SearchOptions { get; init; }

    internal string GetWaitingText()
    {
        string untilText = GetUntilText();
        string throwWord = SearchOptions.IsSafely ? "without" : "with";

        return $"{untilText} within {SearchOptions.Timeout.ToShortIntervalString()} {throwWord} throw on failure with {SearchOptions.RetryInterval.ToShortIntervalString()} retry interval";
    }

    private string GetUntilText()
    {
        if (Method == WaitMethod.Presence)
        {
            return SearchOptions.Visibility switch
            {
                Visibility.Visible => "visible",
                Visibility.Hidden => "hidden",
                Visibility.Any => "visible or hidden",
                _ => string.Empty
            };
        }
        else
        {
            return SearchOptions.Visibility switch
            {
                Visibility.Visible => "missing or hidden",
                Visibility.Hidden => string.Empty,
                Visibility.Any => "missing",
                _ => string.Empty
            };
        }
    }
}
