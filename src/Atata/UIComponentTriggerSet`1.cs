using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public class UIComponentTriggerSet<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private static readonly Dictionary<TriggerEvents, TriggerEvents[]> DenyTriggersMap = new Dictionary<TriggerEvents, TriggerEvents[]>
        {
            [TriggerEvents.BeforeAccess] = new[] { TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess },
            [TriggerEvents.AfterAccess] = new[] { TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess }
        };

        private readonly UIComponent<TOwner> component;

        private readonly List<TriggerEvents> currentDeniedTriggers = new List<TriggerEvents>();

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

        public IEnumerable<TriggerAttribute> AllTriggers => orderedTriggers.AsEnumerable();

        private List<TriggerAttribute>[] AllTriggersLists => new[]
        {
            DeclaredTriggersList,
            ParentComponentTriggersList,
            AssemblyTriggersList,
            ComponentTriggersList
        };

        internal void ApplyMetadata(UIComponentMetadata metadata)
        {
            var allTriggers = ComponentTriggersList.Concat(ParentComponentTriggersList).Concat(AssemblyTriggersList).Concat(DeclaredTriggersList);

            ApplyMetadataToTriggers(metadata, allTriggers);
        }

        public void Add(params TriggerAttribute[] triggers)
        {
            var triggersListToAddTo = component.Metadata != null ? DeclaredTriggersList : ComponentTriggersList;

            triggersListToAddTo.AddRange(triggers);

            if (component.Metadata != null)
                ApplyMetadataToTriggers(component.Metadata, triggers);

            Reorder();
        }

        public bool Remove(params TriggerAttribute[] triggers)
        {
            var allTriggersLists = AllTriggersLists;
            bool isRemoved = false;

            foreach (TriggerAttribute trigger in triggers)
            {
                isRemoved |= allTriggersLists.Aggregate(false, (removed, list) => list.Remove(trigger) || removed);
            }

            Reorder();
            return isRemoved;
        }

        public int RemoveAll(Predicate<TriggerAttribute> match)
        {
            match.CheckNotNull(nameof(match));

            int count = AllTriggersLists.Sum(list => list.RemoveAll(match));

            Reorder();
            return count;
        }

        private static void ApplyMetadataToTriggers(UIComponentMetadata metadata, IEnumerable<TriggerAttribute> triggers)
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
            foreach (TriggerAttribute trigger in ComponentTriggersList)
                trigger.IsDefinedAtComponentLevel = true;

            orderedTriggers = DeclaredTriggersList.
                Concat(ParentComponentTriggersList).
                Concat(AssemblyTriggersList).
                Concat(ComponentTriggersList).
                OrderBy(x => x.Priority).
                ToArray();
        }

        internal void Execute(TriggerEvents on)
        {
            if (on == TriggerEvents.None || currentDeniedTriggers.Contains(on))
                return;

            if (orderedTriggers?.Length > 0)
            {
                TriggerEvents[] denyTriggers;
                if (DenyTriggersMap.TryGetValue(on, out denyTriggers))
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
                        trigger.Execute(context);
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
