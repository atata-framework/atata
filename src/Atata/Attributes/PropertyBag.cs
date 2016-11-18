using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public class PropertyBag
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        internal UIComponentMetadata Metadata { get; set; }

        public object this[string name]
        {
            get { return values[name]; }
            set { values[name] = value; }
        }

        public bool Contains(string name)
        {
            return values.ContainsKey(name);
        }

        public T Get<T>(string name, params Func<UIComponentMetadata, IEnumerable<ISettingsAttribute>>[] settingsAttributesGetters)
        {
            return Get(name, default(T), settingsAttributesGetters);
        }

        public T Get<T>(string name, T defaultValue, params Func<UIComponentMetadata, IEnumerable<ISettingsAttribute>>[] settingsAttributesGetters)
        {
            object value;

            if (values.TryGetValue(name, out value))
                return (T)value;

            if (Metadata != null && settingsAttributesGetters.Any())
            {
                ISettingsAttribute settingsAttribute = settingsAttributesGetters.
                    SelectMany(x => x(Metadata)).
                    FirstOrDefault(x => x.Properties.Contains(name));

                if (settingsAttribute != null)
                    return (T)settingsAttribute.Properties[name];
            }

            return defaultValue;
        }
    }
}
