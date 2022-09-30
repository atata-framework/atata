using System;

namespace Atata
{
    /// <summary>
    /// Represents the interface of provider of data of <typeparamref name="TData"/> type owned by <typeparamref name="TOwner"/> object.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    [Obsolete("Use IObjectProvider<TObject, TOwner> instead.")] // Obsolete since v2.0.0.
    public interface IDataProvider<out TData, out TOwner> : IObjectProvider<TData, TOwner>
    {
    }
}
