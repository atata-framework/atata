namespace Atata;

public static class UntilExtensions
{
    public static WaitUnit[] GetWaitUnits(this Until until, WaitOptions options = null)
    {
        options ??= new WaitOptions();

        return until switch
        {
            Until.Missing =>
                [
                    CreateAbsenceUnit(Visibility.Any, until, options)
                ],
            Until.Hidden =>
                [
                    CreatePresenceUnit(Visibility.Hidden, until, options)
                ],
            Until.MissingOrHidden =>
                [
                    CreateAbsenceUnit(Visibility.Visible, until, options)
                ],
            Until.Visible =>
                [
                    CreatePresenceUnit(Visibility.Visible, until, options)
                ],
            Until.VisibleOrHidden =>
                [
                    CreatePresenceUnit(Visibility.Any, until, options)
                ],
            Until.VisibleThenHidden =>
                [
                    CreatePresenceUnit(Visibility.Visible, until, options),
                    CreatePresenceUnit(Visibility.Hidden, until, options)
                ],
            Until.VisibleThenMissing =>
                [
                    CreatePresenceUnit(Visibility.Visible, until, options),
                    CreateAbsenceUnit(Visibility.Any, until, options)
                ],
            Until.VisibleThenMissingOrHidden =>
                [
                    CreatePresenceUnit(Visibility.Visible, until, options),
                    CreateAbsenceUnit(Visibility.Visible, until, options)
                ],
            Until.MissingThenVisible =>
                [
                    CreateAbsenceUnit(Visibility.Any, until, options),
                    CreatePresenceUnit(Visibility.Visible, until, options)
                ],
            Until.HiddenThenVisible =>
                [
                    CreatePresenceUnit(Visibility.Hidden, until, options),
                    CreatePresenceUnit(Visibility.Visible, until, options)
                ],
            Until.MissingOrHiddenThenVisible =>
                [
                    CreateAbsenceUnit(Visibility.Visible, until, options),
                    CreatePresenceUnit(Visibility.Visible, until, options)
                ],
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(until, nameof(until))
        };
    }

    private static WaitUnit CreatePresenceUnit(Visibility visibility, Until until, WaitOptions options) =>
        new()
        {
            Method = WaitUnit.WaitMethod.Presence,
            Until = until,
            SearchOptions = new SearchOptions
            {
                Timeout = TimeSpan.FromSeconds(options.PresenceTimeout),
                RetryInterval = TimeSpan.FromSeconds(options.RetryInterval),
                Visibility = visibility,
                IsSafely = !options.ThrowOnPresenceFailure
            }
        };

    private static WaitUnit CreateAbsenceUnit(Visibility visibility, Until until, WaitOptions options) =>
        new()
        {
            Method = WaitUnit.WaitMethod.Absence,
            Until = until,
            SearchOptions = new SearchOptions
            {
                Timeout = TimeSpan.FromSeconds(options.AbsenceTimeout),
                RetryInterval = TimeSpan.FromSeconds(options.RetryInterval),
                Visibility = visibility,
                IsSafely = !options.ThrowOnAbsenceFailure
            }
        };
}
