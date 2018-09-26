using System;
using System.Collections.Generic;
using System.Linq;
using _ = Atata.Tests.TriggersPage;

namespace Atata.Tests
{
    [Url("Triggers.html")]
    [VerifyTitle]
    [VerifyH1]
    [InvokeMethod(nameof(OnStaticInit), TriggerEvents.Init)]
    [LogInfo("BeforeGet-Medium", TriggerEvents.BeforeGet, AppliesTo = TriggerScope.Children)]
    [LogInfo("AfterGet-Medium", TriggerEvents.AfterGet, AppliesTo = TriggerScope.Children)]
    public class TriggersPage : Page<_>
    {
        [ThreadStatic]
        private static bool isOnInitInvoked;

        public TriggersPage()
        {
            Triggers.Add(new LogInfoAttribute("Init-Lowest", TriggerEvents.Init, TriggerPriority.Lowest));
            Triggers.Add(new LogInfoAttribute("Init-Low", TriggerEvents.Init, TriggerPriority.Low));

            DynamicInput = Controls.Create<TextInput<_>>(
                nameof(DynamicInput).ToString(TermCase.MidSentence),
                new FindFirstAttribute(),
                new LogInfoAttribute("AfterGet-Lower", TriggerEvents.AfterGet, TriggerPriority.Lower));

            DynamicInput.Triggers.Add(new LogInfoAttribute("AfterGet-Low", TriggerEvents.AfterGet, TriggerPriority.Low));
        }

        public static bool IsOnInitInvoked => isOnInitInvoked;

        public bool IsBeforePerformInvoked { get; private set; }

        public bool IsAfterPerformInvoked { get; private set; }

        public List<TriggerEvents> InputEvents { get; private set; } = new List<TriggerEvents>();

        [InvokeMethod(nameof(OnBeforePerform), TriggerEvents.BeforeClick)]
        [InvokeMethod(nameof(OnAfterPerform), TriggerEvents.AfterClick)]
        public Button<_> Perform { get; private set; }

        [Term("Perform")]
        public Button<_> PerformWithoutTriggers { get; private set; }

        public Link<GoTo1Page, _> GoTo1 { get; private set; }

        [FindFirst]
        public TextInput<_> Input { get; private set; }

        public TextInput<_> DynamicInput { get; private set; }

        [FindFirst]
        [LogInfo("AfterSet-Highest", TriggerEvents.AfterSet, TriggerPriority.Highest)]
        [CustomLogInfo("AfterSet-Higher", TriggerEvents.AfterSet, TriggerPriority.Higher)]
        [LogInfo("AfterSet-High", TriggerEvents.AfterSet, TriggerPriority.High)]
        [CustomLogInfo("AfterSet-Medium", TriggerEvents.AfterSet, TriggerPriority.Medium)]
        [LogInfo("AfterSet-Low", TriggerEvents.AfterSet, TriggerPriority.Low)]
        [CustomLogInfo("AfterSet-Lower", TriggerEvents.AfterSet, TriggerPriority.Lower)]
        [LogInfo("AfterSet-Lowest", TriggerEvents.AfterSet, TriggerPriority.Lowest)]
        public TextInput<_> InputWithLogging { get; private set; }

        [FindById]
        [WriteTriggerEvent(TriggerEvents.BeforeAccess | TriggerEvents.AfterAccess)]
        public TextInput<_> MissingInput { get; private set; }

        [FindById]
        public HierarchyControl Hierarchy { get; private set; }

        protected override void OnInit()
        {
            Triggers.Add(new LogInfoAttribute("Init-Lower", TriggerEvents.Init, TriggerPriority.Lower));

            TriggerEvents allEvents = typeof(TriggerEvents).GetIndividualEnumFlags().Cast<TriggerEvents>().Aggregate((a, b) => a | b);
            Input.Triggers.Add(new WriteTriggerEventAttribute(allEvents));
        }

        public static void OnStaticInit()
        {
            isOnInitInvoked = true;
        }

        public void OnBeforePerform()
        {
            IsBeforePerformInvoked = true;
        }

        public void OnAfterPerform()
        {
            IsAfterPerformInvoked = true;
        }

        public class WriteTriggerEventAttribute : SpecificTriggerAttribute
        {
            public WriteTriggerEventAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
                : base(on, priority)
            {
            }

            protected void Execute(TriggerContext<_> context)
            {
                context.Component.Owner.InputEvents.Add(context.Event);
            }
        }

        public class CustomLogInfoAttribute : TriggerAttribute
        {
            public CustomLogInfoAttribute(string message, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
                : base(on, priority)
            {
                Message = message;
            }

            public string Message { get; private set; }

            protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
            {
                context.Log.Info(Message);
            }
        }

        public class HierarchyControl : Control<_>
        {
            public LevelItem1 Level1 { get; private set; }

            public class LevelItem1 : LevelItem
            {
                public LevelItem2 Level2 { get; private set; }

                public class LevelItem2 : LevelItem
                {
                    public LevelItem3 Level3 { get; private set; }

                    public class LevelItem3 : LevelItem
                    {
                        public LevelItem4 Level4 { get; private set; }

                        public class LevelItem4 : LevelItem
                        {
                        }
                    }
                }
            }
        }

        [ControlDefinition("li", ComponentTypeName = "item")]
        [ControlFinding(FindTermBy.Id)]
        [HoverParent(AppliesTo = TriggerScope.Children)]
        public class LevelItem : Control<_>
        {
        }
    }
}
