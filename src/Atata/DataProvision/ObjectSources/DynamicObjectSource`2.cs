using System;

namespace Atata
{
    /// <summary>
    /// Represents the dynamic object source that gets an object using function.
    /// Also takes an instance of <see cref="IObjectProvider{TObject}" /> of source.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public class DynamicObjectSource<TObject, TSource> : IObjectSource<TObject>
    {
        private readonly IObjectProvider<TSource> _sourceProvider;

        private readonly Func<TSource, TObject> _valueGetFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicObjectSource{TObject, TSource}"/> class.
        /// </summary>
        /// <param name="sourceProvider">The source provider.</param>
        /// <param name="valueGetFunction">The value get function.</param>
        public DynamicObjectSource(IObjectProvider<TSource> sourceProvider, Func<TSource, TObject> valueGetFunction)
        {
            _sourceProvider = sourceProvider.CheckNotNull(nameof(sourceProvider));
            _valueGetFunction = valueGetFunction.CheckNotNull(nameof(valueGetFunction));
        }

        /// <inheritdoc/>
        public TObject Value =>
            _valueGetFunction.Invoke(_sourceProvider.Object);

        /// <inheritdoc/>
        public string SourceProviderName =>
            _sourceProvider.ProviderName;

        /// <inheritdoc/>
        public bool IsDynamic => true;
    }
}
