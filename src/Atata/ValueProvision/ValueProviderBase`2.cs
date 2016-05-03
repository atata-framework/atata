namespace Atata
{
    public abstract class ValueProviderBase<TValue, TOwner> : IUIComponentValueProvider<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected ValueProviderBase(TOwner owner, string componentFullName)
        {
            Owner = owner;
            ComponentFullName = componentFullName;
        }

        public string ComponentFullName { get; private set; }

        public TOwner Owner { get; private set; }

        public abstract string ProviderName { get; }

        public string ConvertValueToString(TValue value)
        {
            return TermResolver.ToDisplayString(value);
        }

        public abstract TValue Get();
    }
}
