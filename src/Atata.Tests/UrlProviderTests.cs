using NUnit.Framework;

namespace Atata.Tests
{
    public class UrlProviderTests : UITestFixture
    {
        [Test]
        public void UrlProvider_QueryParameters()
        {
            Go.To<OrdinaryPage>(url: "/index?empty=&id=1&type=a&type=b&date=1&date=2&").
                AggregateAssert(x => x
                    .PageUrl.QueryParameters.Count.Should.Equal(6)
                    .PageUrl.QueryParameters["id"].Should.Equal("1")
                    .PageUrl.QueryParameters["empty"].Should.BeEmpty()
                    .PageUrl.QueryParameters.Get<int>("id").Should.Equal(1)
                    .PageUrl.QueryParameters.GetAll("type").Should.EqualSequence("a", "b")
                    .PageUrl.QueryParameters.GetAll<int>("date").Should.EqualSequence(1, 2));
        }

        [Test]
        public void UrlProvider_QueryParameters_Empty()
        {
            Go.To<OrdinaryPage>(url: "/").
                AggregateAssert(x => x
                    .PageUrl.QueryParameters.Count.Should.Equal(0)
                    .PageUrl.QueryParameters["id"].Should.BeNull()
                    .PageUrl.QueryParameters.Get<string>("id").Should.BeNull()
                    .PageUrl.QueryParameters.GetAll<string>("id").Should.HaveCount(0)
                    .PageUrl.Fragment.Should.BeEmpty()
                    .PageUrl.AbsolutePath.Should.Equal("/"));
        }

        [Test]
        public void UrlProvider_Fragment()
        {
            Go.To<OrdinaryPage>(url: "/index#anchor").
                PageUrl.Fragment.Should.Equal("#anchor");
        }

        [Test]
        public void UrlProvider_AbsolutePath()
        {
            Go.To<OrdinaryPage>(url: "/index#anchor").
                PageUrl.AbsolutePath.Should.Equal("/index");
        }
    }
}
