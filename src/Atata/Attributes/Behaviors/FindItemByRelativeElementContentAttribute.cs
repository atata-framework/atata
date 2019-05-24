using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior to find an item of <see cref="OptionList{T, TOwner}"/> control by relative element content using its XPath.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FindItemByRelativeElementContentAttribute : TermSettingsAttribute, IFindItemAttribute
    {
        public FindItemByRelativeElementContentAttribute(string relativeElementXPath)
        {
            RelativeElementXPath = relativeElementXPath;
        }

        public FindItemByRelativeElementContentAttribute(string relativeElementXPath, TermCase termCase)
            : base(termCase)
        {
            RelativeElementXPath = relativeElementXPath;
        }

        public FindItemByRelativeElementContentAttribute(string relativeElementXPath, TermMatch match)
            : base(match)
        {
            RelativeElementXPath = relativeElementXPath;
        }

        public FindItemByRelativeElementContentAttribute(string relativeElementXPath, TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
            RelativeElementXPath = relativeElementXPath;
        }

        /// <summary>
        /// Gets the relative element XPath.
        /// </summary>
        public string RelativeElementXPath { get; }

        public IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata)
        {
            return new FindItemByRelativeElementContentStrategy(RelativeElementXPath);
        }
    }
}
