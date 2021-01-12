using System.Drawing;

namespace Atata
{
    public static class SizeIDataVerificationProviderExtensions
    {
        public static TOwner BeGreater<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            int expectedWidth,
            int expectedHeight)
            where TOwner : PageObject<TOwner>
            =>
            should.BeGreater(new Size(expectedWidth, expectedHeight));

        public static TOwner BeGreater<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            Size expected)
            where TOwner : PageObject<TOwner>
            =>
            should.Satisfy(
                actual => actual.Width > expected.Width && actual.Height > expected.Height,
                "be greater than {0}",
                expected);

        public static TOwner BeGreaterOrEqual<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            int expectedWidth,
            int expectedHeight)
            where TOwner : PageObject<TOwner>
            =>
            should.BeGreaterOrEqual(new Size(expectedWidth, expectedHeight));

        public static TOwner BeGreaterOrEqual<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            Size expected)
            where TOwner : PageObject<TOwner>
            =>
            should.Satisfy(
                actual => actual.Width >= expected.Width && actual.Height >= expected.Height,
                "be greater than or equal to {0}",
                expected);

        public static TOwner BeLess<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            int expectedWidth,
            int expectedHeight)
            where TOwner : PageObject<TOwner>
            =>
            should.BeLess(new Size(expectedWidth, expectedHeight));

        public static TOwner BeLess<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            Size expected)
            where TOwner : PageObject<TOwner>
            =>
            should.Satisfy(
                actual => actual.Width < expected.Width && actual.Height < expected.Height,
                "be less than {0}",
                expected);

        public static TOwner BeLessOrEqual<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            int expectedWidth,
            int expectedHeight)
            where TOwner : PageObject<TOwner>
            =>
            should.BeLessOrEqual(new Size(expectedWidth, expectedHeight));

        public static TOwner BeLessOrEqual<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            Size expected)
            where TOwner : PageObject<TOwner>
            =>
            should.Satisfy(
                actual => actual.Width <= expected.Width && actual.Height <= expected.Height,
                "be less than or equal to {0}",
                expected);

        public static TOwner BeInRange<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            int fromWidth,
            int fromHeight,
            int toWidth,
            int toHeight)
            where TOwner : PageObject<TOwner>
            =>
            should.BeInRange(new Size(fromWidth, fromHeight), new Size(toWidth, toHeight));

        public static TOwner BeInRange<TOwner>(
            this IDataVerificationProvider<Size, TOwner> should,
            Size from,
            Size to)
            where TOwner : PageObject<TOwner>
            =>
            should.Satisfy(
                actual => actual.Width >= from.Width && actual.Height >= from.Height && actual.Width <= to.Width && actual.Height <= to.Height,
                "be in range {0} - {1}",
                from,
                to);
    }
}
