using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Atata
{
    public class SafeWait<T> : IWait<T>
    {
        private readonly T input;

        private readonly IClock clock;

        private readonly List<Type> ignoredExceptions = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeWait{T}"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        public SafeWait(T input)
            : this(input, new SystemClock())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeWait{T}"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="clock">The clock to use when measuring the timeout.</param>
        public SafeWait(T input, IClock clock)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "input cannot be null");
            }

            if (clock == null)
            {
                throw new ArgumentNullException("clock", "input cannot be null");
            }

            this.input = input;
            this.clock = clock;
        }

        /// <summary>
        /// Gets or sets how long to wait for the evaluated condition to be true. The default timeout is 500 milliseconds.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Gets or sets how often the condition should be evaluated. The default timeout is 500 milliseconds.
        /// </summary>
        public TimeSpan PollingInterval { get; set; } = TimeSpan.FromMilliseconds(500);

        public string Message { get; set; }

        /// <summary>
        /// Configures this instance to ignore specific types of exceptions while waiting for a condition.
        /// Any exceptions not whitelisted will be allowed to propagate, terminating the wait.
        /// </summary>
        /// <param name="exceptionTypes">The types of exceptions to ignore.</param>
        public void IgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            if (exceptionTypes == null)
            {
                throw new ArgumentNullException("exceptionTypes", "exceptionTypes cannot be null");
            }

            foreach (Type exceptionType in exceptionTypes)
            {
                if (!typeof(Exception).IsAssignableFrom(exceptionType))
                {
                    throw new ArgumentException("All types to be ignored must derive from System.Exception", "exceptionTypes");
                }
            }

            ignoredExceptions.AddRange(exceptionTypes);
        }

        /// <summary>
        /// Repeatedly applies this instance's input value to the given function until one of the following
        /// occurs:
        /// <para>
        /// <list type="bullet">
        /// <item>the function returns neither null nor false</item>
        /// <item>the function throws an exception that is not in the list of ignored exception types</item>
        /// <item>the timeout expires</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The delegate's expected return type.</typeparam>
        /// <param name="condition">A delegate taking an object of type T as its parameter, and returning a TResult.</param>
        /// <returns>The delegate's return value.</returns>
        public TResult Until<TResult>(Func<T, TResult> condition)
        {
            condition.CheckNotNull(nameof(condition));

            var resultType = typeof(TResult);
            if ((resultType.IsValueType && resultType != typeof(bool)) || !typeof(object).IsAssignableFrom(resultType))
            {
                throw new ArgumentException("Can only wait on an object or boolean response, tried to use type: " + resultType.ToString(), "condition");
            }

            var endTime = clock.LaterBy(Timeout);
            while (true)
            {
                try
                {
                    var result = condition(input);

                    if (DoesConditionResultSatisfy(result))
                        return result;
                }
                catch (Exception ex)
                {
                    if (!IsIgnoredException(ex))
                    {
                        throw;
                    }
                }

                // Check the timeout after evaluating the function to ensure conditions
                // with a zero timeout can succeed.
                if (!clock.IsNowBefore(endTime))
                {
                    if (typeof(TResult) == typeof(ReadOnlyCollection<IWebElement>))
                        return (TResult)(object)new IWebElement[0].ToReadOnly();
                    else if (typeof(TResult) == typeof(IWebElement[]))
                        return (TResult)(object)new IWebElement[0];
                    else
                        return default(TResult);
                }

                Thread.Sleep(PollingInterval);
            }
        }

        protected virtual bool DoesConditionResultSatisfy<TResult>(TResult result)
        {
            if (typeof(TResult) == typeof(bool))
            {
                var boolResult = result as bool?;

                if (boolResult.HasValue && boolResult.Value)
                    return true;
            }
            else if (result != null && (!(result is IEnumerable) || ((IEnumerable)result).Cast<object>().Any()))
            {
                return true;
            }

            return false;
        }

        private bool IsIgnoredException(Exception exception)
        {
            return ignoredExceptions.Any(type => type.IsAssignableFrom(exception.GetType()));
        }
    }
}
