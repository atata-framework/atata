#nullable enable

namespace Atata;

/// <summary>
/// Defines the method to invoke on the specified event.
/// </summary>
public class InvokeMethodAttribute : TriggerAttribute
{
    public InvokeMethodAttribute(string methodName, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority) =>
        MethodName = methodName.CheckNotNullOrWhitespace(nameof(methodName));

    /// <summary>
    /// Gets the name of the method.
    /// </summary>
    public string MethodName { get; }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        bool isDefinedAtComponentLevel = context.Component.Metadata.ComponentAttributes.Contains(this);

        var methodOwner = isDefinedAtComponentLevel ? context.Component : context.Component.Parent!;
        MethodInfo method = methodOwner.GetType()
            .GetMethodWithThrowOnError(MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        if (method.IsStatic)
            method.InvokeStaticAsLambda();
        else
            method.InvokeAsLambda(methodOwner);
    }
}
