namespace Atata.MSTest;

/// <summary>
/// Indicates that the attributed method configures the <see cref="AtataContext"/> for the test suite.
/// </summary>
/// <remarks>
/// This attribute is intended to be applied to public static methods
/// that set up or modify the <see cref="AtataContext"/> at the suite level.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ConfiguresSuiteAtataContextAttribute : Attribute
{
}
