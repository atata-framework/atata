using NUnit.Framework;

namespace Atata.Tests
{
    public class UriProviderTests : UITestFixture
    {
        [Test]
        public void UriProvider_Complex()
        {
            Go.To<OrdinaryPage>(url: "/index?id=15&type=a&type=b&num=1&num=2#some-fragment")
                .AggregateAssert(x => x
                    .PageUri.AbsolutePath.Should.Equal("/index")

                    .PageUri.Query.Parameters.Count.Should.Equal(5)

                    .PageUri.Query.Parameters["id"].Should.Equal("15")
                    .PageUri.Query.Parameters.Get<int>("id").Should.Equal(15)

                    .PageUri.Query.Parameters.GetAll("type").Should.EqualSequence("a", "b")
                    .PageUri.Query.Parameters.GetAll<int>("num").Should.EqualSequence(1, 2)

                    .PageUri.Fragment.Should.Equal("#some-fragment"));
        }

        [Test]
        public void UriProvider_QueryParameters()
        {
            Go.To<OrdinaryPage>(url: "/index?empty1=&empty2&id=1&type=a&type=b&date=1&date=2&")
                .AggregateAssert(x => x
                    .PageUri.Query.Parameters.Count.Should.Equal(7)
                    .PageUri.Query.Parameters["id"].Should.Equal("1")
                    .PageUri.Query.Parameters["empty1"].Should.BeEmpty()
                    .PageUri.Query.Parameters["empty2"].Should.BeEmpty()
                    .PageUri.Query.Parameters.Get<int>("id").Should.Equal(1)
                    .PageUri.Query.Parameters.GetAll("type").Should.EqualSequence("a", "b")
                    .PageUri.Query.Parameters.GetAll<int>("date").Should.EqualSequence(1, 2));
        }

        [Test]
        public void UriProvider_QueryParameters_Empty()
        {
            Go.To<OrdinaryPage>(url: "/")
                .AggregateAssert(x => x
                    .PageUri.Query.Parameters.Count.Should.Equal(0)
                    .PageUri.Query.Parameters["id"].Should.BeNull()
                    .PageUri.Query.Parameters.Get<string>("id").Should.BeNull()
                    .PageUri.Query.Parameters.GetAll<string>("id").Should.HaveCount(0)
                    .PageUri.Fragment.Should.BeEmpty()
                    .PageUri.AbsolutePath.Should.Equal("/"));
        }

        [Test]
        public void UriProvider_Fragment()
        {
            Go.To<OrdinaryPage>(url: "/index#anchor")
                .PageUri.Fragment.Should.Equal("#anchor");
        }

        [Test]
        public void UriProvider_AbsolutePath()
        {
            Go.To<OrdinaryPage>(url: "/index#anchor")
                .PageUri.AbsolutePath.Should.Equal("/index");
        }
    }
}
