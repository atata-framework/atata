using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;

namespace Atata
{
    public static class AtataContextBuilderExtensions
    {
        /// <summary>
        /// Use the <see cref="ChromeDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public static ChromeAtataContextBuilder UseChrome(this AtataContextBuilder builder)
        {
            return new ChromeAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="FirefoxDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public static FirefoxAtataContextBuilder UseFirefox(this AtataContextBuilder builder)
        {
            return new FirefoxAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="InternetExplorerDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="InternetExplorerAtataContextBuilder"/> instance.</returns>
        public static InternetExplorerAtataContextBuilder UseInternetExplorer(this AtataContextBuilder builder)
        {
            return new InternetExplorerAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="EdgeDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public static EdgeAtataContextBuilder UseEdge(this AtataContextBuilder builder)
        {
            return new EdgeAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="OperaDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public static OperaAtataContextBuilder UseOpera(this AtataContextBuilder builder)
        {
            return new OperaAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="PhantomJSDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public static PhantomJSAtataContextBuilder UsePhantomJS(this AtataContextBuilder builder)
        {
            return new PhantomJSAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="SafariDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public static SafariAtataContextBuilder UseSafari(this AtataContextBuilder builder)
        {
            return new SafariAtataContextBuilder(builder.BuildingContext);
        }
    }
}
