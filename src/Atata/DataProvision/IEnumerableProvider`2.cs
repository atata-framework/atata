namespace Atata;

/// <summary>
/// Represents the interface of a value provider of <typeparamref name="TItem"/> enumerable value owned by <typeparamref name="TOwner"/> object.
/// </summary>
/// <typeparam name="TItem">The type of the enumerable item.</typeparam>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public interface IEnumerableProvider<out TItem, out TOwner> : IObjectProvider<IEnumerable<TItem>, TOwner>
{
}
