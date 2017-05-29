using System;

namespace Atata
{
    /// <summary>
    /// Defines the settings to apply for the specified finding strategy of a control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FindSettingsAttribute : Attribute, IPropertySettings
    {
        public FindSettingsAttribute()
        {
        }

        public FindSettingsAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public FindSettingsAttribute(Type findAttributeType)
        {
            FindAttributeType = findAttributeType;
        }

        public PropertyBag Properties { get; } = new PropertyBag();

        /// <summary>
        /// Gets the type of the attribute to use for the control finding. Type should be inherited from <see cref="FindAttribute"/>.
        /// </summary>
        public Type FindAttributeType { get; private set; }

        /// <summary>
        /// Gets or sets the index of the control. The default value is -1, meaning that the index is not used.
        /// </summary>
        public int Index
        {
            get { return Properties.Get(nameof(Index), -1); }
            set { Properties[nameof(Index)] = value; }
        }

        /// <summary>
        /// Gets or sets the scope source. The default value is Parent.
        /// </summary>
        public ScopeSource ScopeSource
        {
            get { return Properties.Get(nameof(ScopeSource), ScopeSource.Parent); }
            set { Properties[nameof(ScopeSource)] = value; }
        }

        /// <summary>
        /// Gets or sets the outer XPath. The default value is null.
        /// </summary>
        public string OuterXPath
        {
            get { return Properties.Get<string>(nameof(OuterXPath)); }
            set { Properties[nameof(OuterXPath)] = value; }
        }

        /// <summary>
        /// Gets or sets the strategy type for the control search. Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>. The default value is null, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
        /// </summary>
        public Type Strategy
        {
            get { return Properties.Get<Type>(nameof(Strategy)); }
            set { Properties[nameof(Strategy)] = value; }
        }

        /// <summary>
        /// Gets or sets the visibility. The default value is Visible.
        /// </summary>
        public Visibility Visibility
        {
            get { return Properties.Get(nameof(Visibility), Visibility.Visible); }
            set { Properties[nameof(Visibility)] = value; }
        }
    }
}
