using System;

namespace Atata
{
    public static class UntilExtensions
    {
        public static WaitUnit[] GetWaitUnits(this Until until, WaitOptions options = null)
        {
            options ??= new WaitOptions();

            return until switch
            {
                Until.Missing => new[]
                    {
                        CreateAbsenceUnit(Visibility.Any, until, options)
                    },
                Until.Hidden => new[]
                    {
                        CreatePresenceUnit(Visibility.Hidden, until, options)
                    },
                Until.MissingOrHidden => new[]
                    {
                        CreateAbsenceUnit(Visibility.Visible, until, options)
                    },
                Until.Visible => new[]
                    {
                        CreatePresenceUnit(Visibility.Visible, until, options)
                    },
                Until.VisibleOrHidden => new[]
                    {
                        CreatePresenceUnit(Visibility.Any, until, options)
                    },
                Until.VisibleThenHidden => new[]
                    {
                        CreatePresenceUnit(Visibility.Visible, until, options),
                        CreatePresenceUnit(Visibility.Hidden, until, options)
                    },
                Until.VisibleThenMissing => new[]
                    {
                        CreatePresenceUnit(Visibility.Visible, until, options),
                        CreateAbsenceUnit(Visibility.Any, until, options)
                    },
                Until.VisibleThenMissingOrHidden => new[]
                    {
                        CreatePresenceUnit(Visibility.Visible, until, options),
                        CreateAbsenceUnit(Visibility.Visible, until, options)
                    },
                Until.MissingThenVisible => new[]
                    {
                        CreateAbsenceUnit(Visibility.Any, until, options),
                        CreatePresenceUnit(Visibility.Visible, until, options)
                    },
                Until.HiddenThenVisible => new[]
                    {
                        CreatePresenceUnit(Visibility.Hidden, until, options),
                        CreatePresenceUnit(Visibility.Visible, until, options)
                    },
                Until.MissingOrHiddenThenVisible => new[]
                    {
                        CreateAbsenceUnit(Visibility.Visible, until, options),
                        CreatePresenceUnit(Visibility.Visible, until, options)
                    },
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
}
