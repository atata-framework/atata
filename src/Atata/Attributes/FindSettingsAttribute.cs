using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FindSettingsAttribute : Attribute
    {
        public FindSettingsAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public FindSettingsAttribute(Type findAttributeType)
        {
            FindAttributeType = findAttributeType;
        }

        /// <summary>
        /// Gets the type of the attribute to use for the control search. Type should be inherited from <see cref="FindAttribute"/>.
        /// </summary>
        public Type FindAttributeType { get; private set; }

        /// <summary>
        /// Gets or sets the index of the control. The default value is -1, meaning that the index is not used.
        /// </summary>
        public int Index { get; set; } = -1;

        /// <summary>
        /// Gets or sets the scope source. The default value is Inherit.
        /// </summary>
        public ScopeSource Scope { get; set; }

        /// <summary>
        /// Gets or sets the strategy type for the control search. Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>. The default value is null, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
        /// </summary>
        public Type Strategy { get; set; }
    }
}
