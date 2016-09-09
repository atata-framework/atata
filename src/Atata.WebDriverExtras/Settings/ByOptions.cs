using System;

namespace Atata
{
    public class ByOptions
    {
        protected ByOptions()
        {
        }

        public string Name { get; set; }

        public string Kind { get; set; }

        public TimeSpan? Timeout { get; set; }

        public TimeSpan? RetryInterval { get; set; }

        public ElementVisibility Visibility { get; set; }

        public bool ThrowOnFail { get; set; }

        public static ByOptions CreateDefault()
        {
            return new ByOptions
            {
                Timeout = null,
                RetryInterval = null,
                Visibility = ElementVisibility.Visible,
                ThrowOnFail = true
            };
        }

        public string GetNameWithKind()
        {
            bool hasName = !string.IsNullOrWhiteSpace(Name);
            bool hasKind = !string.IsNullOrWhiteSpace(Kind);

            if (hasName && hasKind)
                return $"\"{Name}\" {Kind}";
            else if (hasName)
                return Name;
            else if (hasKind)
                return Kind;
            else
                return null;
        }
    }
}
