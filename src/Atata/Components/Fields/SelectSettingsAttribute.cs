using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectSettingsAttribute : Attribute
    {
        public SelectSettingsAttribute()
            : this(SelectSelectionKind.ByText)
        {
        }

        public SelectSettingsAttribute(SelectSelectionKind by)
        {
            By = by;
        }

        public SelectSelectionKind By { get; private set; }
    }
}
