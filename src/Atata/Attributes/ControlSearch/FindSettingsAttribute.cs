using System;

namespace Atata
{
    /// <summary>
    /// Defines the settings to apply for the specified finding strategy of a control.
    /// Adds to or overrides properties of <see cref="FindAttribute"/>.
    /// </summary>
    public class FindSettingsAttribute : AttributeSettingsAttribute
    {
        public FindSettingsAttribute()
        {
        }

        [Obsolete(
            "Use default constructor instead and set target attribute via TargetAttributeType property, " +
            "e.g.: [FindSettings(TargetAttributeType = typeof(FindByNameAttribute))].")] // Obsolete since v1.0.0.
        public FindSettingsAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        [Obsolete(
            "Use default constructor instead and set target attribute via TargetAttributeType property, " +
            "e.g.: [FindSettings(TargetAttributeType = typeof(FindByNameAttribute))].")] // Obsolete since v1.0.0.
        public FindSettingsAttribute(Type findAttributeType)
        {
            TargetAttributeType = findAttributeType;
        }

        /// <summary>
        /// Gets the type of the attribute to use for the control finding.
        /// Type should be inherited from <see cref="FindAttribute"/>.
        /// </summary>
        [Obsolete("Use TargetAttributeType instead.")] // Obsolete since v1.0.0.
        public Type FindAttributeType => TargetAttributeType;

        /// <summary>
        /// Gets or sets the index of the control.
        /// The default value is <c>-1</c>, meaning that the index is not used.
        /// </summary>
        public int Index
        {
            get { return Properties.Get(nameof(Index), -1); }
            set { Properties[nameof(Index)] = value; }
        }

        /// <summary>
        /// Gets or sets the visibility.
        /// The default value is <see cref="Visibility.Visible"/>.
        /// </summary>
        public Visibility Visibility
        {
            get { return Properties.Get(nameof(Visibility), Visibility.Visible); }
            set { Properties[nameof(Visibility)] = value; }
        }

        /// <summary>
        /// Gets or sets the scope source.
        /// The default value is <see cref="ScopeSource.Parent"/>.
        /// </summary>
        public ScopeSource ScopeSource
        {
            get { return Properties.Get(nameof(ScopeSource), ScopeSource.Parent); }
            set { Properties[nameof(ScopeSource)] = value; }
        }

        /// <summary>
        /// Gets or sets the outer XPath.
        /// The default value is null, meaning that the control is searchable as descendant (using ".//" XPath) in scope source.
        /// </summary>
        public string OuterXPath
        {
            get { return Properties.Get<string>(nameof(OuterXPath)); }
            set { Properties[nameof(OuterXPath)] = value; }
        }

        /// <summary>
        /// Gets or sets the strategy type for the control search.
        /// Strategy type should implement <see cref="IComponentScopeFindStrategy"/>.
        /// The default value is <see langword="null"/>, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
        /// </summary>
        public Type Strategy
        {
            get { return Properties.Get<Type>(nameof(Strategy)); }
            set { Properties[nameof(Strategy)] = value; }
        }

        /// <summary>
        /// Gets or sets the element find timeout in seconds.
        /// The default value is taken from <see cref="AtataContext.ElementFindTimeout"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double Timeout
        {
            get => Properties.Get<double?>(nameof(Timeout))
                ?? (AtataContext.Current?.ElementFindTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => Properties[nameof(Timeout)] = value;
        }

        /// <summary>
        /// Gets or sets the element find retry interval in seconds.
        /// The default value is taken from <see cref="AtataContext.ElementFindRetryInterval"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double RetryInterval
        {
            get => Properties.Get<double?>(nameof(RetryInterval))
                ?? (AtataContext.Current?.ElementFindRetryInterval ?? RetrySettings.Interval).TotalSeconds;
            set => Properties[nameof(RetryInterval)] = value;
        }
    }
}
