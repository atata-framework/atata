using System;
using OpenQA.Selenium;

namespace Atata
{
    public abstract class WaitForAttribute : TriggerAttribute
    {
        public WaitForAttribute(By by, WaitUntil until, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
            Until = until;
        }

        protected enum WaitMethod
        {
            Presence,
            Absence
        }

        public WaitUntil Until { get; private set; }

        public bool ThrowOnPresenceFailure { get; set; } = true;

        public bool ThrowOnAbsenceFailure { get; set; } = true;

        public double PresenceTimeout { get; set; } = RetrySettings.Timeout.TotalSeconds;

        public double AbsenceTimeout { get; set; } = RetrySettings.Timeout.TotalSeconds;

        public double RetryInterval { get; set; } = RetrySettings.RetryInterval.TotalSeconds;

        protected WaitUnit[] GetWaitUnits(WaitUntil until)
        {
            switch (until)
            {
                case WaitUntil.Missing:
                    return new[]
                    {
                        CreateAbsenceUnit(ElementVisibility.Any)
                    };
                case WaitUntil.Hidden:
                    return new[]
                    {
                        CreatePresenceUnit(ElementVisibility.Invisible)
                    };
                case WaitUntil.MissingOrHidden:
                    return new[]
                    {
                        CreateAbsenceUnit(ElementVisibility.Visible)
                    };
                case WaitUntil.Visible:
                    return new[]
                    {
                        CreatePresenceUnit(ElementVisibility.Visible)
                    };
                case WaitUntil.Exists:
                    return new[]
                    {
                        CreatePresenceUnit(ElementVisibility.Any)
                    };
                case WaitUntil.VisibleAndHidden:
                    return new[]
                    {
                        CreatePresenceUnit(ElementVisibility.Visible),
                        CreatePresenceUnit(ElementVisibility.Invisible)
                    };
                case WaitUntil.VisibleAndMissing:
                    return new[]
                    {
                        CreatePresenceUnit(ElementVisibility.Visible),
                        CreateAbsenceUnit(ElementVisibility.Any)
                    };
                case WaitUntil.MissingAndVisible:
                    return new[]
                    {
                        CreateAbsenceUnit(ElementVisibility.Any),
                        CreatePresenceUnit(ElementVisibility.Visible)
                    };
                case WaitUntil.HiddenAndVisible:
                    return new[]
                    {
                        CreatePresenceUnit(ElementVisibility.Invisible),
                        CreatePresenceUnit(ElementVisibility.Visible)
                    };
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(until, nameof(until));
            }
        }

        private WaitUnit CreatePresenceUnit(ElementVisibility visibility)
        {
            return new WaitUnit
            {
                Method = WaitMethod.Presence,
                Options = new SearchOptions
                {
                    Timeout = TimeSpan.FromSeconds(PresenceTimeout),
                    RetryInterval = TimeSpan.FromSeconds(RetryInterval),
                    Visibility = visibility,
                    IsSafely = !ThrowOnPresenceFailure
                }
            };
        }

        private WaitUnit CreateAbsenceUnit(ElementVisibility visibility)
        {
            return new WaitUnit
            {
                Method = WaitMethod.Absence,
                Options = new SearchOptions
                {
                    Timeout = TimeSpan.FromSeconds(AbsenceTimeout),
                    RetryInterval = TimeSpan.FromSeconds(RetryInterval),
                    Visibility = visibility,
                    IsSafely = !ThrowOnAbsenceFailure
                }
            };
        }

        protected abstract void Wait(IWebElement scopeElement, WaitUnit[] waitUnits);

        protected class WaitUnit
        {
            public WaitMethod Method { get; set; }

            public SearchOptions Options { get; set; }
        }
    }
}
