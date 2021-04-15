using System;

namespace Atata
{
    /// <summary>
    /// Represents the lazy object source that gets an object using function once.
    /// Also takes an instance of <see cref="IObjectProvider{TObject}" /> of source.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public class LazyObjectSource<TObject, TSource> : IObjectSource<TObject>
    {
        private readonly IObjectProvider<TSource> sourceProvider;

        private readonly Lazy<TObject> lazyValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyObjectSource{TValue, TSource}"/> class.
        /// </summary>
        /// <param name="sourceProvider">The source provider.</param>
        /// <param name="valueGetFunction">The value get function.</param>
        public LazyObjectSource(IObjectProvider<TSource> sourceProvider, Func<TSource, TObject> valueGetFunction)
        {
            this.sourceProvider = sourceProvider.CheckNotNull(nameof(sourceProvider));

            valueGetFunction.CheckNotNull(nameof(valueGetFunction));

            lazyValue = new Lazy<TObject>(() => valueGetFunction.Invoke(this.sourceProvider.Value));
            this.sourceProvider = sourceProvider;
        }

        /// <inheritdoc/>
        public TObject Value =>
            lazyValue.Value;

        /// <inheritdoc/>
        public string SourceProviderName =>
            sourceProvider.ProviderName;

        /// <inheritdoc/>
        public bool IsDynamic => false;
    }
}
