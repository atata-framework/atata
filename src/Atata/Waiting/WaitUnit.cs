namespace Atata
{
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
                switch (SearchOptions.Visibility)
                {
                    case Visibility.Visible:
                        return "visible";
                    case Visibility.Hidden:
                        return "hidden";
                    case Visibility.Any:
                        return "visible or hidden";
                    default:
                        return null;
                }
            }
            else
            {
                switch (SearchOptions.Visibility)
                {
                    case Visibility.Visible:
                        return "missing or hidden";
                    case Visibility.Hidden:
                        return null;
                    case Visibility.Any:
                        return "missing";
                    default:
                        return null;
                }
            }
        }
    }
}
