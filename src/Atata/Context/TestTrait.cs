namespace Atata;

/// <summary>
/// Represents a test trait, which is a name-value pair used to categorize or describe a test.
/// </summary>
public sealed record class TestTrait
{
    /// <summary>
    /// The name of the category trait, which is <c>"Category"</c>.
    /// </summary>
    public const string CategoryName = "Category";

    /// <summary>
    /// Initializes a new instance of the <see cref="TestTrait"/> class.
    /// </summary>
    /// <param name="name">The name of the trait.</param>
    /// <param name="value">The value of the trait. Can be <see langword="null"/>.</param>
    public TestTrait(string name, string? value)
    {
        Guard.ThrowIfNull(name);

        Name = name;
        Value = value;
    }

    /// <summary>
    /// Gets the name of the trait.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the value of the trait.
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// Returns a string that represents the current trait.
    /// </summary>
    /// <returns>
    /// The <see cref="Value"/> if the trait is a category; otherwise, a string in the format <c>Name=Value</c>.
    /// </returns>
    public override string ToString() =>
        Name == CategoryName
            ? Value ?? string.Empty
            : $"{Name}={Value}";
}
