namespace Atata;

/// <summary>
/// The exception that is thrown when an attribute cannot be found.
/// </summary>
[Serializable]
public class AttributeNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeNotFoundException"/> class.
    /// </summary>
    public AttributeNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public AttributeNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public AttributeNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeNotFoundException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
    protected AttributeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Creates the <see cref="AttributeNotFoundException"/> for the specified <paramref name="attributeType"/>
    /// and <paramref name="sourceName"/>.
    /// </summary>
    /// <param name="attributeType">Type of the attribute.</param>
    /// <param name="sourceName">Name of the source where the finding of the attribute occurred.</param>
    /// <returns>An instance of <see cref="AttributeNotFoundException"/>.</returns>
    public static AttributeNotFoundException Create(Type attributeType, string sourceName) =>
        new($"{attributeType.FullName} attribute is not found in {sourceName}.");
}
