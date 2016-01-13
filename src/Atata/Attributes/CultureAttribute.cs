using System;
using System.Globalization;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Assembly)]
    public class CultureAttribute : Attribute
    {
        public CultureAttribute(string value = null)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public bool HasValue
        {
            get { return !string.IsNullOrWhiteSpace(Value); }
        }

        public CultureInfo GetCultureInfo()
        {
            return HasValue ? new CultureInfo(Value) : CultureInfo.CurrentCulture;
        }
    }
}
