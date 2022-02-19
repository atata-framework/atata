using System;
using System.Diagnostics;

namespace Atata
{
    /// <summary>
    /// Represents the base object provider class.
    /// </summary>
    /// <typeparam name="TObject">The type of the provided object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner object.</typeparam>
    [DebuggerDisplay("{ProviderName,nq}")]
    public abstract class ObjectProvider<TObject, TOwner> :
        IObjectProvider<TObject, TOwner>,
        IHasProviderName,
        IHasSourceProviderName
    {
        private readonly IObjectSource<TObject> _objectSource;

        private string _sourceProviderName;

        private string _providerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProvider{TValue, TOwner}"/> class.
        /// </summary>
        /// <param name="objectSource">The object source.</param>
        /// <param name="providerName">Name of the provider.</param>
        protected ObjectProvider(IObjectSource<TObject> objectSource, string providerName)
        {
            _objectSource = objectSource.CheckNotNull(nameof(objectSource));
            _providerName = providerName.CheckNotNull(nameof(providerName));
        }

        /// <inheritdoc/>
        public string ProviderName
        {
            get
            {
                string actualSourceProviderName = SourceProviderName;

                return string.IsNullOrEmpty(actualSourceProviderName)
                    ? _providerName
                    : BuildProviderName(actualSourceProviderName, _providerName);
            }
            set => _providerName = value;
        }

        /// <inheritdoc/>
        public string SourceProviderName
        {
            get => _sourceProviderName ?? _objectSource.SourceProviderName;
            set => _sourceProviderName = value;
        }

        /// <inheritdoc/>
        // TODO: Add logging.
        public virtual TObject Value =>
            _objectSource.Value;

        /// <summary>
        /// Gets the owner object.
        /// </summary>
        protected abstract TOwner Owner { get; }

        TOwner IObjectProvider<TObject, TOwner>.Owner => Owner;

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public DataVerificationProvider<TObject, TOwner> Should =>
            CreateVerificationProvider();

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public DataVerificationProvider<TObject, TOwner> ExpectTo =>
            CreateVerificationProvider().Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public DataVerificationProvider<TObject, TOwner> WaitTo =>
            CreateVerificationProvider().Using<WaitingVerificationStrategy>();

        /// <inheritdoc/>
        public bool IsValueDynamic =>
            _objectSource.IsDynamic;

        /// <summary>
        /// Performs an implicit conversion from <see cref="ObjectProvider{TObject, TOwner}"/> to <typeparamref name="TObject"/>.
        /// </summary>
        /// <param name="objectProvider">The object provider.</param>
        /// <returns>The value of the <paramref name="objectProvider"/>.</returns>
        public static implicit operator TObject(ObjectProvider<TObject, TOwner> objectProvider) =>
            objectProvider.Value;

        private static string BuildProviderName(string sourceProviderName, string providerName) =>
            providerName[0] == '['
                ? $"{sourceProviderName}{providerName}"
                : $"{sourceProviderName}.{providerName}";

        private DataVerificationProvider<TObject, TOwner> CreateVerificationProvider() =>
            new DataVerificationProvider<TObject, TOwner>(this);

        /// <summary>
        /// Resolves the current <see cref="AtataContext"/> instance.
        /// Throws <see cref="InvalidOperationException"/> if <see cref="AtataContext.Current"/> is <see langword="null"/>.
        /// </summary>
        /// <returns>An <see cref="AtataContext"/> instance.</returns>
        protected AtataContext ResolveAtataContext() =>
            AtataContext.Current ?? throw new InvalidOperationException(
                $"Failed to resolve {nameof(AtataContext)} as {nameof(AtataContext)}.{nameof(AtataContext.Current)} is null.");

        public override string ToString() =>
            ProviderName;
    }
}
