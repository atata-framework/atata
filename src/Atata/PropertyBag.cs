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

        public T Get<T>(string name, params Func<UIComponentMetadata, IEnumerable<IPropertySettings>>[] propertySettingsGetters)
        {
            return Get(name, default(T), propertySettingsGetters);
        }

        public T Get<T>(string name, T defaultValue, params Func<UIComponentMetadata, IEnumerable<IPropertySettings>>[] propertySettingsGetters)
        {
            object value;

            if (values.TryGetValue(name, out value))
                return (T)value;

            if (Metadata != null && propertySettingsGetters.Any())
            {
                IPropertySettings propertySettings = propertySettingsGetters.
                    SelectMany(x => x(Metadata)).
                    FirstOrDefault(x => x.Properties.Contains(name));

                if (propertySettings != null)
                    return (T)propertySettings.Properties[name];
            }

            return defaultValue;
        }
    }
}
