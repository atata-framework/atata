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
        private readonly IObjectSource<TObject> objectSource;

        private string sourceProviderName;

        private string providerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProvider{TValue, TOwner}"/> class.
        /// </summary>
        /// <param name="objectSource">The object source.</param>
        /// <param name="providerName">Name of the provider.</param>
        protected ObjectProvider(IObjectSource<TObject> objectSource, string providerName)
        {
            this.objectSource = objectSource.CheckNotNull(nameof(objectSource));
            this.providerName = providerName.CheckNotNull(nameof(providerName));
        }

        /// <inheritdoc/>
        public string ProviderName
        {
            get
            {
                string actualSourceProviderName = SourceProviderName;

                return string.IsNullOrEmpty(actualSourceProviderName)
                    ? providerName
                    : BuildProviderName(actualSourceProviderName, providerName);
            }
            set => providerName = value;
        }

        /// <inheritdoc/>
        public string SourceProviderName
        {
            get => sourceProviderName ?? objectSource.SourceProviderName;
            set => sourceProviderName = value;
        }

        /// <inheritdoc/>
        // TODO: Add logging.
        public virtual TObject Value =>
            objectSource.Value;

        /// <summary>
        /// Gets the owner object.
        /// </summary>
        protected abstract TOwner Owner { get; }

        TOwner IDataProvider<TObject, TOwner>.Owner => Owner;

        // TODO: Remove property.
        UIComponent IDataProvider<TObject, TOwner>.Component => null;

        // TODO: Remove property.
        TermOptions IDataProvider<TObject, TOwner>.ValueTermOptions => null;

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
            objectSource.IsDynamic;

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

        public override string ToString() =>
            ProviderName;
    }
}
