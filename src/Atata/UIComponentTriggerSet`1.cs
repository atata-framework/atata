using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public class UIComponentTriggerSet<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;

        private TriggerAttribute[] orderedTriggers;

        internal UIComponentTriggerSet(UIComponent<TOwner> component)
        {
            this.component = component;
        }

        internal List<TriggerAttribute> ComponentTriggersList { get; } = new List<TriggerAttribute>();

        internal List<TriggerAttribute> ParentComponentTriggersList { get; } = new List<TriggerAttribute>();

        internal List<TriggerAttribute> AssemblyTriggersList { get; } = new List<TriggerAttribute>();

        internal List<TriggerAttribute> DeclaredTriggersList { get; } = new List<TriggerAttribute>();

        public IEnumerable<TriggerAttribute> ComponentTriggers => ComponentTriggersList.AsEnumerable();

        public IEnumerable<TriggerAttribute> ParentComponentTriggers => ParentComponentTriggersList.AsEnumerable();

        public IEnumerable<TriggerAttribute> AssemblyTriggers => AssemblyTriggersList.AsEnumerable();

        public IEnumerable<TriggerAttribute> DeclaredTriggers => DeclaredTriggersList.AsEnumerable();

        internal void ApplyMetadata(UIComponentMetadata metadata)
        {
            var allTriggers = ComponentTriggersList.Concat(ParentComponentTriggersList).Concat(AssemblyTriggersList).Concat(DeclaredTriggersList);

            ApplyMetadataToTriggers(metadata, allTriggers);
        }

        public void Add(params TriggerAttribute[] triggers)
        {
            DeclaredTriggersList.AddRange(triggers);

            if (component.Metadata != null)
            {
                ApplyMetadataToTriggers(component.Metadata, triggers);
            }

            Reorder();
        }

        private void ApplyMetadataToTriggers(UIComponentMetadata metadata, IEnumerable<TriggerAttribute> triggers)
        {
            foreach (TriggerAttribute trigger in triggers)
            {
                trigger.ApplyMetadata(metadata);

                IPropertySettings triggerAsPropertySettings = trigger as IPropertySettings;
                if (triggerAsPropertySettings != null)
                    triggerAsPropertySettings.Properties.Metadata = metadata;
            }
        }

        internal void Reorder()
        {
            List<TriggerAttribute> resultTriggers = ComponentTriggersList.
                Where(x => x.AppliesTo == TriggerScope.Self).
                OrderBy(x => x.Priority).
                ToList();

            foreach (TriggerAttribute trigger in resultTriggers)
                trigger.IsDefinedAtComponentLevel = true;

            List<TriggerAttribute> allOtherTriggers = DeclaredTriggersList.
                OrderBy(x => x.Priority).
                Concat(ParentComponentTriggersList.Where(x => x.AppliesTo == TriggerScope.Children).OrderBy(x => x.Priority)).
                Concat(AssemblyTriggersList.OrderBy(x => x.Priority)).
                ToList();

            while (allOtherTriggers.Count > 0)
            {
                TriggerAttribute currentTrigger = allOtherTriggers[0];
                Type currentTriggerType = currentTrigger.GetType();
                TriggerAttribute[] currentTriggersOfSameType = allOtherTriggers.Where(x => x.GetType() == currentTriggerType && x.On == currentTrigger.On).ToArray();

                if (currentTriggersOfSameType.First().On != TriggerEvents.None)
                    resultTriggers.Add(currentTriggersOfSameType.First());

                foreach (TriggerAttribute trigger in currentTriggersOfSameType)
                    allOtherTriggers.Remove(trigger);
            }

            orderedTriggers = resultTriggers.OrderBy(x => x.Priority).ToArray();
        }

        internal void Execute(TriggerEvents on)
        {
            if (orderedTriggers == null || orderedTriggers.Length == 0 || on == TriggerEvents.None)
                return;

            var triggers = orderedTriggers.Where(x => x.On.HasFlag(on));

            TriggerContext<TOwner> context = new TriggerContext<TOwner>
            {
                Event = on,
                Driver = component.Driver,
                Log = component.Log,
                Component = component
            };

            foreach (var trigger in triggers)
                trigger.Execute(context);

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
