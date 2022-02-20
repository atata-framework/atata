using System;

namespace Atata
{
    /// <summary>
    /// Provides a set of methods for <see cref="DynamicObjectSource{TValue}"/> creation.
    /// </summary>
    public static class DynamicObjectSource
    {
        /// <summary>
        /// Creates a <see cref="DynamicObjectSource{TValue}"/> for the specified <paramref name="valueGetFunction"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="valueGetFunction">The value get function.</param>
        /// <returns>An instance of <see cref="DynamicObjectSource{TValue}"/>.</returns>
        public static DynamicObjectSource<TValue> Create<TValue>(Func<TValue> valueGetFunction) =>
            new DynamicObjectSource<TValue>(valueGetFunction);
    }
}
