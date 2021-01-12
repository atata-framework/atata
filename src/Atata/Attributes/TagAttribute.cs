using System;
using System.Collections.ObjectModel;

namespace Atata
{
    /// <summary>
    /// Specifies the tag(s) of the component.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface,
        AllowMultiple = true)]
    public class TagAttribute : Attribute
    {
        public TagAttribute(params string[] values)
        {
            Values = new ReadOnlyCollection<string>(values ?? new string[0]);
        }

        /// <summary>
        /// Gets the tag values.
        /// </summary>
        public ReadOnlyCollection<string> Values { get; }
    }
}
