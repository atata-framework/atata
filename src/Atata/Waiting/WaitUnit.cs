namespace Atata;

public class WaitUnit
{
    public enum WaitMethod
    {
        Presence,
        Absence
    }

    public WaitMethod Method { get; set; }

    public Until Until { get; set; }

    public SearchOptions SearchOptions { get; set; }

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
                _ => null
            };
        }
        else
        {
            return SearchOptions.Visibility switch
            {
                Visibility.Visible => "missing or hidden",
                Visibility.Hidden => null,
                Visibility.Any => "missing",
                _ => null
            };
        }
    }
}
