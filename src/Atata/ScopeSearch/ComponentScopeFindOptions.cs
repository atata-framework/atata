using System;

namespace Atata
{
    /// <summary>
    /// Represents the options of UI component scope element finding.
    /// </summary>
    public class ComponentScopeFindOptions : ICloneable
    {
        public UIComponent Component { get; private set; }

        public UIComponentMetadata Metadata { get; private set; }

        public string ElementXPath { get; private set; }

        public string[] Terms { get; set; }

        public string OuterXPath { get; set; }

        public int? Index { get; set; }

        public TermMatch Match { get; set; }

        public static ComponentScopeFindOptions Create(UIComponent component, UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ControlDefinitionAttribute definition = metadata.ComponentDefinitionAttribute as ControlDefinitionAttribute;

            int index = findAttribute.Index;

            ComponentScopeFindOptions options = new ComponentScopeFindOptions
            {
                Component = component,
                Metadata = metadata,
                ElementXPath = definition?.ScopeXPath ?? ScopeDefinitionAttribute.DefaultScopeXPath,
                Index = index >= 0 ? (int?)index : null,
                OuterXPath = findAttribute.OuterXPath
            };

            if (findAttribute is ITermFindAttribute termFindAttribute)
                options.Terms = termFindAttribute.GetTerms(metadata);

            if (findAttribute is ITermMatchFindAttribute termMatchFindAttribute)
                options.Match = termMatchFindAttribute.GetTermMatch(metadata);

            return options;
        }

        public string GetTermsAsString() =>
            Terms != null ? string.Join("/", Terms) : null;

        /// <inheritdoc cref="Clone"/>
        object ICloneable.Clone() =>
            Clone();

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public ComponentScopeFindOptions Clone() =>
            (ComponentScopeFindOptions)MemberwiseClone();
    }
}
