namespace Atata;

public static class SizeVerificationProviderExtensions
{
    public static TOwner BeGreater<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        int expectedWidth,
        int expectedHeight)
        =>
        verifier.BeGreater(new Size(expectedWidth, expectedHeight));

    public static TOwner BeGreater<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        Size expected)
        =>
        verifier.Satisfy(
            actual => actual.Width > expected.Width && actual.Height > expected.Height,
            "be greater than {0}",
            expected);

    public static TOwner BeGreaterOrEqual<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        int expectedWidth,
        int expectedHeight)
        =>
        verifier.BeGreaterOrEqual(new Size(expectedWidth, expectedHeight));

    public static TOwner BeGreaterOrEqual<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        Size expected)
        =>
        verifier.Satisfy(
            actual => actual.Width >= expected.Width && actual.Height >= expected.Height,
            "be greater than or equal to {0}",
            expected);

    public static TOwner BeLess<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        int expectedWidth,
        int expectedHeight)
        =>
        verifier.BeLess(new Size(expectedWidth, expectedHeight));

    public static TOwner BeLess<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        Size expected)
        =>
        verifier.Satisfy(
            actual => actual.Width < expected.Width && actual.Height < expected.Height,
            "be less than {0}",
            expected);

    public static TOwner BeLessOrEqual<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        int expectedWidth,
        int expectedHeight)
        =>
        verifier.BeLessOrEqual(new Size(expectedWidth, expectedHeight));

    public static TOwner BeLessOrEqual<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        Size expected)
        =>
        verifier.Satisfy(
            actual => actual.Width <= expected.Width && actual.Height <= expected.Height,
            "be less than or equal to {0}",
            expected);

    public static TOwner BeInRange<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        int fromWidth,
        int fromHeight,
        int toWidth,
        int toHeight)
        =>
        verifier.BeInRange(new Size(fromWidth, fromHeight), new Size(toWidth, toHeight));

    public static TOwner BeInRange<TOwner>(
        this IObjectVerificationProvider<Size, TOwner> verifier,
        Size from,
        Size to)
        =>
        verifier.Satisfy(
            actual => actual.Width >= from.Width && actual.Height >= from.Height && actual.Width <= to.Width && actual.Height <= to.Height,
            "be in range {0} - {1}",
            from,
            to);
}
