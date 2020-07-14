using System;

namespace Atata
{
    public static class UntilExtensions
    {
        public static WaitUnit[] GetWaitUnits(this Until until, WaitOptions options = null)
        {
            options = options ?? new WaitOptions();

            switch (until)
            {
                case Until.Missing:
                    return new[]
                    {
                        CreateAbsenceUnit(Visibility.Any, until, options)
                    };
                case Until.Hidden:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Hidden, until, options)
                    };
                case Until.MissingOrHidden:
                    return new[]
                    {
                        CreateAbsenceUnit(Visibility.Visible, until, options)
                    };
                case Until.Visible:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Visible, until, options)
                    };
                case Until.VisibleOrHidden:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Any, until, options)
                    };
                case Until.VisibleThenHidden:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Visible, until, options),
                        CreatePresenceUnit(Visibility.Hidden, until, options)
                    };
                case Until.VisibleThenMissing:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Visible, until, options),
                        CreateAbsenceUnit(Visibility.Any, until, options)
                    };
                case Until.VisibleThenMissingOrHidden:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Visible, until, options),
                        CreateAbsenceUnit(Visibility.Visible, until, options)
                    };
                case Until.MissingThenVisible:
                    return new[]
                    {
                        CreateAbsenceUnit(Visibility.Any, until, options),
                        CreatePresenceUnit(Visibility.Visible, until, options)
                    };
                case Until.HiddenThenVisible:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Hidden, until, options),
                        CreatePresenceUnit(Visibility.Visible, until, options)
                    };
                case Until.MissingOrHiddenThenVisible:
                    return new[]
                    {
                        CreateAbsenceUnit(Visibility.Visible, until, options),
                        CreatePresenceUnit(Visibility.Visible, until, options)
                    };
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(until, nameof(until));
            }
        }

        private static WaitUnit CreatePresenceUnit(Visibility visibility, Until until, WaitOptions options)
        {
            return new WaitUnit
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
        }

        private static WaitUnit CreateAbsenceUnit(Visibility visibility, Until until, WaitOptions options)
        {
            return new WaitUnit
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
}
