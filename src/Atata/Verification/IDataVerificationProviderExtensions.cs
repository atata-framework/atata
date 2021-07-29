using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Atata
{
    public static class IDataVerificationProviderExtensions
    {
        public static TOwner Satisfy<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, Predicate<TData> predicate, string message, params TData[] args)
        {
            should.CheckNotNull(nameof(should));
            predicate.CheckNotNull(nameof(predicate));

            void ExecuteVerification()
            {
                TData actual = default;
                Exception exception = null;

                bool doesSatisfy = ExecuteUntil(
                    () =>
                    {
                        try
                        {
                            actual = should.DataProvider.Value;
                            bool result = predicate(actual) != should.IsNegation;
                            exception = null;
                            return result;
                        }
                        catch (Exception e)
                        {
                            exception = e;
                            return false;
                        }
                    },
                    should.GetRetryOptions());

                if (!doesSatisfy)
                {
                    string expectedMessage = VerificationUtils.BuildExpectedMessage(message, args?.Cast<object>().ToArray());
                    string actualMessage = exception == null ? Stringifier.ToString(actual) : null;

                    string failureMessage = VerificationUtils.BuildFailureMessage(should, expectedMessage, actualMessage);

                    should.ReportFailure(failureMessage, exception);
                }
            }

            if (AtataContext.Current is null)
            {
                ExecuteVerification();
            }
            else
            {
                string verificationConstraintMessage = VerificationUtils.BuildConstraintMessage(should, message, args);

                LogSection logSection = should.DataProvider.Component is null
                    ? (LogSection)new ValueVerificationLogSection(should.VerificationKind, should.DataProvider.ProviderName, verificationConstraintMessage)
                    : new VerificationLogSection(should.VerificationKind, should.DataProvider.Component, should.DataProvider.ProviderName, verificationConstraintMessage);

                AtataContext.Current.Log.ExecuteSection(logSection, ExecuteVerification);
            }

            return should.Owner;
        }

        public static TOwner Satisfy<TData, TOwner>(
            this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should,
            Predicate<IEnumerable<TData>> predicate,
            string message,
            params TData[] args)
        {
            should.CheckNotNull(nameof(should));
            predicate.CheckNotNull(nameof(predicate));

            string expectedMessage = (args != null && args.Any())
                ? message?.FormatWith(Stringifier.ToString(args))
                : message;

            void ExecuteVerification()
            {
                IEnumerable<TData> actual = null;
                Exception exception = null;

                bool doesSatisfy = ExecuteUntil(
                    () =>
                    {
                        try
                        {
                            actual = should.DataProvider.Value?.Select(x => x.Value).ToArray();
                            bool result = predicate(actual) != should.IsNegation;
                            exception = null;
                            return result;
                        }
                        catch (Exception e)
                        {
                            exception = e;
                            return false;
                        }
                    },
                    should.GetRetryOptions());

                if (!doesSatisfy)
                {
                    string actualMessage = exception == null ? Stringifier.ToString(actual) : null;

                    string failureMessage = VerificationUtils.BuildFailureMessage(should, expectedMessage, actualMessage);

                    should.ReportFailure(failureMessage, exception);
                }
            }

            if (AtataContext.Current is null)
            {
                ExecuteVerification();
            }
            else
            {
                string verificationConstraintMessage = $"{should.GetShouldText()} {expectedMessage}";

                LogSection logSection = should.DataProvider.Component is null
                    ? (LogSection)new ValueVerificationLogSection(should.VerificationKind, should.DataProvider.ProviderName, verificationConstraintMessage)
                    : new VerificationLogSection(should.VerificationKind, should.DataProvider.Component, should.DataProvider.ProviderName, verificationConstraintMessage);

                AtataContext.Current.Log.ExecuteSection(logSection, ExecuteVerification);
            }

            return should.Owner;
        }

        private static bool ExecuteUntil(Func<bool> condition, RetryOptions retryOptions)
        {
            var wait = CreateSafeWait(retryOptions);
            return wait.Until(_ => condition());
        }

        private static SafeWait<object> CreateSafeWait(RetryOptions options)
        {
            var wait = new SafeWait<object>(string.Empty)
            {
                Timeout = options.Timeout,
                PollingInterval = options.Interval
            };

            foreach (Type exceptionType in options.IgnoredExceptionTypes)
                wait.IgnoreExceptionTypes(exceptionType);

            return wait;
        }

        public static IDataVerificationProvider<TData, TOwner> WithSettings<TData, TOwner>(
            this IDataVerificationProvider<TData, TOwner> should,
            IVerificationProvider<TOwner> sourceVerificationProvider)
        {
            should.CheckNotNull(nameof(should));
            sourceVerificationProvider.CheckNotNull(nameof(sourceVerificationProvider));

            IDataVerificationProvider<TData, TOwner> resultVerificationProvider =
                sourceVerificationProvider.IsNegation && !should.IsNegation
                    ? GetNegationVerificationProvider(should)
                    : should;

            resultVerificationProvider.Strategy = sourceVerificationProvider.Strategy;
            resultVerificationProvider.Timeout = sourceVerificationProvider.Timeout;
            resultVerificationProvider.RetryInterval = sourceVerificationProvider.RetryInterval;

            return resultVerificationProvider;
        }

        private static IDataVerificationProvider<TData, TOwner> GetNegationVerificationProvider<TData, TOwner>(IDataVerificationProvider<TData, TOwner> verificationProvider)
        {
            if (verificationProvider is DataVerificationProvider<TData, TOwner> dataVerificationProvider)
                return dataVerificationProvider.Not;
            else
                return (IDataVerificationProvider<TData, TOwner>)verificationProvider.GetType().GetPropertyWithThrowOnError("Not").GetValue(verificationProvider);
        }

        public static TOwner Equal<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
        {
            return should.Satisfy(actual => Equals(actual, expected), "equal {0}", expected);
        }

        public static TOwner BeTrue<TOwner>(this IDataVerificationProvider<bool, TOwner> should)
        {
            return should.Satisfy(actual => actual, "be true");
        }

        public static TOwner BeTrue<TOwner>(this IDataVerificationProvider<bool?, TOwner> should)
        {
            return should.Satisfy(actual => actual == true, "be true");
        }

        public static TOwner BeFalse<TOwner>(this IDataVerificationProvider<bool, TOwner> should)
        {
            return should.Satisfy(actual => !actual, "be false");
        }

        public static TOwner BeFalse<TOwner>(this IDataVerificationProvider<bool?, TOwner> should)
        {
            return should.Satisfy(actual => actual == false, "be false");
        }

        public static TOwner BeNull<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should)
        {
            return should.Satisfy(actual => Equals(actual, null), "be null");
        }

        public static TOwner BeNullOrEmpty<TOwner>(this IDataVerificationProvider<string, TOwner> should)
        {
            return should.Satisfy(actual => string.IsNullOrEmpty(actual), "be null or empty");
        }

        public static TOwner BeNullOrWhiteSpace<TOwner>(this IDataVerificationProvider<string, TOwner> should)
        {
            return should.Satisfy(actual => string.IsNullOrWhiteSpace(actual), "be null or white-space");
        }

        public static TOwner EqualIgnoringCase<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
        {
            return should.Satisfy(actual => string.Equals(expected, actual, StringComparison.CurrentCultureIgnoreCase), "equal {0} ignoring case", expected);
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.Contains(expected), "contain {0}", expected);
        }

        public static TOwner ContainIgnoringCase<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.ToUpper().Contains(expected.ToUpper()), "contain {0} ignoring case", expected);
        }

        public static TOwner StartWith<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.StartsWith(expected), "start with {0}", expected);
        }

        public static TOwner StartWithIgnoringCase<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase), "start with {0} ignoring case", expected);
        }

        public static TOwner EndWith<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.EndsWith(expected), "end with {0}", expected);
        }

        public static TOwner EndWithIgnoringCase<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expected)
        {
            expected.CheckNotNull(nameof(expected));

            return should.Satisfy(actual => actual != null && actual.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase), "end with {0} ignoring case", expected);
        }

        public static TOwner Match<TOwner>(this IDataVerificationProvider<string, TOwner> should, string pattern)
        {
            pattern.CheckNotNull(nameof(pattern));

            return should.Satisfy(actual => actual != null && Regex.IsMatch(actual, pattern), $"match pattern \"{pattern}\"");
        }

        public static TOwner BeGreater<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(expected) > 0, "be greater than {0}", expected);
        }

        public static TOwner BeGreater<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) > 0, "be greater than {0}", expected);
        }

        public static TOwner BeGreaterOrEqual<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(expected) >= 0, "be greater than or equal to {0}", expected);
        }

        public static TOwner BeGreaterOrEqual<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) >= 0, "be greater than or equal to {0}", expected);
        }

        public static TOwner BeLess<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(expected) < 0, "be less than {0}", expected);
        }

        public static TOwner BeLess<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) < 0, "be less than {0}", expected);
        }

        public static TOwner BeLessOrEqual<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expected)
            where TData : IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(expected) <= 0, "be less than or equal to {0}", expected);
        }

        public static TOwner BeLessOrEqual<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData expected)
            where TData : struct, IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(expected) <= 0, "be less than or equal to {0}", expected);
        }

        public static TOwner BeInRange<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData from, TData to)
            where TData : IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.CompareTo(from) >= 0 && actual.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);
        }

        public static TOwner BeInRange<TData, TOwner>(this IDataVerificationProvider<TData?, TOwner> should, TData from, TData to)
            where TData : struct, IComparable<TData>, IComparable
        {
            return should.Satisfy(actual => actual != null && actual.Value.CompareTo(from) >= 0 && actual.Value.CompareTo(to) <= 0, "be in range {0} - {1}", from, to);
        }

        public static TOwner EqualDate<TOwner>(this IDataVerificationProvider<DateTime, TOwner> should, DateTime expected)
        {
            return should.Satisfy(actual => Equals(actual.Date, expected.Date), "equal date {0}", expected);
        }

        public static TOwner EqualDate<TOwner>(this IDataVerificationProvider<DateTime?, TOwner> should, DateTime expected)
        {
            return should.Satisfy(actual => actual != null && Equals(actual.Value.Date, expected.Date), "equal date {0}", expected);
        }

        public static TOwner MatchAny<TOwner>(this IDataVerificationProvider<string, TOwner> should, TermMatch match, params string[] expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            var predicate = match.GetPredicate();

            string message = new StringBuilder().
                Append($"{match.GetShouldText()} ").
                AppendIf(expected.Length > 1, "any of: ").
                AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}")).
                ToString();

            return should.Satisfy(actual => actual != null && expected.Any(x => predicate(actual, x)), message, expected);
        }

        public static TOwner ContainAll<TOwner>(this IDataVerificationProvider<string, TOwner> should, params string[] expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            string message = new StringBuilder().
                Append($"contain ").
                AppendIf(expected.Length > 1, "all of: ").
                AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}")).
                ToString();

            return should.Satisfy(actual => actual != null && expected.All(x => actual.Contains(x)), message, expected);
        }

        public static TOwner BeEmpty<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should)
        {
            return should.Satisfy(actual => actual != null && !actual.Any(), "be empty");
        }

        public static TOwner HaveCount<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, int expected)
        {
            return should.Satisfy(actual => actual != null && actual.Count() == expected, $"have count {expected}");
        }

        public static TOwner HaveLength<TOwner>(this IDataVerificationProvider<string, TOwner> should, int expected)
        {
            return should.Satisfy(actual => actual != null && actual.Length == expected, $"have length of {expected}");
        }

        public static TOwner BeEquivalent<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, params TData[] expected)
        {
            return should.BeEquivalent(expected.AsEnumerable());
        }

        public static TOwner BeEquivalent<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, IEnumerable<TData> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {Stringifier.ToString(expected)}");
        }

        public static TOwner BeEquivalent<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should, params TData[] expected)
        {
            return should.BeEquivalent(expected.AsEnumerable());
        }

        public static TOwner BeEquivalent<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should, IEnumerable<TData> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.Count() == expected.Count() && actual.All(expected.Contains),
                $"be equivalent to {Stringifier.ToString(expected)}");
        }

        public static TOwner EqualSequence<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, params TData[] expected)
        {
            return should.EqualSequence(expected.AsEnumerable());
        }

        public static TOwner EqualSequence<TData, TOwner>(this IDataVerificationProvider<IEnumerable<TData>, TOwner> should, IEnumerable<TData> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {Stringifier.ToString(expected)}");
        }

        public static TOwner EqualSequence<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should, params TData[] expected)
        {
            return should.EqualSequence(expected.AsEnumerable());
        }

        public static TOwner EqualSequence<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should, IEnumerable<TData> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && actual.SequenceEqual(expected),
                $"equal sequence {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains only a single item.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should)
        {
            return should.Satisfy(actual => actual != null && actual.Count() == 1, $"contain single item");
        }

        /// <summary>
        /// Verifies that collection contains a single item equal to <paramref name="expected"/> parameter.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expected">An expected item value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should, TItem expected)
        {
            return should.Satisfy(
                actual => actual != null && actual.Count(x => Equals(x, expected)) == 1,
                $"contain single {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains a single item equal to <paramref name="expected"/> parameter.
        /// </summary>
        /// <typeparam name="TData">The type of the collection item data.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expected">An expected data value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should, TData expected)
        {
            return should.Satisfy(
                actual => actual != null && actual.Count((TData x) => Equals(x, expected)) == 1,
                $"contain single {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains a single item matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainSingle<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should, Expression<Func<TItem, bool>> predicateExpression)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return should.Satisfy(
                actual => actual != null && actual.Count(predicate) == 1,
                $"contain single {Stringifier.ToString(predicateExpression)}");
        }

        /// <summary>
        /// Verifies that collection contains exact count of items equal to <paramref name="expectedValue"/> parameter.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expectedCount">The expected count of items.</param>
        /// <param name="expectedValue">An expected item value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainExactly<TItem, TOwner>(
            this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should,
            int expectedCount,
            TItem expectedValue)
        {
            return should.Satisfy(
                actual => actual != null && actual.Count(x => Equals(x, expectedValue)) == expectedCount,
                $"contain exactly {expectedCount} {Stringifier.ToString(expectedValue)} items");
        }

        /// <summary>
        /// Verifies that collection contains exact count of items equal to <paramref name="expectedValue"/> parameter.
        /// </summary>
        /// <typeparam name="TData">The type of the collection item data.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expectedCount">The expected count of items.</param>
        /// <param name="expectedValue">An expected data value.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainExactly<TData, TOwner>(
            this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should,
            int expectedCount,
            TData expectedValue)
        {
            return should.Satisfy(
                actual => actual != null && actual.Count((TData x) => Equals(x, expectedValue)) == expectedCount,
                $"contain exactly {expectedCount} {Stringifier.ToString(expectedValue)} items");
        }

        /// <summary>
        /// Verifies that collection contains exact count of items matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expectedCount">The expected count of items.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner ContainExactly<TItem, TOwner>(
            this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should,
            int expectedCount,
            Expression<Func<TItem, bool>> predicateExpression)
        {
            expectedCount.CheckGreaterOrEqual(nameof(expectedCount), 0);
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return should.Satisfy(
                actual => actual != null && actual.Count(predicate) == expectedCount,
                $"contain exactly {expectedCount} {Stringifier.ToString(predicateExpression)} items");
        }

        /// <summary>
        /// Verifies that collection contains expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should, params TItem[] expected)
        {
            return should.Contain(expected.AsEnumerable());
        }

        /// <summary>
        /// Verifies that collection contains expected items.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expected">An expected item values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should, IEnumerable<TItem> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && should.IsNegation
                    ? actual.Intersect(expected).Any()
                    : actual.Intersect(expected).Count() == expected.Count(),
                $"contain {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains items equal to expected values.
        /// </summary>
        /// <typeparam name="TData">The type of the collection item data.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expected">An expected data values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should, params TData[] expected)
        {
            return should.Contain(expected.AsEnumerable());
        }

        /// <summary>
        /// Verifies that collection contains items equal to expected values.
        /// </summary>
        /// <typeparam name="TData">The type of the collection item data.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="expected">An expected data values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> should, IEnumerable<TData> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && should.IsNegation
                    ? actual.Intersect(expected).Any()
                    : actual.Intersect(expected).Count() == expected.Count(),
                $"contain {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection contains at least one item matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> should, Expression<Func<TItem, bool>> predicateExpression)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return should.Satisfy(
                actual => actual != null && actual.Any(predicate),
                $"contain {Stringifier.ToString(predicateExpression)}");
        }

        /// <summary>
        /// Verifies that collection contains at least one item matching <paramref name="predicateExpression"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the collection item object.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The should instance.</param>
        /// <param name="predicateExpression">The predicate expression.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner Contain<TObject, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TObject>>, TOwner> should, Expression<Func<TObject, bool>> predicateExpression)
        {
            var predicate = predicateExpression.CheckNotNull(nameof(predicateExpression)).Compile();

            return should.Satisfy(
                actual => actual != null && actual.Any(predicate),
                $"contain {Stringifier.ToString(predicateExpression)}");
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<IEnumerable<string>, TOwner> should, TermMatch match, params string[] expected)
        {
            return should.Contain(match, expected.AsEnumerable());
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<IEnumerable<string>, TOwner> should, TermMatch match, IEnumerable<string> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && should.IsNegation
                    ? expected.Any(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                    : expected.All(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue))),
                $"contain having value that {match.ToString(TermCase.MidSentence)} {Stringifier.ToString(expected)}");
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<string>>, TOwner> should, TermMatch match, params string[] expected)
        {
            return should.Contain(match, expected.AsEnumerable());
        }

        public static TOwner Contain<TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<string>>, TOwner> should, TermMatch match, IEnumerable<string> expected)
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual => actual != null && should.IsNegation
                    ? expected.Any(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                    : expected.All(expectedValue => actual.Any(actualValue => match.IsMatch(actualValue, expectedValue))),
                $"contain having value that {match.ToString(TermCase.MidSentence)} {Stringifier.ToString(expected)}");
        }

        public static TOwner ContainHavingContent<TControl, TOwner>(this IDataVerificationProvider<IEnumerable<TControl>, TOwner> should, TermMatch match, params string[] expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return should.ContainHavingContent(match, expected.AsEnumerable());
        }

        public static TOwner ContainHavingContent<TControl, TOwner>(this IDataVerificationProvider<IEnumerable<TControl>, TOwner> should, TermMatch match, IEnumerable<string> expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            return should.Satisfy(
                actual =>
                {
                    if (actual == null)
                        return false;

                    var actualValues = actual.Select(x => x.Content.Value).ToArray();
                    return should.IsNegation
                        ? expected.Any(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)))
                        : expected.All(expectedValue => actualValues.Any(actualValue => match.IsMatch(actualValue, expectedValue)));
                },
                $"contain having content that {match.ToString(TermCase.MidSentence)} {Stringifier.ToString(expected)}");
        }

        /// <summary>
        /// Verifies that collection is sorted in ascending order.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInAscendingOrder<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> verifier)
            where TItem : IComparable<TItem>
        {
            return verifier.Satisfy(
                actual => actual != null && actual.OrderBy(x => x).SequenceEqual(actual),
                "be in ascending order");
        }

        /// <summary>
        /// Verifies that collection is sorted in ascending order.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item of nullable value (struct) type.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInAscendingOrder<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem?>, TOwner> verifier)
            where TItem : struct, IComparable<TItem>
        {
            return verifier.Satisfy(
                actual => actual != null && actual.OrderBy(x => x).SequenceEqual(actual),
                "be in ascending order");
        }

        /// <summary>
        /// Verifies that collection is sorted in ascending order.
        /// </summary>
        /// <typeparam name="TData">The type of the collection item data.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInAscendingOrder<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> verifier)
        {
            return verifier.Satisfy(
                (IEnumerable<TData> actual) => actual != null && actual.OrderBy(x => x).SequenceEqual(actual),
                "be in ascending order");
        }

        /// <summary>
        /// Verifies that collection is sorted in descending order.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInDescendingOrder<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem>, TOwner> verifier)
            where TItem : IComparable<TItem>
        {
            return verifier.Satisfy(
                actual => actual != null && actual.OrderByDescending(x => x).SequenceEqual(actual),
                "be in descending order");
        }

        /// <summary>
        /// Verifies that collection is sorted in descending order.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item of nullable value (struct) type.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInDescendingOrder<TItem, TOwner>(this IDataVerificationProvider<IEnumerable<TItem?>, TOwner> verifier)
            where TItem : struct, IComparable<TItem>
        {
            return verifier.Satisfy(
                actual => actual != null && actual.OrderByDescending(x => x).SequenceEqual(actual),
                "be in descending order");
        }

        /// <summary>
        /// Verifies that collection is sorted in descending order.
        /// </summary>
        /// <typeparam name="TData">The type of the collection item data.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeInDescendingOrder<TData, TOwner>(this IDataVerificationProvider<IEnumerable<IObjectProvider<TData>>, TOwner> verifier)
        {
            return verifier.Satisfy(
                (IEnumerable<TData> actual) => actual != null && actual.OrderByDescending(x => x).SequenceEqual(actual),
                "be in descending order");
        }
    }
}
