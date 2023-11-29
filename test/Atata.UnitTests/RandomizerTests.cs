namespace Atata.UnitTests;

[TestFixture]
public class RandomizerTests
{
    [TestCase(null, 1)]
    [TestCase(null, 5)]
    [TestCase(null, 35)]
    [TestCase(null, 70)]
    [TestCase("s", 2)]
    [TestCase("{0}", 1)]
    [TestCase("start{0}end", 9)]
    [TestCase("start{0}end", 70)]
    public void GetString(string format, int length)
    {
        string result = Randomizer.GetString(format, length);

        result.Should().HaveLength(length);

        if (format is not null and not "{0}")
            result.Should().ContainAll(format.Split(["{0}"], StringSplitOptions.RemoveEmptyEntries));
    }

    [TestCase(null, -1)]
    [TestCase(null, 0)]
    [TestCase("s", 1)]
    [TestCase("s{0}", 1)]
    [TestCase("start{0}end", 8)]
    public void GetString_ThrowsArgumentException(string format, int length) =>
        Assert.Throws<ArgumentException>(() => Randomizer.GetString(format, length));
}
