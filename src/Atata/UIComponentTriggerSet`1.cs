using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public class UIComponentTriggerSet<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;

        private readonly List<TriggerEvents> currentDeniedTriggers = new List<TriggerEvents>();

        internal UIComponentTriggerSet(UIComponent<TOwner> component)
        {
            this.component = component;
        }

        public IEnumerable<TriggerAttribute> ComponentTriggers =>
            component.Metadata?.GetAll<TriggerAttribute>(x => x.At(AttributeLevels.Component)).OrderBy(x => x.Priority);

        public IEnumerable<TriggerAttribute> ParentComponentTriggers =>
            component.Metadata?.GetAll<TriggerAttribute>(x => x.At(AttributeLevels.ParentComponent)).OrderBy(x => x.Priority);

        public IEnumerable<TriggerAttribute> AssemblyTriggers =>
            component.Metadata?.GetAll<TriggerAttribute>(x => x.At(AttributeLevels.Assembly)).OrderBy(x => x.Priority);

        public IEnumerable<TriggerAttribute> GlobalTriggers =>
            component.Metadata?.GetAll<TriggerAttribute>(x => x.At(AttributeLevels.Global)).OrderBy(x => x.Priority);

        public IEnumerable<TriggerAttribute> DeclaredTriggers =>
            component.Metadata?.GetAll<TriggerAttribute>(x => x.At(AttributeLevels.Declared)).OrderBy(x => x.Priority);

        public IEnumerable<TriggerAttribute> AllTriggers =>
            component.Metadata?.GetAll<TriggerAttribute>().OrderBy(x => x.Priority);

        [Obsolete("Use component.Metadata.Add method instead.")] // Obsolete since v1.8.0.
        public void Add(params TriggerAttribute[] triggers)
        {
            component.Metadata.Add(triggers);
        }

        [Obsolete("Use component.Metadata.Remove method instead.")] // Obsolete since v1.8.0.
        public bool Remove(params TriggerAttribute[] triggers)
        {
            return component.Metadata.Remove(triggers);
        }

        [Obsolete("Use component.Metadata.RemoveAll method instead.")] // Obsolete since v1.8.0.
        public int RemoveAll(Predicate<TriggerAttribute> match)
        {
            return component.Metadata.RemoveAll(x => x is TriggerAttribute trigger && match(trigger));
        }

        internal void Execute(TriggerEvents on)
        {
            if (on == TriggerEvents.None || currentDeniedTriggers.Contains(on))
                return;

            var orderedTriggers = AllTriggers;

            if (orderedTriggers.Any())
            {
                if (DenyTriggersMap.Values.TryGetValue(on, out TriggerEvents[] denyTriggers))
                    currentDeniedTriggers.AddRange(denyTriggers);

                try
                {
                    var triggers = orderedTriggers.Where(x => x.On.HasFlag(on));

                    TriggerContext<TOwner> context = new TriggerContext<TOwner>
                    {
                        Event = on,
                        Driver = component.Driver,
                        Log = component.Log,
                        Component = component
                    };

                    foreach (var trigger in triggers)
                    {
                        trigger.Properties.Metadata = component.Metadata;

#pragma warning disable CS0618 // Type or member is obsolete
                        trigger.ApplyMetadata(component.Metadata);
#pragma warning restore CS0618 // Type or member is obsolete

                        component.Log.ExecuteSection(
                            new ExecuteTriggerLogSection(component, trigger, on),
                            () => trigger.Execute(context));
                    }
                }
                finally
                {
                    if (denyTriggers != null)
                        currentDeniedTriggers.RemoveAll(x => denyTriggers.Contains(x));
                }
            }

            ExecuteForChildren(on);
        }

        private void ExecuteForChildren(TriggerEvents on)
        {
            if (on == TriggerEvents.Init || on == TriggerEvents.DeInit)
            {
                foreach (UIComponent<TOwner> child in component.Controls)
                {
                    child.Triggers.Execute(on);
                }
            }
        }
    }
}
