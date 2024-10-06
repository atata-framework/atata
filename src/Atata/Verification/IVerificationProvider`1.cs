namespace Atata;

public interface IVerificationProvider<out TOwner>
{
    /// <summary>
    /// Gets a value indicating whether the verification is a negation verification.
    /// </summary>
    bool IsNegation { get; }

    /// <summary>
    /// Gets or sets the verification strategy.
    /// </summary>
    IVerificationStrategy Strategy { get; set; }

    /// <summary>
    /// Gets the owner object.
    /// </summary>
    TOwner Owner { get; }

    /// <summary>
    /// Gets the associated execution unit.
    /// </summary>
    IAtataExecutionUnit ExecutionUnit { get; }

    /// <summary>
    /// Gets or sets the timeout.
    /// </summary>
    TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Gets or sets the retry interval.
    /// </summary>
    TimeSpan? RetryInterval { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Dictionary{TKey, TValue}"/> of <see cref="Type"/> and <see cref="IEqualityComparer{T}"/>.
    /// Can be <see langword="null"/>.
    /// </summary>
    Dictionary<Type, object> TypeEqualityComparerMap { get; set; }

    /// <summary>
    /// Gets the retry options.
    /// </summary>
    /// <returns>The retry options.</returns>
    (TimeSpan Timeout, TimeSpan RetryInterval) GetRetryOptions();

    /// <summary>
    /// Resolves the <see cref="IEqualityComparer{T}"/> for the specified <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">The type of object to compare.</typeparam>
    /// <returns>
    /// An instance of registered <see cref="IEqualityComparer{T}"/>
    /// or the <see cref="EqualityComparer{T}.Default"/> when the specific one cannot be found.
    /// </returns>
    IEqualityComparer<T> ResolveEqualityComparer<T>();

    /// <summary>
    /// Resolves the <see cref="StringComparison"/>.
    /// </summary>
    /// <returns>A registered <see cref="StringComparison"/> or <see cref="StringComparison.Ordinal"/> by default.</returns>
    StringComparison ResolveStringComparison();
}
