using System;
using System.Reflection;
using OpenQA.Selenium.Remote;

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

        public bool ApplyFixOfCommandExecutionDelay { get; private set; }

        RemoteWebDriver IDriverFactory.Create()
        {
            RemoteWebDriver driver = CreateDriver();

            if (ApplyFixOfCommandExecutionDelay)
                FixDriverCommandExecutionDelay(driver);

            return driver;
        }

        /// <summary>
        /// Creates the driver instance.
        /// </summary>
        /// <returns>The created <see cref="RemoteWebDriver"/> instance.</returns>
        protected abstract RemoteWebDriver CreateDriver();

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

        /// <summary>
        /// Specifies that the fix of driver's HTTP command execution delay should be applied.
        /// There is a bug in Selenium.WebDriver v3.6.0 for .NET Core 2.0: each WebDriver request takes extra 1 second.
        /// Link to the bug: https://github.com/dotnet/corefx/issues/24104.
        /// The fix does: finds <c>HttpCommandExecutor</c> instance of <see cref="RemoteWebDriver"/> instance and updates its <c>remoteServerUri</c> field by replacing "locahost" with "127.0.0.1".
        /// </summary>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithFixOfCommandExecutionDelay()
        {
            ApplyFixOfCommandExecutionDelay = true;
            return (TBuilder)this;
        }

        private static void FixDriverCommandExecutionDelay(RemoteWebDriver driver)
        {
            try
            {
                PropertyInfo commandExecutorProperty = typeof(RemoteWebDriver).GetPropertyWithThrowOnError("CommandExecutor", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
                ICommandExecutor commandExecutor = (ICommandExecutor)commandExecutorProperty.GetValue(driver);

                FieldInfo GetRemoteServerUriField(ICommandExecutor executor)
                {
                    return executor.GetType().GetField("remoteServerUri", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);
                }

                FieldInfo remoteServerUriField = GetRemoteServerUriField(commandExecutor);

                if (remoteServerUriField == null)
                {
                    FieldInfo internalExecutorField = commandExecutor.GetType().GetField("internalExecutor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                    commandExecutor = (ICommandExecutor)internalExecutorField.GetValue(commandExecutor);
                    remoteServerUriField = GetRemoteServerUriField(commandExecutor);
                }

                if (remoteServerUriField != null)
                {
                    string remoteServerUri = remoteServerUriField.GetValue(commandExecutor).ToString();

                    string localhostUriPrefix = "http://localhost";

                    if (remoteServerUri.StartsWith(localhostUriPrefix))
                    {
                        remoteServerUri = remoteServerUri.Replace(localhostUriPrefix, "http://127.0.0.1");

                        remoteServerUriField.SetValue(commandExecutor, new Uri(remoteServerUri));
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                AtataContext.Current?.Log?.Error("Failed to apply fix of command execution delay.", exception);
            }

            AtataContext.Current?.Log?.Error("Failed to apply fix of command execution delay.");
        }
    }
}
