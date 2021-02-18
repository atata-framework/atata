using System;

namespace Atata
{
    public class ComponentScopeLocateOptions : ICloneable
    {
        public UIComponent Component { get; private set; }

        public UIComponentMetadata Metadata { get; set; }

        public string[] Terms { get; set; }

        public string OuterXPath { get; set; }

        public string ElementXPath { get; set; }

        public int? Index { get; set; }

        public TermMatch Match { get; set; }

        // TODO: Probably remove, as Visibility is present in SearchOptions.
        public Visibility Visibility { get; set; }

        public static ComponentScopeLocateOptions Create(UIComponent component, UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ControlDefinitionAttribute definition = metadata.ComponentDefinitionAttribute as ControlDefinitionAttribute;

            int index = findAttribute.Index;

            ComponentScopeLocateOptions options = new ComponentScopeLocateOptions
            {
                Component = component,
                Metadata = metadata,
                ElementXPath = definition?.ScopeXPath ?? ScopeDefinitionAttribute.DefaultScopeXPath,
                Index = index >= 0 ? (int?)index : null,
                Visibility = findAttribute.Visibility,
                OuterXPath = findAttribute.OuterXPath
            };

            if (findAttribute is ITermFindAttribute termFindAttribute)
                options.Terms = termFindAttribute.GetTerms(metadata);

            if (findAttribute is ITermMatchFindAttribute termMatchFindAttribute)
                options.Match = termMatchFindAttribute.GetTermMatch(metadata);

            return options;
        }

        public string GetTermsAsString()
        {
            return Terms != null ? string.Join("/", Terms) : null;
        }

        /// <inheritdoc cref="Clone"/>
        object ICloneable.Clone() =>
            Clone();

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public ComponentScopeLocateOptions Clone()
        {
            return (ComponentScopeLocateOptions)MemberwiseClone();
        }
    }
}
