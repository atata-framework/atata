using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableSettingsAttribute : Attribute
    {
        public TableSettingsAttribute()
        {
        }

        public int ColumnIndexToClick { get; set; }
    }
}
