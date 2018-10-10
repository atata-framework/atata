using System;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class UriUtilsTests
    {
        [TestCase("http://something.com", true)]
        [TestCase("https://something.com", true)]
        [TestCase("ftp://something.com", true)]
        [TestCase("custom://something.com", true)]
        [TestCase("http:/something.com", false)]
        [TestCase("//something", false)]
        [TestCase("/something", false)]
        [TestCase("something", false)]
        public void UriUtils_TryCreateAbsoluteUrl(string url, bool isAbsolute)
        {
            var isActuallyAbsolute = UriUtils.TryCreateAbsoluteUrl(url, out Uri result);
            isActuallyAbsolute.Should().Be(isAbsolute);

            if (isAbsolute)
                result.AbsoluteUri.Should().StartWith(url);
            else
                result.Should().BeNull();
        }
    }
}
