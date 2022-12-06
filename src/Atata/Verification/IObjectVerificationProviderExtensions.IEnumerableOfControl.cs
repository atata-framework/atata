using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public static partial class IObjectVerificationProviderExtensions
    {
        public static TOwner ContainHavingContent<TControl, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TControl>, TOwner> verifier,
            TermMatch match,
            params string[] expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            verifier.ContainHavingContent(match, expected?.AsEnumerable());

        public static TOwner ContainHavingContent<TControl, TOwner>(
            this IObjectVerificationProvider<IEnumerable<TControl>, TOwner> verifier,
            TermMatch match,
            IEnumerable<string> expected)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            expected.CheckNotNullOrEmpty(nameof(expected));

            var predicate = match.GetPredicate(verifier.ResolveStringComparison());

            return verifier.Satisfy(
                actual =>
                {
                    if (actual == null)
                        return false;

                    var actualValues = actual.Select(x => x.Content.Value).ToArray();
                    return verifier.IsNegation
                        ? expected.Any(expectedValue => actualValues.Any(actualValue => predicate(actualValue, expectedValue)))
                        : expected.All(expectedValue => actualValues.Any(actualValue => predicate(actualValue, expectedValue)));
                },
                $"contain having content that {match.ToString(TermCase.MidSentence)} {Stringifier.ToStringInFormOfOneOrMany(expected)}");
        }
    }
}
