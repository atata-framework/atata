using System;

namespace Atata
{
    /// <summary>
    /// Defines the settings to apply for the specified finding strategy of control. Applies to the class (inheritor of <see cref="UIComponent{TOwner}"/>) and assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class TermFindSettingsAttribute : FindSettingsAttribute, ITermSettings
    {
        public TermFindSettingsAttribute(FindTermBy by, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : this(by.ResolveFindAttributeType(), match, termCase)
        {
        }

        public TermFindSettingsAttribute(Type findAttributeType, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(findAttributeType)
        {
            Match = match;
            Case = termCase;
        }

        public TermCase Case { get; set; }

        public new TermMatch Match { get; set; }

        public string Format { get; set; }
    }
}
