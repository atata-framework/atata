using System;

namespace Atata
{
    /// <summary>
    /// Represents the lazy object source that gets an object using function once.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public class LazyObjectSource<T> : IObjectSource<T>
    {
        private readonly Lazy<T> _lazyValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyObjectSource{TValue}"/> class.
        /// </summary>
        /// <param name="valueGetFunction">The value get function.</param>
        public LazyObjectSource(Func<T> valueGetFunction)
        {
            valueGetFunction.CheckNotNull(nameof(valueGetFunction));
            _lazyValue = new Lazy<T>(valueGetFunction);
        }

        /// <inheritdoc/>
        public T Value =>
            _lazyValue.Value;

        /// <inheritdoc/>
        public string SourceProviderName => null;

        /// <inheritdoc/>
        public bool IsDynamic => false;
    }
}
