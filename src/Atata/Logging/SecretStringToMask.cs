namespace Atata;

/// <summary>
/// Represents the pair of a secret string and a mask that should replace the string.
/// </summary>
public class SecretStringToMask
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecretStringToMask"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="mask">The mask.</param>
    public SecretStringToMask(string value, string mask)
    {
        Value = value;
        Mask = mask;
    }

    /// <summary>
    /// Gets the secret value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the mask.
    /// </summary>
    public string Mask { get; }
}
