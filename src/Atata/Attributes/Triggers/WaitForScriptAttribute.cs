using System;
using System.Text;

namespace Atata
{
    /// <summary>
    /// Represents the base trigger attribute for a waiting for script to be executed successfully.
    /// An inherited class should override <c>BuildScript</c> method and optionally <c>BuildReportMessage</c>.
    /// </summary>
    public abstract class WaitForScriptAttribute : TriggerAttribute
    {
        private const string DefaultReportMessage = "Wait for script";

        private double? timeout;

        private double? retryInterval;

        protected WaitForScriptAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        /// <summary>
        /// Gets or sets the waiting timeout in seconds.
        /// The default value is taken from <c>AtataContext.Current.WaitingTimeout.TotalSeconds</c>.
        /// </summary>
        public double Timeout
        {
            get => timeout ?? (AtataContext.Current?.WaitingTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => timeout = value;
        }

        /// <summary>
        /// Gets or sets the retry interval in seconds.
        /// The default value is taken from <c>AtataContext.Current.WaitingRetryInterval.TotalSeconds</c>.
        /// </summary>
        public double RetryInterval
        {
            get => retryInterval ?? (AtataContext.Current?.WaitingRetryInterval ?? RetrySettings.Interval).TotalSeconds;
            set => retryInterval = value;
        }

        /// <summary>
        /// Builds the report message.
        /// The default message is <c>Wait for script</c>.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="context">The trigger context.</param>
        /// <returns>The message or <see langword="null"/>.</returns>
        protected virtual string BuildReportMessage<TOwner>(TriggerContext<TOwner> context)
            where TOwner : PageObject<TOwner>, IPageObject<TOwner>
        {
            return DefaultReportMessage;
        }

        /// <summary>
        /// Builds the script to wait until it returns <c>true</c>.
        /// The script should return <see langword="true"/> or <see langword="false"/>.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="context">The trigger context.</param>
        /// <returns>The script.</returns>
        protected abstract string BuildScript<TOwner>(TriggerContext<TOwner> context)
            where TOwner : PageObject<TOwner>, IPageObject<TOwner>;

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            string message = BuildReportMessage(context) ?? DefaultReportMessage;
            string script = BuildScript(context);

            context.Log.Start(message, LogLevel.Trace);

            bool isCompleted = context.Driver.Try(TimeSpan.FromSeconds(Timeout), TimeSpan.FromSeconds(RetryInterval)).Until(
                x => (bool)context.Driver.ExecuteScript(script));

            if (!isCompleted)
            {
                StringBuilder errorMessageBuilder = new StringBuilder("Timed out waiting for script.");

                if (message != DefaultReportMessage)
                {
                    errorMessageBuilder.Append(" ").Append(message);

                    if (!message.EndsWith("."))
                        errorMessageBuilder.Append(".");
                }

                throw new TimeoutException(errorMessageBuilder.ToString());
            }

            context.Log.EndSection();
        }
    }
}
