using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Provides a functionality for object creation.
    /// </summary>
    public interface IObjectCreator
    {
        /// <summary>
        /// Creates an instance of specified type with a set of named values (constructor or property values).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="valuesMap">The values map.</param>
        /// <returns>An instance of created object.</returns>
        object Create(Type type, Dictionary<string, object> valuesMap);

        /// <summary>
        /// Creates an instance of specified type with a set of named values (constructor or property values).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="valuesMap">The values map.</param>
        /// <param name="alternativeParameterNamesMap">The map of alternative parameter names.</param>
        /// <returns>An instance of created object.</returns>
        object Create(Type type, Dictionary<string, object> valuesMap, Dictionary<string, string> alternativeParameterNamesMap);
    }
}
