using System;

namespace Atata
{
    public class ComponentScopeLocateOptions : ICloneable
    {
        public UIComponentMetadata Metadata { get; set; }

        public string[] Terms { get; set; }

        public string ElementXPath { get; set; }

        public int? Index { get; set; }

        public TermMatch Match { get; set; }

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
