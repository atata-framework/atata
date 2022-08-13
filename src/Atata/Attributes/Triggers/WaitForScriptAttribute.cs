using System;
using System.Text;

namespace Atata
{
    /// <summary>
    /// Represents the base trigger attribute for a waiting for script to be executed successfully.
    /// An inherited class should override <c>BuildScript</c> method and optionally <c>BuildReportMessage</c>.
    /// </summary>
    public abstract class WaitForScriptAttribute : WaitingTriggerAttribute
    {
        private const string DefaultReportMessage = "Wait for script";

        protected WaitForScriptAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        /// <summary>
        /// Builds the report message.
        /// The default message is <c>"Wait for script"</c>.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="context">The trigger context.</param>
        /// <returns>The message or <see langword="null"/>.</returns>
        protected virtual string BuildReportMessage<TOwner>(TriggerContext<TOwner> context)
            where TOwner : PageObject<TOwner>, IPageObject<TOwner>
            =>
            DefaultReportMessage;

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

            void OnExecute()
            {
                bool isCompleted = context.Driver
                    .Try(TimeSpan.FromSeconds(Timeout), TimeSpan.FromSeconds(RetryInterval))
                    .Until(_ => context.Component.Script.Execute<bool>(script).Value);

                if (!isCompleted)
                {
                    StringBuilder errorMessageBuilder = new StringBuilder("Timed out waiting for script.");

                    if (message != DefaultReportMessage)
                    {
                        errorMessageBuilder.Append(' ').Append(message);

                        if (message[message.Length - 1] != '.')
                            errorMessageBuilder.Append('.');
                    }

                    throw new TimeoutException(errorMessageBuilder.ToString());
                }
            }

            if (message != DefaultReportMessage)
            {
                context.Log.ExecuteSection(
                    new LogSection(message),
                    OnExecute);
            }
            else
            {
                OnExecute();
            }
        }
    }
}
