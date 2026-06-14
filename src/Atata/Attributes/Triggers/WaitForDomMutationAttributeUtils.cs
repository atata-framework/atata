namespace Atata;

internal static class WaitForDomMutationAttributeUtils
{
    internal static TimeSpan ResolveImmutableStateTime<TOwner>(
        TriggerContext<TOwner> context,
        double immutableStateSeconds)
        where TOwner : PageObject<TOwner>
        =>
        immutableStateSeconds > 0d
            ? TimeSpan.FromSeconds(immutableStateSeconds)
            : context.Component.Session.WaitForDomImmutableStateTime;

    internal static IUIComponent<TOwner> ResolveTargetComponent<TOwner>(
        TriggerAttribute triggerAttribute,
        TriggerContext<TOwner> context,
        string? controlName)
        where TOwner : PageObject<TOwner>
    {
        if (controlName is null or [])
        {
            return context.Component;
        }
        else
        {
            bool isDefinedAtComponentLevel = context.Component.Metadata.ComponentAttributes.Contains(triggerAttribute);

            bool IsTargetControl(UIComponent<TOwner> component) =>
                component.Metadata.Name == controlName;

            return isDefinedAtComponentLevel || context.Component.Parent is null
                ? context.Component.Controls.First(IsTargetControl)
                : context.Component.Parent.Controls.FirstOrDefault(IsTargetControl)
                    ?? context.Component.Controls.First(IsTargetControl);
        }
    }
}
