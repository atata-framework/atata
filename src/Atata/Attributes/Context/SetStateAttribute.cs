namespace Atata;

/// <summary>
/// Represents an attribute that sets a state object to <see cref="AtataContextBuilder"/>.
/// </summary>
public class SetStateAttribute : AtataContextConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetStateAttribute"/> class.
    /// </summary>
    /// <param name="key">The variable key.</param>
    /// <param name="value">The variable value.</param>
    public SetStateAttribute(string key, object value)
    {
        Guard.ThrowIfNullOrWhitespace(key);

        Key = key;
        Value = value;
    }

    /// <summary>
    /// Gets the state key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the state value.
    /// </summary>
    public object Value { get; }

    protected internal override void ConfigureAtataContext(AtataContextBuilder builder, object? testSuite) =>
        builder.UseState(Key, Value);
}
