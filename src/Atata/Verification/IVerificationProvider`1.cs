using System;

namespace Atata
{
    public interface IVerificationProvider<out TOwner>
    {
        /// <summary>
        /// Gets a value indicating whether the verification is a negation verification.
        /// </summary>
        bool IsNegation { get; }

        /// <summary>
        /// Gets or sets the verification strategy.
        /// </summary>
        IVerificationStrategy Strategy { get; set; }

        /// <summary>
        /// Gets the owner object.
        /// </summary>
        TOwner Owner { get; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the retry interval.
        /// </summary>
        TimeSpan? RetryInterval { get; set; }

        /// <summary>
        /// Gets the should text.
        /// </summary>
        /// <returns>Either <c>"should"</c> or <c>"should not"</c>.</returns>
        string GetShouldText();

        /// <summary>
        /// Gets the retry options.
        /// </summary>
        /// <returns>The retry options.</returns>
        (TimeSpan Timeout, TimeSpan RetryInterval) GetRetryOptions();

        /// <summary>
        /// Reports the failure.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void ReportFailure(string message, Exception exception = null);
    }
}
