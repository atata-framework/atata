namespace Atata
{
    /// <summary>
    /// Represents the interface of provider of data of <typeparamref name="TData"/> type owned by <typeparamref name="TOwner"/> object.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public interface IDataProvider<out TData, TOwner>
    {
        /// <summary>
        /// Gets the associated component.
        /// </summary>
        UIComponent Component { get; }

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Gets the owner object.
        /// </summary>
        TOwner Owner { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        TData Value { get; }

        /// <summary>
        /// Gets the value term options.
        /// </summary>
        // TODO: Extract ValueTermOptions to another interface.
        TermOptions ValueTermOptions { get; }
    }
}
