using System;

namespace Atata
{
    public class ComponentScopeLocateOptions : ICloneable
    {
        public UIComponentMetadata Metadata { get; set; }

        public string[] Terms { get; set; }

        public string OuterXPath { get; set; }

        public string ElementXPath { get; set; }

        public int? Index { get; set; }

        public TermMatch Match { get; set; }

        public Visibility Visibility { get; set; }

        public static ComponentScopeLocateOptions Create(UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ControlDefinitionAttribute definition = metadata.ComponentDefinitionAttribute as ControlDefinitionAttribute;

            int index = findAttribute.Index;

            ComponentScopeLocateOptions options = new ComponentScopeLocateOptions
            {
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

        public ComponentScopeLocateOptions Clone()
        {
            return (ComponentScopeLocateOptions)MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
    }
}
