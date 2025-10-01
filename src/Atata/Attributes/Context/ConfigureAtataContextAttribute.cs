namespace Atata;

/// <summary>
/// Represents an attribute that configures <see cref="AtataContext"/> via a specified static or instance method.
/// The first parameter of the method must be of type <see cref="AtataContextBuilder"/>.
/// If the <see cref="TargetType"/> is not specified, then the method is searched in the test suite type.
/// If the <see cref="MethodName"/> is not specified, then the default method name <c>"Configure"</c> is used.
/// </summary>
public class ConfigureAtataContextAttribute : AtataContextConfigurationAttribute
{
    /// <summary>
    /// The default configuration method name, which is <c>"Configure"</c>.
    /// </summary>
    public const string DefaultMethodName = "Configure";

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureAtataContextAttribute"/> class
    /// using the specified method name.
    /// </summary>
    /// <param name="methodName">The name of the configuration method.</param>
    public ConfigureAtataContextAttribute(string methodName)
        : this(null, methodName, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureAtataContextAttribute"/> class
    /// using the specified target type.
    /// </summary>
    /// <param name="targetType">The type that contains the configuration method.</param>
    public ConfigureAtataContextAttribute(Type targetType)
        : this(targetType, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureAtataContextAttribute"/> class
    /// using the specified method name and method parameters.
    /// </summary>
    /// <param name="methodName">The name of the configuration method.</param>
    /// <param name="methodParameters">The parameters to pass to the configuration method.</param>
    public ConfigureAtataContextAttribute(string methodName, object?[]? methodParameters)
        : this(null, methodName, methodParameters)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureAtataContextAttribute"/> class
    /// using the specified target type and method name.
    /// </summary>
    /// <param name="targetType">The type that contains the configuration method.</param>
    /// <param name="methodName">The name of the configuration method.</param>
    public ConfigureAtataContextAttribute(Type targetType, string methodName)
        : this(targetType, methodName, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureAtataContextAttribute"/> class
    /// using the specified target type, method name, and method parameters.
    /// </summary>
    /// <param name="targetType">The type that contains the configuration method.</param>
    /// <param name="methodName">The name of the configuration method.</param>
    /// <param name="methodParameters">The parameters to pass to the configuration method.</param>
    public ConfigureAtataContextAttribute(Type? targetType, string? methodName, object?[]? methodParameters)
    {
        TargetType = targetType;
        MethodName = methodName;
        MethodParameters = methodParameters;
    }

    /// <summary>
    /// Gets the type that contains the configuration method.
    /// </summary>
    public Type? TargetType { get; }

    /// <summary>
    /// Gets the name of the configuration method.
    /// </summary>
    public string? MethodName { get; }

    /// <summary>
    /// Gets the parameters to pass to the configuration method.
    /// </summary>
    public object?[]? MethodParameters { get; }

    /// <summary>
    /// Invokes the targeted method passing <paramref name="builder"/> to it.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> instance.</param>
    /// <param name="testSuite">The test suite object.</param>
    protected internal override void ConfigureAtataContext(AtataContextBuilder builder, object? testSuite)
    {
        if (TargetType is null && testSuite is null)
            throw new InvalidOperationException($"'{nameof(TargetType)}' must be specified in {GetType().Name}.");

        if (TargetType is null && MethodName is null)
            throw new InvalidOperationException($"Either '{nameof(TargetType)}' or '{nameof(MethodName)}' must be specified in {GetType().Name}.");

        Type targetType = TargetType ?? testSuite!.GetType();
        string methodName = MethodName ?? DefaultMethodName;

        MethodInfo method = targetType.GetMethodWithThrowOnError(
            methodName,
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        object? instance = method.IsStatic
            ? null
            : TargetType is null
                ? testSuite
                : ActivatorEx.CreateInstance(TargetType);

        object?[] methodParameters = MethodParameters is null
            ? [builder]
            : [builder, .. MethodParameters];

        method.InvokeWithExceptionUnwrapping(instance, methodParameters);
    }
}
