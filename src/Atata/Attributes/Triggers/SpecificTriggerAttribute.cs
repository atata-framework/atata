namespace Atata;

public abstract class SpecificTriggerAttribute : TriggerAttribute
{
    protected SpecificTriggerAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal sealed override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        MethodInfo? declaredMethod = GetType().GetMethod("Execute", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (declaredMethod is not null)
        {
            Type ownerType = context.Component.Owner.GetType();

            MethodInfo actualMethod = declaredMethod.IsGenericMethodDefinition
                ? declaredMethod.MakeGenericMethod(ownerType)
                : declaredMethod;

            actualMethod.InvokeWithExceptionUnwrapping(this, [context]);
        }
    }
}
