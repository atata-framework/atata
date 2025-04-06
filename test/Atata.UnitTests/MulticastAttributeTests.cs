namespace Atata.UnitTests;

public static class MulticastAttributeTests
{
    public class TestMulticastAttribute : MulticastAttribute
    {
    }

    public class CalculateTargetRank
    {
        private UIComponentMetadata _metadata = null!;

        [SetUp]
        public void SetUp()
        {
            _metadata = new UIComponentMetadata(
                TargetNames.Some, TargetTypes.Some, TargetParentTypes.Some);
            _metadata.Push(new TagAttribute(TargetTags.Some));
        }

        [Test]
        public void Default() =>
            Test(_ => { })
                .Should().Be(0);

        [Test]
        public void TargetName_Match() =>
            Test(x => x.TargetName = TargetNames.Some)
                .Should().BePositive();

        [Test]
        public void TargetName_NoMatch() =>
            Test(x => x.TargetName = TargetNames.Other)
                .Should().BeNull();

        [Test]
        public void TargetType_Match() =>
            Test(x => x.TargetType = TargetTypes.Some)
                .Should().BePositive();

        [Test]
        public void TargetType_NoMatch() =>
            Test(x => x.TargetType = TargetTypes.Other)
                .Should().BeNull();

        [Test]
        public void TargetTag_Match() =>
            Test(x => x.TargetTag = TargetTags.Some)
                .Should().BePositive();

        [Test]
        public void TargetTag_NoMatch() =>
            Test(x => x.TargetTag = TargetTags.Other)
                .Should().BeNull();

        [Test]
        public void TargetParentType_Match() =>
            Test(x => x.TargetParentType = TargetParentTypes.Some)
                .Should().BePositive();

        [Test]
        public void TargetParentType_NoMatch() =>
            Test(x => x.TargetParentType = TargetParentTypes.Other)
                .Should().BeNull();

        [Test]
        public void ExcludeTargetName_Match() =>
            Test(x => x.ExcludeTargetName = TargetNames.Some)
                .Should().BeNull();

        [Test]
        public void ExcludeTargetName_NoMatch() =>
            Test(x => x.ExcludeTargetName = TargetNames.Other)
                .Should().Be(0);

        [Test]
        public void ExcludeTargetType_Match() =>
            Test(x => x.ExcludeTargetType = TargetTypes.Some)
                .Should().BeNull();

        [Test]
        public void ExcludeTargetType_NoMatch() =>
            Test(x => x.ExcludeTargetType = TargetTypes.Other)
                .Should().Be(0);

        [Test]
        public void ExcludeTargetTag_Match() =>
            Test(x => x.ExcludeTargetTag = TargetTags.Some)
                .Should().BeNull();

        [Test]
        public void ExcludeTargetTag_NoMatch() =>
            Test(x => x.ExcludeTargetTag = TargetTags.Other)
                .Should().Be(0);

        [Test]
        public void ExcludeTargetParentType_Match() =>
            Test(x => x.ExcludeTargetParentType = TargetParentTypes.Some)
                .Should().BeNull();

        [Test]
        public void ExcludeTargetParentType_NoMatch() =>
            Test(x => x.ExcludeTargetType = TargetParentTypes.Other)
                .Should().Be(0);

        private int? Test(Action<TestMulticastAttribute> sutInitializer)
        {
            var sut = new TestMulticastAttribute();
            sutInitializer?.Invoke(sut);
            return sut.CalculateTargetRank(_metadata);
        }
    }

    public class IsTargetSpecified
    {
        [Test]
        public void Default() =>
            Test(_ => { })
                .Should().BeFalse();

        [Test]
        public void TargetName() =>
            Test(x => x.TargetName = TargetNames.Some)
                .Should().BeTrue();

        [Test]
        public void TargetType() =>
            Test(x => x.TargetType = TargetTypes.Some)
                .Should().BeTrue();

        [Test]
        public void TargetTag() =>
            Test(x => x.TargetTag = TargetTags.Some)
                .Should().BeTrue();

        [Test]
        public void TargetParentType() =>
            Test(x => x.TargetParentType = TargetParentTypes.Some)
                .Should().BeTrue();

        [Test]
        public void ExcludeTargetName() =>
            Test(x => x.ExcludeTargetName = TargetNames.Some)
                .Should().BeTrue();

        [Test]
        public void ExcludeTargetType() =>
            Test(x => x.ExcludeTargetType = TargetTypes.Some)
                .Should().BeTrue();

        [Test]
        public void ExcludeTargetTag() =>
            Test(x => x.ExcludeTargetTag = TargetTags.Some)
                .Should().BeTrue();

        [Test]
        public void ExcludeTargetParentType() =>
            Test(x => x.ExcludeTargetParentType = TargetParentTypes.Some)
                .Should().BeTrue();

        private static bool Test(Action<TestMulticastAttribute> sutInitializer)
        {
            var sut = new TestMulticastAttribute();
            sutInitializer?.Invoke(sut);
            return sut.IsTargetSpecified;
        }
    }

    private static class TargetNames
    {
        public const string Some = "Some";

        public const string Other = "Other";
    }

    private static class TargetTypes
    {
        public static readonly Type Some = typeof(Input<,>);

        public static readonly Type Other = typeof(Button<>);
    }

    private static class TargetTags
    {
        public const string Some = "sometag";

        public const string Other = "othertag";
    }

    private static class TargetParentTypes
    {
        public static readonly Type Some = typeof(OrdinaryPage);

        public static readonly Type Other = typeof(TestPage);
    }
}
