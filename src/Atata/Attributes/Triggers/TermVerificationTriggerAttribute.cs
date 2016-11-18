using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// The base trigger attribute class that can be used in the verification process when the page object is initialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public abstract class TermVerificationTriggerAttribute : TriggerAttribute, ITermDataProvider, ISettingsAttribute
    {
        protected TermVerificationTriggerAttribute(TermCase termCase)
            : this()
        {
            Case = termCase;
        }

        protected TermVerificationTriggerAttribute(TermMatch match, TermCase termCase)
            : this()
        {
            Match = match;
            Case = termCase;
        }

        protected TermVerificationTriggerAttribute(TermMatch match, params string[] values)
            : this(values)
        {
            Match = match;
        }

        protected TermVerificationTriggerAttribute(params string[] values)
            : base(TriggerEvents.Init)
        {
            Values = values;
        }

        public PropertyBag Properties { get; } = new PropertyBag();

        public string[] Values { get; private set; }

        public TermCase Case
        {
            get { return Properties.Get(nameof(Case), DefaultCase, GetSettingsAttributes); }
            set { Properties[nameof(Case)] = value; }
        }

        protected virtual TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public new TermMatch Match
        {
            get { return Properties.Get(nameof(Match), DefaultMatch, GetSettingsAttributes); }
            set { Properties[nameof(Match)] = value; }
        }

        protected virtual TermMatch DefaultMatch
        {
            get { return TermMatch.Equals; }
        }

        public string Format
        {
            get { return Properties.Get<string>(nameof(Format), GetSettingsAttributes); }
            set { Properties[nameof(Format)] = value; }
        }

        protected virtual IEnumerable<ISettingsAttribute> GetSettingsAttributes(UIComponentMetadata metadata)
        {
            yield break;
        }

        protected virtual ITermSettings ResolveTermSettings(UIComponentMetadata metadata)
        {
            return null;
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            string[] expectedValues = this.GetActualValues(context.Component.ComponentName);

            OnExecute(context, expectedValues);
        }

        protected abstract void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
            where TOwner : PageObject<TOwner>;
    }
}
