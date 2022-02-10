namespace Atata
{
    /// <summary>
    /// Represents the interface of provider of data of <typeparamref name="TData"/> type owned by <typeparamref name="TOwner"/> object.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public interface IDataProvider<out TData, out TOwner> : IObjectProvider<TData>
    {
        /// <summary>
        /// Gets the associated component.
        /// </summary>
        UIComponent Component { get; }

        /// <summary>
        /// Gets the owner object.
        /// </summary>
        TOwner Owner { get; }
    }
}
