using System;

namespace Atata
{
    /// <summary>
    /// Represents the provider of UI component data of <typeparamref name="TData"/> type
    /// owned by <typeparamref name="TOwner"/> object.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    [Obsolete("Use ValueProvider<TValue, TOwner> instead.")] // Obsolete since v2.0.0.
    public class DataProvider<TData, TOwner> : ValueProvider<TData, TOwner>, IDataProvider<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider{TData, TOwner}"/> class.
        /// </summary>
        /// <param name="component">The associated component.</param>
        /// <param name="valueGetFunction">The function that gets the value.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DataProvider(UIComponent<TOwner> component, Func<TData> valueGetFunction, string providerName)
            : base(component.Owner, DynamicObjectSource.Create(valueGetFunction), providerName)
        {
        }
    }
}
