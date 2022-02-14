using OpenQA.Selenium;

namespace Atata
{
    public abstract class DriverAtataContextBuilder<TBuilder> : AtataContextBuilder, IDriverFactory
        where TBuilder : DriverAtataContextBuilder<TBuilder>
    {
        protected DriverAtataContextBuilder(AtataBuildingContext buildingContext, string alias = null)
            : base(buildingContext)
        {
            Alias = alias;
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        public string Alias { get; private set; }

        IWebDriver IDriverFactory.Create() =>
            CreateDriver();

        /// <summary>
        /// Creates the driver instance.
        /// </summary>
        /// <returns>The created <see cref="IWebDriver"/> instance.</returns>
        protected abstract IWebDriver CreateDriver();

        /// <summary>
        /// Specifies the driver alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithAlias(string alias)
        {
            alias.CheckNotNullOrWhitespace(nameof(alias));

            Alias = alias;
            return (TBuilder)this;
        }
    }
}
