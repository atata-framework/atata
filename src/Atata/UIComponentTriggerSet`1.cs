using System.Collections.Generic;

namespace Atata
{
    public class UIComponentTriggerSet<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;

        private readonly List<TriggerAttribute> componentTriggers = new List<TriggerAttribute>();

        private readonly List<TriggerAttribute> parentComponentTriggers = new List<TriggerAttribute>();

        private readonly List<TriggerAttribute> assemblyTriggers = new List<TriggerAttribute>();

        private readonly List<TriggerAttribute> declaredTriggers = new List<TriggerAttribute>();

        private UIComponentTriggerSet(UIComponent<TOwner> component)
        {
            this.component = component;
        }

        public IEnumerable<TriggerAttribute> ComponentTriggers => componentTriggers;

        public IEnumerable<TriggerAttribute> ParentComponentTriggers => parentComponentTriggers;

        public IEnumerable<TriggerAttribute> AssemblyTriggers => assemblyTriggers;

        public IEnumerable<TriggerAttribute> DeclaredTriggers => declaredTriggers;

        internal static UIComponentTriggerSet<TOwner> CreateForPageObject(
            UIComponent<TOwner> pageObject,
            TriggerAttribute[] componentTriggers,
            TriggerAttribute[] assemblyTriggers)
        {
            UIComponentTriggerSet<TOwner> manager = new UIComponentTriggerSet<TOwner>(pageObject);

            manager.componentTriggers.AddRange(componentTriggers);
            manager.assemblyTriggers.AddRange(assemblyTriggers);

            return manager;
        }

        internal static UIComponentTriggerSet<TOwner> CreateForControl(
            UIComponent<TOwner> control,
            TriggerAttribute[] componentTriggers,
            TriggerAttribute[] parentComponentTriggers,
            TriggerAttribute[] assemblyTriggers,
            TriggerAttribute[] declaredTriggers)
        {
            UIComponentTriggerSet<TOwner> manager = new UIComponentTriggerSet<TOwner>(control);

            manager.componentTriggers.AddRange(componentTriggers);
            manager.parentComponentTriggers.AddRange(parentComponentTriggers);
            manager.assemblyTriggers.AddRange(assemblyTriggers);
            manager.declaredTriggers.AddRange(declaredTriggers);

            return manager;
        }

        public void Add(params TriggerAttribute[] triggers)
        {
            declaredTriggers.AddRange(triggers);
        }

        public void Execute(TriggerEvents on)
        {
            ////if (Triggers == null || Triggers.Length == 0 || on == TriggerEvents.None)
            ////    return;

            TriggerContext<TOwner> context = new TriggerContext<TOwner>
            {
                Event = on,
                Driver = component.Driver,
                Log = component.Log,
                Component = component
            };

            ////var triggers = Triggers.Where(x => x.On.HasFlag(on));

            ////foreach (var trigger in triggers)
            ////    trigger.Execute(context);

            if (on == TriggerEvents.Init || on == TriggerEvents.DeInit)
            {
                foreach (UIComponent<TOwner> child in component.Controls)
                {
                    child.TriggerSet.Execute(on);
                }
            }
        }
    }
}
