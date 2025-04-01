namespace Atata.UnitTests;

public sealed class EnumExtensionMethodsTests
{
    [Flags]
    public enum Int64EnumValues : long
    {
        None = 0,
        One = 1,
        Four = 4,
        Two = 2,
        Max = long.MaxValue
    }

    [Flags]
    public enum UInt64EnumValues : ulong
    {
        None = 0,
        One = 1,
        Four = 4,
        Two = 2,
        Max = ulong.MaxValue
    }

    [Test]
    public void GetIndividualFlags_None() =>
        Subject.ResultOf(() => Int64EnumValues.None.GetIndividualFlags())
            .Should.BeEmpty();

    [Test]
    public void GetIndividualFlags_One() =>
        Subject.ResultOf(() => Int64EnumValues.Four.GetIndividualFlags())
            .Should.EqualSequence(Int64EnumValues.Four);

    [Test]
    public void GetIndividualFlags_Two() =>
        Subject.ResultOf(() => (Int64EnumValues.Two | Int64EnumValues.One).GetIndividualFlags())
            .Should.EqualSequence(Int64EnumValues.One, Int64EnumValues.Two);

    [Test]
    public void GetIndividualFlags_Int64Enum_Max() =>
        Subject.ResultOf(() => Int64EnumValues.Max.GetIndividualFlags())
            .Should.EqualSequence(Int64EnumValues.One, Int64EnumValues.Two, Int64EnumValues.Four);

    [Test]
    public void GetIndividualFlags_UInt64Enum_Max() =>
        Subject.ResultOf(() => UInt64EnumValues.Max.GetIndividualFlags())
            .Should.EqualSequence(UInt64EnumValues.One, UInt64EnumValues.Two, UInt64EnumValues.Four);
}
