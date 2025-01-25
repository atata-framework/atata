#nullable enable

namespace Atata;

/// <summary>
/// Represents an attribute that sets a variable to <see cref="AtataContextBuilder"/>.
/// </summary>
public class SetVariableAttribute : AtataContextConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetVariableAttribute"/> class.
    /// </summary>
    /// <param name="key">The variable key.</param>
    /// <param name="value">The variable value.</param>
    public SetVariableAttribute(string key, object value)
    {
        Key = key.CheckNotNullOrWhitespace(nameof(key));
        Value = value;
    }

    /// <summary>
    /// Gets the variable key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the variable value.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Configures the Atata context by adding the variable.
    /// </summary>
    /// <param name="builder">The Atata context builder.</param>
    public override void ConfigureAtataContext(AtataContextBuilder builder) =>
        builder.AddVariable(Key, Value);
}
